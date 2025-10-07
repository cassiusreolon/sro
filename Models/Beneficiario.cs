namespace sro.Models;

public class Beneficiario
{
    public long Id { get; set; }
    public long DocumentoId { get; set; }
    public string DocumentoIdentificacao { get; set; } = string.Empty;
    public int TipoDocumento { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? NomeSocial { get; set; }
    public string CodigoPostal { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public bool BeneficiarioFinal { get; set; }

    // Navigation
    public Documento Documento { get; set; } = null!;
}