using System.Text;
using System.Text.Json;
using sro.DTOs;

namespace sro.Services;

public interface IApiExternaHttpService
{
    Task<DocumentoResponseDto> EnviarDocumentosAsync(List<DocumentoRequestDto> documentos);
}

public class ApiExternaHttpService : IApiExternaHttpService
{
    private readonly IApiExternaAuthService _authService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiExternaHttpService> _logger;

    public ApiExternaHttpService(
        IApiExternaAuthService authService, 
        IConfiguration configuration,
        ILogger<ApiExternaHttpService> logger)
    {
        _authService = authService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<DocumentoResponseDto> EnviarDocumentosAsync(List<DocumentoRequestDto> documentos)
    {
        var baseUrl = _configuration["ApiExterna:BaseUrl"];
        var endpoint = _configuration["ApiExterna:RegistroEndpoint"];
        
        if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(endpoint))
        {
            throw new InvalidOperationException("Configurações da API externa não encontradas");
        }

        var url = $"{baseUrl.TrimEnd('/')}{endpoint}";

        try
        {
            using var httpClient = await _authService.GetAuthenticatedHttpClientAsync();
            
            // Preparar o payload
            var payload = new
            {
                data = new
                {
                    documento = documentos
                }
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            _logger.LogInformation("Enviando {Count} documentos para API externa: {Url}", documentos.Count, url);
            _logger.LogDebug("Payload: {Payload}", json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Resposta da API externa: {StatusCode}", response.StatusCode);
            _logger.LogDebug("Conteúdo da resposta: {ResponseContent}", responseContent);

            if (response.IsSuccessStatusCode)
            {
                var responseDto = JsonSerializer.Deserialize<DocumentoResponseDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (responseDto != null)
                {
                    _logger.LogInformation("Documentos enviados com sucesso. Identificador: {IdentificadorLote}", 
                        responseDto.Data?.IdentificadorLote);
                    return responseDto;
                }
                else
                {
                    _logger.LogWarning("Resposta de sucesso mas não foi possível deserializar o DTO");
                    return new DocumentoResponseDto
                    {
                        Data = new DocumentoResponseDataDto
                        {
                            IdentificadorLote = Guid.NewGuid().ToString()
                        }
                    };
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
            {
                // Erro de validação - tentar deserializar a resposta de erro
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<DocumentoResponseDto>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    if (errorResponse != null)
                    {
                        var erros = errorResponse.Erros?.Select(e => e.ToString()) ?? new List<string>();
                        _logger.LogWarning("Erro de validação na API externa: {Erros}", 
                            string.Join(", ", erros));
                        return errorResponse;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Erro ao deserializar resposta de erro da API externa");
                }

                // Se não conseguir deserializar, retorna erro genérico
                return new DocumentoResponseDto
                {
                    Erros = new List<DocumentoErroDto> 
                    { 
                        new DocumentoErroDto 
                        { 
                            Codigo = response.StatusCode.ToString(),
                            Titulo = "Erro de validação",
                            Detalhe = $"Erro de validação na API externa: {response.StatusCode}"
                        }
                    }
                };
            }
            else
            {
                var errorMessage = $"Erro HTTP na API externa: {response.StatusCode}";
                _logger.LogError(errorMessage + " - Conteúdo: {ResponseContent}", responseContent);
                
                throw new HttpRequestException($"{errorMessage} - {responseContent}");
            }
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            var errorMessage = "Timeout ao enviar documentos para API externa";
            _logger.LogError(ex, errorMessage);
            throw new TimeoutException(errorMessage, ex);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de rede ao enviar documentos para API externa");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao enviar documentos para API externa");
            throw new Exception("Erro inesperado ao enviar documentos para API externa", ex);
        }
    }
}