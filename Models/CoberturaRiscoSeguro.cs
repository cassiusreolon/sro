namespace sro.Models;

public class CoberturaRiscoSeguro
{
    public long Id { get; set; }
    public long DocumentoId { get; set; }
    public string CoberturaInternaSeguradora { get; set; } = string.Empty;
    public string GrupoRamo { get; set; } = string.Empty;
    public string CodCoberturaRisco { get; set; } = string.Empty;
    public string? OutrasDescricao { get; set; }
    public string NumeroProcesso { get; set; } = string.Empty;
    public decimal LimiteMaximoIndenizacaoReal { get; set; }
    public decimal? LimiteMaximoIndenizacaoMoedaOriginal { get; set; }
    public DateTime DataInicioCobertura { get; set; }
    public DateTime DataTerminoCobertura { get; set; }
    public short IndiceAtualizacao { get; set; }
    public string? DescricaoIndiceAtualizacao { get; set; }
    public short CoberturaCaracteristica { get; set; }
    public int TipoRisco { get; set; }
    public short CoberturaTipo { get; set; }
    public short PeriodicidadePremio { get; set; }
    public string? DescricaoPeriodicidade { get; set; }
    public decimal ValorPremioReal { get; set; }
    public decimal? ValorPremioMoedaOriginal { get; set; }
    public decimal Iof { get; set; }
    public int? BaseIndenizacao { get; set; }
    public bool PossuiCarencia { get; set; }
    public bool PossuiFranquia { get; set; }
    public bool PossuiPos { get; set; }
    public short InclusaoDependentes { get; set; }
    public short? AbrangenciaViagem { get; set; }
    public short RegimeFinanceiro { get; set; }
    public short FormaTarifacao { get; set; }
    public string? FormaTarifacaoDescricao { get; set; }
    public short ModalidadeCapital { get; set; }
    public short PrestamistaTipo { get; set; }
    public short TipoObrigacao { get; set; }
    public string? DescricaoObrigacao { get; set; }

    // Navigation
    public Documento Documento { get; set; } = null!;
    public ICollection<Carencia> Carencias { get; set; } = null!;
}