// Exemplo de teste da integração com API externa
// Este arquivo é apenas para demonstração - não deve ser incluído na build de produção

using sro.Services;
using sro.DTOs;

namespace sro.Tests;

public class ExemploTesteApiExterna
{
    // Exemplo de como testar a integração (usando um framework de testes como xUnit)
    /*
    [Fact]
    public async Task DeveEnviarDocumentosComSucesso()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();
            
        var logger = new Mock<ILogger<ApiExternaAuthService>>();
        var authService = new ApiExternaAuthService(configuration, logger.Object);
        
        var httpLogger = new Mock<ILogger<ApiExternaHttpService>>();
        var httpService = new ApiExternaHttpService(authService, configuration, httpLogger.Object);
        
        var documentos = new List<DocumentoRequestDto>
        {
            new DocumentoRequestDto
            {
                CodigoSeguradora = "12345",
                ApoliceCodigo = "APOL001",
                // ... outros campos obrigatórios
            }
        };
        
        // Act
        var resultado = await httpService.EnviarDocumentosAsync(documentos);
        
        // Assert
        Assert.True(resultado.Sucesso);
        Assert.NotNull(resultado.IdentificadorLote);
    }
    */
    
    // Exemplo de configuração para testes (appsettings.Test.json)
    /*
    {
      "ApiExterna": {
        "BaseUrl": "https://api-test-seguros-cert-insurconnect.b3.com.br",
        "RegistroEndpoint": "/registro",
        "TokenUrl": "https://api-test-seguros-cert-insurconnect.b3.com.br/oauth/token",
        "ClientId": "client_id_teste",
        "ClientSecret": "client_secret_teste",
        "CertificadoPath": "C:\\certificados\\teste\\certificado.pfx",
        "CertificadoPassword": "senha_teste",
        "TimeoutSeconds": 60
      }
    }
    */
    
    // Exemplo de uso direto da API (para debugging)
    public static async Task ExemploUsoDirecto()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();
            
        // Simulação - em produção estes logs seriam injetados via DI
        var authLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<ApiExternaAuthService>.Instance;
        var httpLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<ApiExternaHttpService>.Instance;
        
        var authService = new ApiExternaAuthService(configuration, authLogger);
        var httpService = new ApiExternaHttpService(authService, configuration, httpLogger);
        
        try
        {
            // Exemplo de documento mínimo
            var documentos = new List<DocumentoRequestDto>
            {
                new DocumentoRequestDto
                {
                    CodigoSeguradora = "12345",
                    ApoliceCodigo = "APOL001",
                    CertificadoCodigo = "CERT001",
                    NumeroSusepApolice = "1234567890123456",
                    TipoDocumentoEmitido = 1, // A = Apolice
                    NaturezaDocumentoId = 1,
                    DataEmissao = DateTime.Now,
                    DataInicio = DateTime.Now,
                    DataTermino = DateTime.Now.AddYears(1),
                    MoedaApolice = "BRL",
                    LimiteMaximoGarantiaReal = 100000.00m,
                    PercentualRetido = 0.00m,
                    PossuiBeneficiario = false,
                    PossuiBeneficiarioFinal = false,
                    PossuiIntermediario = false,
                    RetificacaoRegistro = false,
                    Intermediario = new List<IntermediarioDto>(),
                    CoberturaRiscoSeguro = new List<CoberturaRiscoSeguroDto>(),
                    PremioContribuicao = new PremioContribuicaoDto
                    {
                        ValorTotalReal = 1000.00m,
                        AdicionalFracionamento = 0.00m,
                        ValorCarregamentoTotal = 100.00m,
                        Iof = 50.00m,
                        NumeroParcelas = 12,
                        DadosContratoColetivo = new DadosContratoColetivoDto
                        {
                            TipoPlano = 1, // Individual
                            ValorSegurado = 100000.00m,
                            ValorEstipulante = 100000.00m
                        }
                    }
                }
            };
            
            var resultado = await httpService.EnviarDocumentosAsync(documentos);
            
            if (resultado.Sucesso)
            {
                Console.WriteLine($"✅ Sucesso! Lote: {resultado.IdentificadorLote}");
            }
            else
            {
                Console.WriteLine("❌ Erro no envio:");
                foreach (var erro in resultado.Erros ?? new List<DocumentoErroDto>())
                {
                    Console.WriteLine($"  - [{erro.Codigo}] {erro.Titulo}: {erro.Detalhe}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exceção: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"   Detalhe: {ex.InnerException.Message}");
            }
        }
    }
}