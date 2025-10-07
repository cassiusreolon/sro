namespace sro.Models;

public class Intermediario
{
    public long Id { get; set; }
    public long DocumentoId { get; set; }
    public string DocumentoIdentificacao { get; set; } = string.Empty;
    public int TipoComissao { get; set; }
    public int TipoIntermediario { get; set; }
    public string? DescricaoIntermediario { get; set; }
    public string? Codigo { get; set; }
    public int TipoDocumento { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public decimal ValorComissaoReal { get; set; } = 0;

    // Navigation
    public Documento Documento { get; set; } = null!;
}