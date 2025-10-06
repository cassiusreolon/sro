using System.Text.Json.Serialization;

namespace sro.DTOs
{
    public class DadosContratoColetivoDto
    {
        [JsonPropertyName("tipo_plano")]
        public int TipoPlano { get; set; }

        [JsonPropertyName("valor_segurado")]
        public decimal ValorSegurado { get; set; }

        [JsonPropertyName("valor_estipulante")]
        public decimal ValorEstipulante { get; set; }
    }
}