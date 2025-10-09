using System.Text.Json.Serialization;

namespace sro.DTOs
{
    public class DocumentoAlteracaoRequestDto
    {
        [JsonPropertyName("codigo_seguradora")]
        public string CodigoSeguradora { get; set; } = string.Empty;

        [JsonPropertyName("apolice_codigo")]
        public string ApoliceCodigo { get; set; } = string.Empty;

        [JsonPropertyName("certificado_codigo")]
        public string? CertificadoCodigo { get; set; }

        [JsonPropertyName("alteracao_sequencial")]
        public int AlteracaoSequencial { get; set; }

        [JsonPropertyName("numero_susep_apolice")]
        public string? NumeroSusepApolice { get; set; }

        [JsonPropertyName("alteracao_codigo")]
        public string AlteracaoCodigo { get; set; } = string.Empty;

        [JsonPropertyName("tipo_documento_emitido")]
        public int TipoDocumentoEmitido { get; set; }

        [JsonPropertyName("natureza_documento")]
        public int NaturezaDocumento { get; set; }

        [JsonPropertyName("alteracao_descricao")]
        public string? AlteracaoDescricao { get; set; }

        [JsonPropertyName("alteracao_tipo")]
        public int AlteracaoTipo { get; set; }

        [JsonPropertyName("endosso_averbavel")]
        public bool EndossoAverbavel { get; set; } = false;

        [JsonPropertyName("data_emissao"), JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? DataEmissao { get; set; }

        [JsonPropertyName("data_inicio"), JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? DataInicio { get; set; }

        [JsonPropertyName("data_termino"), JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? DataTermino { get; set; }

        [JsonPropertyName("data_inicio_documento"), JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? DataInicioDocumento { get; set; }

        [JsonPropertyName("data_termino_documento"), JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? DataTerminoDocumento { get; set; }

        [JsonPropertyName("moeda_apolice")]
        public string MoedaApolice { get; set; } = "BRL";

        [JsonPropertyName("limite_maximo_garantia_real")]
        public decimal LimiteMaximoGarantiaReal { get; set; } = 0;

        [JsonPropertyName("percentual_retido")]
        public decimal PercentualRetido { get; set; }

        [JsonPropertyName("possui_beneficiario")]
        public bool PossuiBeneficiario { get; set; } = false;

        [JsonPropertyName("possui_beneficiario_final")]
        public bool PossuiBeneficiarioFinal { get; set; } = false;

        [JsonPropertyName("possui_intermediario")]
        public bool PossuiIntermediario { get; set; } = false;

        [JsonPropertyName("retificacao_registro")]
        public bool RetificacaoRegistro { get; set; } = false;
    }
}