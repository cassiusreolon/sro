using System.Text.Json.Serialization;

namespace sro.DTOs
{
    public class IntermediarioDto
    {
        [JsonPropertyName("documento")]
        public string Documento { get; set; } = string.Empty;

        [JsonPropertyName("tipo_comissao")]
        public int TipoComissao { get; set; }

        [JsonPropertyName("tipo")]
        public int Tipo { get; set; }

        [JsonPropertyName("descricao_intermediario")]
        public string? DescricaoIntermediario { get; set; }

        [JsonPropertyName("codigo")]
        public string Codigo { get; set; } = string.Empty;

        [JsonPropertyName("tipo_documento")]
        public int TipoDocumento { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("codigo_postal")]
        public string CodigoPostal { get; set; } = string.Empty;

        [JsonPropertyName("uf")]
        public string UF { get; set; } = string.Empty;

        [JsonPropertyName("pais")]
        public string Pais { get; set; } = string.Empty;

        [JsonPropertyName("valor_comissao_real")]
        public decimal ValorComissaoReal { get; set; }

        [JsonPropertyName("valor_comissao_moeda_original")]
        public decimal ValorComissaoMoedaOriginal { get; set; }
    }
}