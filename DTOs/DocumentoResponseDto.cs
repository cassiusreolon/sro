using System.Text.Json.Serialization;

namespace sro.DTOs;

public class DocumentoResponseDto
{
    // Para resposta de sucesso (HTTP 200)
    [JsonPropertyName("data")]
    public DocumentoResponseDataDto? Data { get; set; }

    // Para resposta de erro (HTTP 422)
    [JsonPropertyName("erros")]
    public List<DocumentoErroDto>? Erros { get; set; }

    // Propriedades auxiliares para facilitar o uso
    [JsonIgnore]
    public bool Sucesso => Data != null && (Erros == null || !Erros.Any());

    [JsonIgnore]
    public bool TemErros => Erros != null && Erros.Any();

    [JsonIgnore]
    public string? IdentificadorLote => Data?.IdentificadorLote;
}

public class DocumentoResponseDataDto
{
    [JsonPropertyName("identificador_lote")]
    public string IdentificadorLote { get; set; } = string.Empty;
}

public class DocumentoErroDto
{
    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = string.Empty;

    [JsonPropertyName("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [JsonPropertyName("detalhe")]
    public string Detalhe { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[{Codigo}] {Titulo}: {Detalhe}";
    }
}