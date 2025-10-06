using System.Text.Json.Serialization;

namespace sro.DTOs
{
    public class PessoasDto
    {
        [JsonPropertyName("inclusao_dependentes")]
        public int InclusaoDependentes { get; set; }

        [JsonPropertyName("abrangencia_viagem")]
        public int? AbrangenciaViagem { get; set; }

        [JsonPropertyName("regime_financeiro")]
        public int RegimeFinanceiro { get; set; }

        [JsonPropertyName("forma_tarifacao")]
        public int FormaTarifacao { get; set; }

        [JsonPropertyName("forma_tarifacao_descricao")]
        public string? FormaTarifacaoDescricao { get; set; }
    }
}