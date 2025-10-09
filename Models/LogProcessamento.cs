namespace sro.Models
{
    public class LogProcessamento
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string Processo { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}