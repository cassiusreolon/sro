using sro.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace sro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TesteApiExternaController : ControllerBase
{
    private readonly IApiExternaAuthService _authService;
    private readonly ILogger<TesteApiExternaController> _logger;
    private readonly IConfiguration _configuration;

    public TesteApiExternaController(
        IApiExternaAuthService authService, 
        ILogger<TesteApiExternaController> logger,
        IConfiguration configuration)
    {
        _authService = authService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("testar-certificado")]
    public IActionResult TestarCertificado()
    {
        try
        {
            var resultado = new
            {
                status = "iniciando_teste",
                timestamp = DateTime.Now,
                configuracao = new
                {
                    baseUrl = _configuration["ApiExterna:BaseUrl"],
                    tokenUrl = _configuration["ApiExterna:TokenUrl"],
                    clientId = _configuration["ApiExterna:ClientId"],
                    certificadoPath = _configuration["ApiExterna:CertificadoPath"],
                    certificadoKeyPath = _configuration["ApiExterna:CertificadoKeyPath"],
                    certificadoCerPath = _configuration["ApiExterna:CertificadoCerPath"],
                    certificadoPathExists = !string.IsNullOrEmpty(_configuration["ApiExterna:CertificadoPath"]) ? 
                        System.IO.File.Exists(_configuration["ApiExterna:CertificadoPath"]) : false,
                    keyPathExists = !string.IsNullOrEmpty(_configuration["ApiExterna:CertificadoKeyPath"]) ? 
                        System.IO.File.Exists(_configuration["ApiExterna:CertificadoKeyPath"]) : false,
                    cerPathExists = !string.IsNullOrEmpty(_configuration["ApiExterna:CertificadoCerPath"]) ? 
                        System.IO.File.Exists(_configuration["ApiExterna:CertificadoCerPath"]) : false
                }
            };

            _logger.LogInformation("Testando configuração de certificados: {@Configuracao}", resultado.configuracao);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao testar certificado");
            return BadRequest(new { erro = ex.Message, detalhes = ex.ToString() });
        }
    }

    [HttpGet("testar-token")]
    public async Task<IActionResult> TestarToken()
    {
        try
        {
            _logger.LogInformation("Iniciando teste de obtenção de token");
            
            var token = await _authService.GetAccessTokenAsync();
            
            var resultado = new
            {
                status = "sucesso",
                timestamp = DateTime.Now,
                tokenObtido = !string.IsNullOrEmpty(token),
                tokenLength = token?.Length ?? 0,
                tokenPreview = token?.Length > 20 ? $"{token[..20]}..." : token
            };

            _logger.LogInformation("Token obtido com sucesso: {@Resultado}", resultado);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter token");
            
            var resultado = new
            {
                status = "erro",
                timestamp = DateTime.Now,
                erro = ex.Message,
                tipo = ex.GetType().Name,
                detalhes = ex.InnerException?.Message
            };

            return BadRequest(resultado);
        }
    }

    [HttpGet("testar-conectividade-com-certificado")]
    public async Task<IActionResult> TestarConectividadeComCertificado()
    {
        try
        {
            var baseUrl = _configuration["ApiExterna:BaseUrl"];
            var tokenUrl = _configuration["ApiExterna:TokenUrl"];
            var testes = new List<object>();

            // Teste 2: Conectividade com URL do token COM certificado
            try
            {
                using var httpClientComCertificado = await _authService.GetAuthenticatedHttpClientAsync();
                httpClientComCertificado.Timeout = TimeSpan.FromSeconds(15);

                var response2 = await httpClientComCertificado.GetAsync(tokenUrl);
                testes.Add(new
                {
                    teste = "conectividade_token_url_com_certificado",
                    url = tokenUrl,
                    status = response2.StatusCode.ToString(),
                    sucesso = response2.StatusCode != System.Net.HttpStatusCode.Forbidden,
                    headers = response2.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value))
                });
            }
            catch (Exception ex)
            {
                testes.Add(new
                {
                    teste = "conectividade_token_url_com_certificado",
                    url = tokenUrl,
                    erro = ex.Message,
                    tipo = ex.GetType().Name,
                    sucesso = false
                });
            }

            // Teste 1: Conectividade COM certificado (que é o correto para B3)
            try
            {
                using var httpClientComCertificado = await _authService.GetAuthenticatedHttpClientAsync();
                httpClientComCertificado.Timeout = TimeSpan.FromSeconds(15);

                var response1 = await httpClientComCertificado.GetAsync(baseUrl);
                testes.Add(new
                {
                    teste = "conectividade_base_url_com_certificado",
                    url = baseUrl,
                    status = response1.StatusCode.ToString(),
                    sucesso = response1.StatusCode != System.Net.HttpStatusCode.Forbidden,
                    headers = response1.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value))
                });
            }
            catch (Exception ex)
            {
                testes.Add(new
                {
                    teste = "conectividade_base_url_com_certificado",
                    url = baseUrl,
                    erro = ex.Message,
                    tipo = ex.GetType().Name,
                    sucesso = false
                });
            }

            return Ok(new
            {
                timestamp = DateTime.Now,
                info = "Testes realizados COM certificado mTLS (necessário para B3)",
                testes = testes
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao testar conectividade com certificado");
            return BadRequest(new { erro = ex.Message, detalhes = ex.ToString() });
        }
    }

    [HttpGet("validar-certificado")]
    public IActionResult ValidarCertificado()
    {
        try
        {
            var certificadoPath = _configuration["ApiExterna:CertificadoPath"];
            var certificadoPassword = _configuration["ApiExterna:CertificadoPassword"];

            if (string.IsNullOrEmpty(certificadoPath))
            {
                return BadRequest(new { erro = "Caminho do certificado não configurado" });
            }

            if (!System.IO.File.Exists(certificadoPath))
            {
                return BadRequest(new { erro = $"Arquivo de certificado não encontrado: {certificadoPath}" });
            }

            try
            {
                var certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificadoPath, certificadoPassword);
                
                var resultado = new
                {
                    status = "certificado_valido",
                    certificado = new
                    {
                        subject = certificate.Subject,
                        issuer = certificate.Issuer,
                        validFrom = certificate.NotBefore,
                        validTo = certificate.NotAfter,
                        isValid = DateTime.Now >= certificate.NotBefore && DateTime.Now <= certificate.NotAfter,
                        hasPrivateKey = certificate.HasPrivateKey,
                        thumbprint = certificate.Thumbprint,
                        serialNumber = certificate.SerialNumber
                    }
                };

                _logger.LogInformation("Certificado validado com sucesso: {@Certificado}", resultado.certificado);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar certificado");
                return BadRequest(new 
                { 
                    erro = "Erro ao carregar certificado", 
                    detalhes = ex.Message,
                    possiveisCausas = new[]
                    {
                        "Senha do certificado incorreta",
                        "Arquivo corrompido",
                        "Formato de arquivo não suportado",
                        "Certificado expirado"
                    }
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar certificado");
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("testar-token-detalhado")]
    public async Task<IActionResult> TestarTokenDetalhado()
    {
        try
        {
            var tokenUrl = _configuration["ApiExterna:TokenUrl"];
            var clientId = _configuration["ApiExterna:ClientId"];
            var clientSecret = _configuration["ApiExterna:ClientSecret"];

            _logger.LogInformation("Iniciando teste detalhado de token");

            // Criar HttpClient com certificado manualmente para debug
            var handler = new HttpClientHandler();
            
            // Carregar certificado
            var certificadoPath = _configuration["ApiExterna:CertificadoPath"];
            var certificadoPassword = _configuration["ApiExterna:CertificadoPassword"];
            
            if (!string.IsNullOrEmpty(certificadoPath) && System.IO.File.Exists(certificadoPath))
            {
                var certificate = new X509Certificate2(certificadoPath, certificadoPassword);
                handler.ClientCertificates.Add(certificate);
                _logger.LogInformation("Certificado adicionado ao handler");
            }

            using var httpClient = new HttpClient(handler);
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            // Preparar dados do POST
            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId ?? ""),
                new KeyValuePair<string, string>("client_secret", clientSecret ?? "")
            });

            _logger.LogInformation("Enviando requisição POST para: {TokenUrl}", tokenUrl);
            _logger.LogInformation("Client ID: {ClientId}", clientId);

            var response = await httpClient.PostAsync(tokenUrl, tokenRequest);
            var responseContent = await response.Content.ReadAsStringAsync();

            var resultado = new
            {
                status = response.IsSuccessStatusCode ? "sucesso" : "erro",
                timestamp = DateTime.Now,
                request = new
                {
                    method = "POST",
                    url = tokenUrl,
                    headers = httpClient.DefaultRequestHeaders.ToDictionary(h => h.Key, h => string.Join(", ", h.Value)),
                    body = "grant_type=client_credentials&client_id=" + clientId + "&client_secret=[OCULTO]"
                },
                response = new
                {
                    statusCode = response.StatusCode.ToString(),
                    statusCodeNumber = (int)response.StatusCode,
                    headers = response.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value)),
                    content = responseContent,
                    contentLength = responseContent.Length
                }
            };

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Token obtido com sucesso via teste detalhado");
                return Ok(resultado);
            }
            else
            {
                _logger.LogWarning("Erro ao obter token via teste detalhado: {StatusCode}", response.StatusCode);
                return BadRequest(resultado);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no teste detalhado de token");
            return BadRequest(new 
            { 
                status = "erro_exception",
                timestamp = DateTime.Now,
                erro = ex.Message,
                tipo = ex.GetType().Name,
                detalhes = ex.ToString()
            });
        }
    }
}