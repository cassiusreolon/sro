using sro.Configuration;
using sro.Repositories;
using sro.DTOs;

namespace sro.Services;
public class DocumentoService
{
    private readonly SROContext _context;

    public DocumentoService(SROContext context)
    {
        _context = context;
    }

    public async Task<DocumentoRequestDto> EnviarDocumentosRegistroAsync()
    {
        //string sroApiUrl = "https://Caminho_backend/api/documentos/v3/registro"; // URL da API externa

        var documentos = await new DocumentoRepository(_context).ObterDocumentosParaEnvioAsync();
        if (documentos.Count == 0)
            return null;

        // Para cada documento a enviar
        /* foreach (var documento in documentos)
        {
            // Carrega os demais dados relacionados ao documento em questao
            _documentoRequestDto = new DocumentoRequestDto
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
                SeguradoParticipante = new List<SeguradoParticipanteDto>()
            };

            // SeguradoParticipante = ... // Carregar lista de segurados/participantes relacionados
            foreach (var segurado in documento.Segurados)
            {
                _documentoRequestDto.SeguradoParticipante.Add(new SeguradoParticipanteDto
                {
                    Documento = segurado.DocumentoIdentificacao,
                    TipoDocumento = segurado.TipoDocumento,
                    Nome = segurado.Nome,
                    NomeSocial = segurado.NomeSocial,
                    DataNascimento = segurado.DataNascimento,
                    Sexo = segurado.Sexo,
                    CodigoPostal = segurado.CodigoPostal,
                    UF = segurado.UF
                });
            }

            // Beneficiarios = ... // Carregar lista de beneficiarios relacionados
            // Intermediarios = ... // Carregar lista de intermediarios relacionados


            // Enviar via HttpClient para a API externa

            //atualiza o campo data_envio do documento no banco de dados

            return _documentoRequestDto;
        }
        */
        return null;
    }
}