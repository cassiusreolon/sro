using System.Text.Json.Serialization;

namespace sro.DTOs
{
    public class BeneficiarioDto
    {
        [JsonPropertyName("documento")]
        public string Documento { get; set; } = string.Empty;

        [JsonPropertyName("tipo_documento")]
        public int TipoDocumento { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("nome_social")]
        public string? NomeSocial { get; set; }

        [JsonPropertyName("codigo_postal")]
        public string CodigoPostal { get; set; } = string.Empty;

        [JsonPropertyName("uf")]
        public string UF { get; set; } = string.Empty;

        [JsonPropertyName("pais")]
        public string Pais { get; set; } = "BRA";
    }
}