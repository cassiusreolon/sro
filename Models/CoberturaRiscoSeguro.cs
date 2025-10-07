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
    public DateTime DataInicioCobertura { get; set; }
    public DateTime DataTerminoCobertura { get; set; }
    public int IndiceAtualizacao { get; set; }
    public string? DescricaoIndiceAtualizacao { get; set; }
    public int CoberturaCaracteristica { get; set; }
    public int TipoRisco { get; set; }
    public int CoberturaTipo { get; set; }
    public int PeriodicidadePremio { get; set; }
    public string? DescricaoPeriodicidade { get; set; }
    public decimal ValorPremioReal { get; set; }
    public decimal Iof { get; set; }
    public int? BaseIndenizacao { get; set; }
    public bool PossuiCarencia { get; set; }
    public bool PossuiFranquia { get; set; }
    public bool PossuiPos { get; set; }
    public int InclusaoDependentes { get; set; }
    public int? AbrangenciaViagem { get; set; }
    public int RegimeFinanceiro { get; set; }
    public int FormaTarifacao { get; set; }
    public string? FormaTarifacaoDescricao { get; set; }
    public int ModalidadeCapital { get; set; }
    public int PrestamistaTipo { get; set; }
    public int TipoObrigacao { get; set; }
    public string? DescricaoObrigacao { get; set; }

    // Navigation
    public Documento Documento { get; set; } = null!;
}