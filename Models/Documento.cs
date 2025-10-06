namespace sro.Models;

public class Documento
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public short? Sucursal { get; set; }
    public short? RamoSisseg { get; set; }
    public int? NumeroApolice { get; set; }
    public short? NumeroSub { get; set; }
    public long? NumeroEndossoCertificado { get; set; }
    public DateTime? DataRegistro { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public bool LiberadoEnvio { get; set; }
    public DateTime? DataEnvio { get; set; }
    public string CodigoSeguradora { get; set; } = string.Empty;
    public string ApoliceCodigo { get; set; } = string.Empty;
    public string? CertificadoCodigo { get; set; }
    public string? NumeroSusepApolice { get; set; }
    public short TipoDocumentoEmitido { get; set; }
    public short NaturezaDocumento { get; set; }
    public DateTime DataEmissao { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public string MoedaApolice { get; set; } = string.Empty;
    public decimal LimiteMaximoGarantiaReal { get; set; }
    public decimal? LimiteMaximoGarantiaMoedaOriginal { get; set; }
    public decimal? PercentualRetido { get; set; }
    public bool PossuiBeneficiario { get; set; }
    public bool PossuiBeneficiarioFinal { get; set; }
    public bool PossuiIntermediario { get; set; }
    public bool RetificacaoRegistro { get; set; }
    public decimal ValorTotalReal { get; set; }
    public decimal AdicionalFracionamento { get; set; }
    public decimal ValorCarregamentoTotal { get; set; }
    public decimal Iof { get; set; }
    public short NumeroParcelas { get; set; }
    public short? TipoPlano { get; set; }
    public decimal? ValorSegurado { get; set; }
    public decimal? ValorEstipulante { get; set; }
    public string? IdentificadorLote { get; set; }

    // Navigation properties
    public ICollection<SeguradoParticipante> SeguradosParticipantes { get; set; } = new List<SeguradoParticipante>();
    public ICollection<Beneficiario> Beneficiarios { get; set; } = new List<Beneficiario>();
    public ICollection<Intermediario> Intermediarios { get; set; } = new List<Intermediario>();
    public ICollection<CoberturaRiscoSeguro> CoberturasRiscoSeguro { get; set; } = new List<CoberturaRiscoSeguro>();
}