using System.Text.Json.Serialization;

namespace sro.DTOs
{
    public class CarenciaDto
    {
        [JsonPropertyName("identificador_carencia")]
        public string IdentificadorCarencia { get; set; } = string.Empty;

        [JsonPropertyName("carencia_periodo")]
        public int CarenciaPeriodo { get; set; }

        [JsonPropertyName("carencia_periodicidade")]
        public int CarenciaPeriodicidade { get; set; }

        [JsonPropertyName("carencia_periodicidade_dias")]
        public int CarenciaPeriodicidadeDias { get; set; }

        [JsonPropertyName("carencia_descricao")]
        public string CarenciaDescricao { get; set; } = string.Empty;
    }
}