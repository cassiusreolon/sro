using sro.Configuration;
using sro.Repositories;
using sro.DTOs;

namespace sro.Services;
public class DocumentoService
{
    private readonly SROContext _context;
    private readonly IApiExternaHttpService _apiExternaHttpService;

    public DocumentoService(SROContext context, IApiExternaHttpService apiExternaHttpService)
    {
        _context = context;
        _apiExternaHttpService = apiExternaHttpService;
    }

    public async Task<DocumentoResponseDto> EnviarDocumentosRegistroAsync()
    {
        var repository = new DocumentoRepository(_context);
        
        // Verifica se há processamento em andamento antes de iniciar
        if (await repository.TemProcessamentoEmAndamentoAsync())
        {
            throw new InvalidOperationException("Existe um processamento em andamento. Aguarde a conclusão antes de iniciar um novo envio.");
        }

        // Inicia o log de processamento
        long logId = await repository.IniciarLogProcessamentoAsync("EnviarDocumentosRegistro", "sistema");

        try
        {
            var documentos = await repository.ObterDocumentosParaEnvioAsync();
            if (documentos.Count == 0)
            {
                // Finaliza o log mesmo se não houver documentos
                await repository.FinalizarLogProcessamentoAsync(logId);
                return new DocumentoResponseDto
                {
                    Data = new DocumentoResponseDataDto
                    {
                        IdentificadorLote = "Nenhum documento para envio"
                    }
                };
            }

            var loteDocumentos = new List<DocumentoRequestDto>();

            // Para cada documento a enviar
            foreach (var documento in documentos)
            {
                // bloco "documento"
                try
                {
                    var documentoRequestDto = new DocumentoRequestDto
                    {
                        CodigoSeguradora = documento.CodigoSeguradora,
                        ApoliceCodigo = documento.ApoliceCodigo,
                        CertificadoCodigo = documento.CertificadoCodigo,
                        NumeroSusepApolice = documento.NumeroSusepApolice,
                        TipoDocumentoEmitido = documento.TipoDocumentoEmitido,
                        NaturezaDocumentoId = documento.NaturezaDocumento,
                        DataEmissao = documento.DataEmissao,
                        DataInicio = documento.DataInicio,
                        DataTermino = documento.DataTermino,
                        MoedaApolice = documento.MoedaApolice,
                        LimiteMaximoGarantiaReal = documento.LimiteMaximoGarantiaReal,
                        PercentualRetido = documento.PercentualRetido,
                        PossuiBeneficiario = documento.PossuiBeneficiario,
                        PossuiBeneficiarioFinal = documento.PossuiBeneficiarioFinal,
                        PossuiIntermediario = documento.PossuiIntermediario,
                        RetificacaoRegistro = documento.RetificacaoRegistro,
                        Intermediario = new List<IntermediarioDto>(),
                        //Bloco "premio_contribuicao"
                        PremioContribuicao = new PremioContribuicaoDto
                        {
                            ValorTotalReal = documento.ValorTotalReal,
                            AdicionalFracionamento = documento.AdicionalFracionamento,
                            ValorCarregamentoTotal = documento.ValorCarregamentoTotal,
                            Iof = documento.Iof,
                            NumeroParcelas = documento.NumeroParcelas,
                            // bloco "dados_contrato_coletivo"
                            DadosContratoColetivo = new DadosContratoColetivoDto
                            {
                                TipoPlano = documento.TipoPlano,
                                ValorSegurado = documento.ValorSegurado,
                                ValorEstipulante = documento.ValorEstipulante
                            },
                        }
                    };

                    foreach (var intermediario in documento.Intermediarios)
                    {
                        //Bloco "intermediario"
                        var intermediarioDto = new IntermediarioDto
                        {
                            DocumentoIdentificacao = intermediario.DocumentoIdentificacao,
                            TipoComissao = intermediario.TipoComissao,
                            TipoIntermediario = intermediario.TipoIntermediario,
                            DescricaoIntermediario = intermediario.DescricaoIntermediario,
                            Codigo = intermediario.Codigo,
                            TipoDocumento = intermediario.TipoDocumento,
                            Nome = intermediario.Nome,
                            CodigoPostal = intermediario.CodigoPostal,
                            Uf = intermediario.Uf,
                            Pais = intermediario.Pais,
                            ValorComissaoReal = intermediario.ValorComissaoReal
                        };
                        documentoRequestDto.Intermediario.Add(intermediarioDto);
                    }

                    foreach (var cobertura in documento.CoberturasRiscoSeguro)
                    {
                        //Bloco "cobertura_risco_seguro"
                        var coberturaDto = new CoberturaRiscoSeguroDto
                        {
                            CoberturaInternaSeguradora = cobertura.CoberturaInternaSeguradora,
                            GrupoRamo = cobertura.GrupoRamo,
                            CodCoberturaRisco = cobertura.CodCoberturaRisco,
                            OutrasDescricao = cobertura.OutrasDescricao,
                            NumeroProcesso = cobertura.NumeroProcesso,
                            LimiteMaximoIndenizacaoReal = cobertura.LimiteMaximoIndenizacaoReal,
                            DataInicioCobertura = cobertura.DataInicioCobertura,
                            DataTerminoCobertura = cobertura.DataTerminoCobertura,
                            IndiceAtualizacao = cobertura.IndiceAtualizacao,
                            DescricaoIndiceAtualizacao = cobertura.DescricaoIndiceAtualizacao,
                            CoberturaCaracteristica = cobertura.CoberturaCaracteristica,
                            TipoRisco = cobertura.TipoRisco,
                            CoberturaTipo = cobertura.CoberturaTipo,
                            PeriodicidadePremio = cobertura.PeriodicidadePremio,
                            DescricaoPeriodicidade = cobertura.DescricaoPeriodicidade,
                            ValorPremioReal = cobertura.ValorPremioReal,
                            Iof = cobertura.Iof,
                            BaseIndenizacao = cobertura.BaseIndenizacao,
                            PossuiCarencia = cobertura.PossuiCarencia,
                            PossuiFranquia = cobertura.PossuiFranquia,
                            PossuiPos = cobertura.PossuiPos,
                            //Bloco "pessoas"
                            Pessoas = new PessoasDto
                            {
                                InclusaoDependentes = cobertura.InclusaoDependentes,
                                AbrangenciaViagem = cobertura.AbrangenciaViagem,
                                RegimeFinanceiro = cobertura.RegimeFinanceiro,
                                FormaTarifacao = cobertura.FormaTarifacao,
                                FormaTarifacaoDescricao = cobertura.FormaTarifacaoDescricao
                            },
                            //Bloco "prestamista"
                            Prestamista = new PrestamistaDto
                            {
                                ModalidadeCapital = cobertura.ModalidadeCapital,
                                PrestamistaTipo = cobertura.PrestamistaTipo,
                                TipoObrigacao = cobertura.TipoObrigacao,
                                DescricaoObrigacao = cobertura.DescricaoObrigacao
                            }
                        };
                        documentoRequestDto.CoberturaRiscoSeguro.Add(coberturaDto);
                    }
                    
                    // SeguradoParticipante = ... // Carregar lista de segurados/participantes relacionados (somente na previdencia se necessario)
                    // Beneficiarios = ... // Carregar lista de beneficiarios relacionados (somente no sinistro se necessário)

                    try
                    {
                        // Adiciona o documento à lista para processamento
                        loteDocumentos.Add(documentoRequestDto);
                        
                        // DataEnvio será definida apenas após sucesso na API externa
                    }
                    catch (Exception ex)
                    {
                        // Log do erro ao processar dados do documento
                        //Console.WriteLine($"Erro ao processar dados do documento ID {documento.Id}: {ex.Message}");
                        throw new Exception($"Erro ao processar dados do documento ID {documento.Id}: {ex.Message}", ex);
                    }
                }
                catch (Exception ex)
                {
                    // Log do erro ao criar o DTO do documento
                    //Console.WriteLine($"Erro ao criar DTO do documento ID {documento.Id}: {ex.Message}");
                    throw new Exception($"Erro ao criar DTO do documento ID {documento.Id}: {ex.Message}", ex);
                }
            }

            // Após processar todos os documentos, tenta enviar para a API externa
            if (loteDocumentos.Count > 0)
            {
                try
                {
                    var responseDto = await _apiExternaHttpService.EnviarDocumentosAsync(loteDocumentos);

                    if (responseDto.Sucesso)
                    {
                        Console.WriteLine($"Lote enviado com sucesso! Identificador: {responseDto.IdentificadorLote}");

                        // APENAS EM CASO DE SUCESSO: Atualiza DataEnvio e IdentificadorLote
                        var dataEnvio = DateTime.Now;
                        var identificadorLote = responseDto.IdentificadorLote;
                        
                        foreach (var documento in documentos)
                        {
                            documento.DataEnvio = dataEnvio;
                            documento.IdentificadorLote = identificadorLote;
                        }
                        await _context.SaveChangesAsync();
                        
                        Console.WriteLine($"DataEnvio e IdentificadorLote atualizados para {documentos.Count} documentos");
                        Console.WriteLine($"Identificador do lote: {identificadorLote}");
                    }
                    else if (responseDto.TemErros)
                    {
                        Console.WriteLine("Erro no envio do lote:");
                        foreach (var erro in responseDto.Erros!)
                        {
                            Console.WriteLine($"  - {erro}");
                        }
                        Console.WriteLine("DataEnvio NÃO foi atualizada devido aos erros");
                        // IMPORTANTE: Não chama SaveChanges() aqui - mantém DataEnvio = null
                    }
                    else
                    {
                        Console.WriteLine("Resposta inesperada da API externa - DataEnvio não foi atualizada");
                        // IMPORTANTE: Não chama SaveChanges() aqui - mantém DataEnvio = null
                    }

                    // Retorna a resposta da API externa
                    return responseDto;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Erro ao enviar lote para API externa: {ex.Message}");
                    throw new Exception($"Erro ao enviar lote para API externa: {ex.Message}", ex);
                    // Não atualiza DataEnvio em caso de exceção
                }
            }
            else
            {
                await _context.SaveChangesAsync();
                
                // Retorna resposta indicando que não havia documentos
                return new DocumentoResponseDto
                {
                    Data = new DocumentoResponseDataDto
                    {
                        IdentificadorLote = "Nenhum documento processado"
                    }
                };
            }
        }
        finally
        {
            // Sempre finaliza o log, independente de sucesso ou erro
            await repository.FinalizarLogProcessamentoAsync(logId);
        }
    }


}