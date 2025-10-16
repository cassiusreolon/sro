using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace sro.Services;

public interface IApiExternaAuthService
{
    Task<HttpClient> GetAuthenticatedHttpClientAsync();
    Task<string> GetAccessTokenAsync();
}

public class ApiExternaAuthService : IApiExternaAuthService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiExternaAuthService> _logger;
    private string? _cachedToken;
    private DateTime _tokenExpiration;
    private readonly SemaphoreSlim _tokenSemaphore = new(1, 1);

    public ApiExternaAuthService(IConfiguration configuration, ILogger<ApiExternaAuthService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<HttpClient> GetAuthenticatedHttpClientAsync()
    {
        var handler = new HttpClientHandler();
        
        // Configurar certificado cliente
        var certificate = LoadCertificate();
        if (certificate != null)
        {
            handler.ClientCertificates.Add(certificate);
            _logger.LogInformation("Certificado cliente carregado com sucesso");
        }

        var httpClient = new HttpClient(handler);
        
        // Configurar timeout
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        
        // Obter token de acesso
        var token = await GetAccessTokenAsync();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Headers padrão
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        return httpClient;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        await _tokenSemaphore.WaitAsync();
        try
        {
            // Verifica se o token ainda é válido (com margem de 5 minutos)
            if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiration.AddMinutes(-5))
            {
                return _cachedToken;
            }

            // Obtém novo token
            _cachedToken = await RequestNewTokenAsync();
            return _cachedToken;
        }
        finally
        {
            _tokenSemaphore.Release();
        }
    }

    private async Task<string> RequestNewTokenAsync()
    {
        var clientId = _configuration["ApiExterna:ClientId"];
        var clientSecret = _configuration["ApiExterna:ClientSecret"];
        var tokenUrl = _configuration["ApiExterna:TokenUrl"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(tokenUrl))
        {
            throw new InvalidOperationException("Configurações de autenticação não encontradas");
        }

        // Criar HttpClientHandler com certificado para a requisição do token
        var handler = new HttpClientHandler();
        var certificate = LoadCertificate();
        if (certificate != null)
        {
            handler.ClientCertificates.Add(certificate);
            _logger.LogInformation("Usando certificado cliente para autenticação do token");
        }
        else
        {
            _logger.LogWarning("Nenhum certificado configurado para autenticação do token");
        }

        using var httpClient = new HttpClient(handler);
        
        var tokenRequest = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret)
        });

        try
        {
            _logger.LogInformation("Solicitando novo token de acesso para URL: {TokenUrl}", tokenUrl);
            _logger.LogDebug("Client ID: {ClientId}", clientId);
            
            var response = await httpClient.PostAsync(tokenUrl, tokenRequest);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogDebug("Resposta do servidor de token - Status: {StatusCode}, Content: {Content}", 
                response.StatusCode, responseContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Erro ao obter token: {StatusCode} - {Content}", response.StatusCode, responseContent);
                
                // Fornecer mais detalhes sobre o erro
                var errorDetails = $"Status: {response.StatusCode}, Resposta: {responseContent}";
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    errorDetails += "\n\nPossíveis causas do erro Forbidden:\n" +
                                  "1. Client ID ou Client Secret incorretos\n" +
                                  "2. Certificado não configurado ou inválido\n" +
                                  "3. Certificado expirado\n" +
                                  "4. IP não autorizado na B3\n" +
                                  "5. Ambiente incorreto (produção vs homologação)";
                }
                
                throw new HttpRequestException($"Erro ao obter token: {errorDetails}");
            }

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
            
            if (tokenResponse?.AccessToken == null)
            {
                throw new InvalidOperationException("Token de acesso não encontrado na resposta");
            }

            // Define expiração do token (padrão 1 hora se não especificado)
            var expiresIn = tokenResponse.ExpiresIn ?? 3600;
            _tokenExpiration = DateTime.UtcNow.AddSeconds(expiresIn);
            
            _logger.LogInformation("Token obtido com sucesso. Expira em: {Expiration}", _tokenExpiration);
            
            return tokenResponse.AccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao solicitar token de acesso");
            throw;
        }
    }

    private X509Certificate2? LoadCertificate()
    {
        var certificatePath = _configuration["ApiExterna:CertificadoPath"];
        var certificatePassword = _configuration["ApiExterna:CertificadoPassword"];
        var keyPath = _configuration["ApiExterna:CertificadoKeyPath"];
        var cerPath = _configuration["ApiExterna:CertificadoCerPath"];

        try
        {
            // Opção 1: Arquivo .p12 ou .pfx (mais comum)
            if (!string.IsNullOrEmpty(certificatePath) && File.Exists(certificatePath))
            {
                _logger.LogInformation("Carregando certificado .p12/.pfx: {Path}", certificatePath);
                return new X509Certificate2(certificatePath, certificatePassword);
            }

            // Opção 2: Arquivos separados .key + .cer
            if (!string.IsNullOrEmpty(keyPath) && !string.IsNullOrEmpty(cerPath) && 
                File.Exists(keyPath) && File.Exists(cerPath))
            {
                _logger.LogInformation("Carregando certificado .key + .cer: {KeyPath}, {CerPath}", keyPath, cerPath);
                
                // Carrega o certificado .cer
                var certificate = new X509Certificate2(cerPath);
                
                // Carrega a chave privada .key
                var keyContent = File.ReadAllText(keyPath);
                
                // Remove headers e footers se existirem
                keyContent = keyContent
                    .Replace("-----BEGIN PRIVATE KEY-----", "")
                    .Replace("-----END PRIVATE KEY-----", "")
                    .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                    .Replace("-----END RSA PRIVATE KEY-----", "")
                    .Replace("\r", "")
                    .Replace("\n", "");

                var keyBytes = Convert.FromBase64String(keyContent);
                
                // Cria certificado com chave privada
                using var rsa = RSA.Create();
                rsa.ImportPkcs8PrivateKey(keyBytes, out _);
                
                var certificateWithKey = certificate.CopyWithPrivateKey(rsa);
                return new X509Certificate2(certificateWithKey.Export(X509ContentType.Pfx, certificatePassword), certificatePassword);
            }

            _logger.LogWarning("Nenhum certificado configurado ou arquivos não encontrados");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar certificado cliente");
            throw new InvalidOperationException("Erro ao carregar certificado cliente", ex);
        }
    }

    private class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
        
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
        
        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }
    }
}