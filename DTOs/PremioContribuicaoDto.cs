using System.Text.Json.Serialization;

namespace sro.DTOs;

public class PremioContribuicaoDto
{
    [JsonPropertyName("valor_total_real")]
    public decimal ValorTotalReal { get; set; }

    [JsonPropertyName("adicional_fracionamento")]
    public decimal AdicionalFracionamento { get; set; }

    [JsonPropertyName("valor_carregamento_total")]
    public decimal ValorCarregamentoTotal { get; set; }

    [JsonPropertyName("iof")]
    public decimal Iof { get; set; }

    [JsonPropertyName("numero_parcelas")]
    public int NumeroParcelas { get; set; }

    [JsonPropertyName("dados_contrato_coletivo")]
    public DadosContratoColetivoDto? DadosContratoColetivo { get; set; }

    public PremioContribuicaoDto()
    { 
        DadosContratoColetivo = new DadosContratoColetivoDto();
    }
}