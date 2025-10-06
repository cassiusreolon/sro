namespace sro.Models;

public class Carencia
{
    public long Id { get; set; }
    public long CoberturaRiscoSeguroId { get; set; }
    public string IdentificadorCarencia { get; set; } = string.Empty;
    public short CarenciaPeriodo { get; set; }
    public short CarenciaPeriodicidade { get; set; }
    public short CarenciaPeriodicidadeDias { get; set; }
    public string CarenciaDescricao { get; set; } = string.Empty;

    // Navigation
    public CoberturaRiscoSeguro CoberturaRiscoSeguro { get; set; } = null!;
}