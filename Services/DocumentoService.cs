using sro.Configuration;
using sro.Repositories;
using sro.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace sro.Services;
public class DocumentoService
{
    private readonly SROContext _context;

    public DocumentoService(SROContext context)
    {
        _context = context;
    }

    public async Task<List<DocumentoRequestDto>> EnviarDocumentosRegistroAsync()
    {
        //string sroApiUrl = "https://Caminho_backend/api/documentos/v3/registro"; // URL da API externa

        var documentos = await new DocumentoRepository(_context).ObterDocumentosParaEnvioAsync();
        if (documentos.Count == 0)
            return new List<DocumentoRequestDto>();

        var documentosProcessados = new List<DocumentoRequestDto>();

        // Para cada documento a enviar
        foreach (var documento in documentos)
        {
            // Carrega os demais dados relacionados ao documento em questao
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
                LimiteMaximoGarantiaMoedaOriginal = documento.LimiteMaximoGarantiaMoedaOriginal,
                PercentualRetido = documento.PercentualRetido,
                PossuiBeneficiario = documento.PossuiBeneficiario,
                PossuiBeneficiarioFinal = documento.PossuiBeneficiarioFinal,
                PossuiIntermediario = documento.PossuiIntermediario,
                RetificacaoRegistro = documento.RetificacaoRegistro,
                Intermediario = new List<IntermediarioDto>()
            };

            foreach (var intermediario in documento.Intermediarios)
            {
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
                    UF = intermediario.UF,
                    Pais = intermediario.Pais,
                    ValorComissaoReal = intermediario.ValorComissaoReal
                };
                documentoRequestDto.Intermediario.Add(intermediarioDto);
            }
            
           

            // SeguradoParticipante = ... // Carregar lista de segurados/participantes relacionados
            // Beneficiarios = ... // Carregar lista de beneficiarios relacionados
            // Intermediarios = ... // Carregar lista de intermediarios relacionados


            // Enviar via HttpClient para a API externa

            //atualiza o campo data_envio do documento no banco de dados
            
            // Adiciona o documento processado à lista
            documentosProcessados.Add(documentoRequestDto);
        }
        
        return documentosProcessados;
    }
}