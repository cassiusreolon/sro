namespace sro.Models;

public class Documento
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public int? Sucursal { get; set; }
    public int? RamoSisseg { get; set; }
    public int? NumeroApolice { get; set; }
    public int? NumeroSub { get; set; }
    public long? NumeroEndosso { get; set; }
    public DateTime? DataRegistro { get; set; }
    public bool LiberadoEnvio { get; set; }
    public DateTime? DataEnvio { get; set; }
    public bool? RegistroExcluido { get; set; }
    public DateTime? DataEnvioExclusao { get; set; }
    public string CodigoSeguradora { get; set; } = string.Empty;
    public string ApoliceCodigo { get; set; } = string.Empty;
    public string? CertificadoCodigo { get; set; }
    public string? NumeroSusepApolice { get; set; }
    public int TipoDocumentoEmitido { get; set; }
    public int NaturezaDocumento { get; set; }
    public DateTime DataEmissao { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public string MoedaApolice { get; set; } = string.Empty;
    public decimal LimiteMaximoGarantiaReal { get; set; }
    public decimal? PercentualRetido { get; set; }
    public bool PossuiBeneficiario { get; set; }
    public bool PossuiBeneficiarioFinal { get; set; }
    public bool PossuiIntermediario { get; set; }
    public bool RetificacaoRegistro { get; set; }
    public decimal ValorTotalReal { get; set; }
    public decimal AdicionalFracionamento { get; set; }
    public decimal ValorCarregamentoTotal { get; set; }
    public decimal Iof { get; set; }
    public int NumeroParcelas { get; set; }
    public int TipoPlano { get; set; }
    public decimal ValorSegurado { get; set; }
    public decimal ValorEstipulante { get; set; }
    public int? AlteracaoSequencial { get; set; }
    public string? AlteracaoCodigo { get; set; }
    public int? AlteracaoTipo { get; set; }
    public string? AlteracaoDescricao { get; set; }
    public bool? EndossoAverbavel { get; set; }
    public string? IdentificadorLote { get; set; }

    // Navigation properties
    public ICollection<SeguradoParticipante> SeguradosParticipantes { get; set; } = new List<SeguradoParticipante>();
    public ICollection<Beneficiario> Beneficiarios { get; set; } = new List<Beneficiario>();
    public ICollection<Intermediario> Intermediarios { get; set; } = new List<Intermediario>();
    public ICollection<CoberturaRiscoSeguro> CoberturasRiscoSeguro { get; set; } = new List<CoberturaRiscoSeguro>();
}