using System;
using System.Text.Json.Serialization;

namespace sro.DTOs;

public class CoberturaRiscoSeguroDto
{
    [JsonPropertyName("cobertura_interna_seguradora")]
    public string CoberturaInternaSeguradora { get; set; } = string.Empty;

    [JsonPropertyName("grupo_ramo")]
    public string GrupoRamo { get; set; } = string.Empty;

    [JsonPropertyName("cod_cobertura_risco")]
    public string CodCoberturaRisco { get; set; } = string.Empty;

    [JsonPropertyName("outras_descricao")]
    public string? OutrasDescricao { get; set; }

    [JsonPropertyName("numero_processo")]
    public string NumeroProcesso { get; set; } = string.Empty;

    [JsonPropertyName("limite_maximo_indenizacao_real")]
    public decimal LimiteMaximoIndenizacaoReal { get; set; }

    [JsonPropertyName("limite_maximo_indenizacao_moeda_original")]
    public decimal? LimiteMaximoIndenizacaoMoedaOriginal { get; set; }

    [JsonPropertyName("data_inicio_cobertura"), JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime DataInicioCobertura { get; set; }

    [JsonPropertyName("data_termino_cobertura"), JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime DataTerminoCobertura { get; set; }

    [JsonPropertyName("indice_atualizacao")]
    public int IndiceAtualizacao { get; set; }

    [JsonPropertyName("descricao_indice_atualizacao")]
    public string? DescricaoIndiceAtualizacao { get; set; }

    [JsonPropertyName("cobertura_caracteristica")]
    public int CoberturaCaracteristica { get; set; }

    [JsonPropertyName("tipo_risco")]
    public int TipoRisco { get; set; }

    [JsonPropertyName("cobertura_tipo")]
    public int CoberturaTipo { get; set; }

    [JsonPropertyName("periodicidade_premio")]
    public int PeriodicidadePremio { get; set; }

    [JsonPropertyName("descricao_periodicidade")]
    public string? DescricaoPeriodicidade { get; set; }

    [JsonPropertyName("valor_premio_real")]
    public decimal ValorPremioReal { get; set; }

    [JsonPropertyName("valor_premio_moeda_original")]
    public decimal? ValorPremioMoedaOriginal { get; set; }

    [JsonPropertyName("iof")]
    public decimal Iof { get; set; }

    [JsonPropertyName("base_indenizacao")]
    public int? BaseIndenizacao { get; set; }

    [JsonPropertyName("possui_carencia")]
    public bool PossuiCarencia { get; set; }

    [JsonPropertyName("possui_franquia")]
    public bool PossuiFranquia { get; set; }

    [JsonPropertyName("possui_pos")]
    public bool PossuiPos { get; set; }

    [JsonPropertyName("pessoas")]
    public PessoasDto? Pessoas { get; set; }

    [JsonPropertyName("prestamista")]
    public PrestamistaDto? Prestamista { get; set; }
}