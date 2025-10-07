namespace sro.Models;

public class SeguradoParticipante
{
    public long Id { get; set; }
    public long DocumentoId { get; set; }
    public string DocumentoIdentificacao { get; set; } = string.Empty;
    public int TipoDocumento { get; set; }
    public string? Nome { get; set; }
    public string? NomeSocial { get; set; }
    public DateTime? DataNascimento { get; set; }
    public int Sexo { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Uf { get; set; }
    public string? Pais { get; set; }

    // Navigation
    public Documento Documento { get; set; } = null!;
}