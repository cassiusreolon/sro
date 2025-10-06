using System.Text.Json.Serialization;
namespace sro.DTOs;

public class DocumentoRequestDto
{
    [JsonPropertyName("codigo_seguradora")]
    public string CodigoSeguradora { get; set; } = string.Empty;

    [JsonPropertyName("apolice_codigo")]
    public string ApoliceCodigo { get; set; } = string.Empty;

    [JsonPropertyName("certificado_codigo")]
    public string? CertificadoCodigo { get; set; }

    [JsonPropertyName("numero_susep_apolice")]
    public string? NumeroSusepApolice { get; set; }

    [JsonPropertyName("tipo_documento_emitido")]
    public int TipoDocumentoEmitido { get; set; }

    [JsonPropertyName("natureza_documento")]
    public int NaturezaDocumentoId { get; set; }

    [JsonPropertyName("data_emissao"), JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime DataEmissao { get; set; }

    [JsonPropertyName("data_inicio"), JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime DataInicio { get; set; }

    [JsonPropertyName("data_termino"), JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime DataTermino { get; set; }

    [JsonPropertyName("moeda_apolice")]
    public string MoedaApolice { get; set; } = "BRL";

    [JsonPropertyName("limite_maximo_garantia_real")]
    public decimal LimiteMaximoGarantiaReal { get; set; } = 0;

    [JsonPropertyName("limite_maximo_garantia_moeda_original")]
    public decimal? LimiteMaximoGarantiaMoedaOriginal { get; set; }

    [JsonPropertyName("percentual_retido")]
    public decimal? PercentualRetido { get; set; }

    [JsonPropertyName("possui_beneficiario")]
    public bool PossuiBeneficiario { get; set; } = false;

    [JsonPropertyName("possui_beneficiario_final")]
    public bool PossuiBeneficiarioFinal { get; set; } = false;

    [JsonPropertyName("possui_intermediario")]
    public bool PossuiIntermediario { get; set; } = false;

    [JsonPropertyName("retificacao_registro")]
    public bool RetificacaoRegistro { get; set; } = false;

    //[JsonPropertyName("segurado_participante")]
    //public List<SeguradoParticipanteDto>? SeguradoParticipante { get; set; }

    //[JsonPropertyName("beneficiario")]
    //public List<BeneficiarioDto>? Beneficiario { get; set; }

    //[JsonPropertyName("beneficiario_final")]
    //public List<BeneficiarioDto>? BeneficiarioFinal { get; set; }

    [JsonPropertyName("intermediario")]
    public List<IntermediarioDto>? Intermediario { get; set; }

    //[JsonPropertyName("cobertura_risco_seguro")]
    //public List<CoberturaRiscoSeguroDto>? CoberturaRiscoSeguro { get; set; }

    public DocumentoRequestDto()
    {
        /*SeguradoParticipante = new List<SeguradoParticipanteDto>();
        Beneficiario = new List<BeneficiarioDto>();
        BeneficiarioFinal = new List<BeneficiarioDto>();
        Intermediario = new List<IntermediarioDto>();
        CoberturaRiscoSeguro = new List<CoberturaRiscoSeguroDto>();*/
    }
}