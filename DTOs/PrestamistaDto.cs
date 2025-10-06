using System.Text.Json.Serialization;

namespace sro.DTOs
{
    public class PrestamistaDto
    {
        [JsonPropertyName("modalidade_capital")]
        public int ModalidadeCapital { get; set; }

        [JsonPropertyName("prestamista_tipo")]
        public int PrestamistaTipo { get; set; }

        [JsonPropertyName("tipo_obrigacao")]
        public int TipoObrigacao { get; set; }

        [JsonPropertyName("descricao_obrigacao")]
        public string? DescricaoObrigacao { get; set; }
    }
}