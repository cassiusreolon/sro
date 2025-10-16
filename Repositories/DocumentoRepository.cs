using Microsoft.EntityFrameworkCore;
using sro.Configuration;
using sro.Models;

namespace sro.Repositories
{
    public class DocumentoRepository
    {
        // Adicione métodos e propriedades conforme necessário
        private readonly SROContext _context;

        public DocumentoRepository(SROContext context)
        {
            // Construtor padrão
            _context = context;
        }

        public async Task<List<Documento>> ObterDocumentosParaEnvioAsync()
        {
            return await _context.Documento
                .Include(d => d.Intermediarios) // Carrega os Intermediarios relacionados
                .Include(c => c.CoberturasRiscoSeguro) // Carrega as coberturas relacionadas
                .Where(d => d.DataEnvio == null &&
                            d.LiberadoEnvio == true &&
                            d.AlteracaoSequencial == null) // Envia apenas documentos
                .OrderBy(d => d.Id)
                .ToListAsync();
        }

        public async Task<List<Documento>> ObterDocumentosAlteracaoParaEnvioAsync()
        {
            return await _context.Documento
                .Include(d => d.Intermediarios) // Carrega os Intermediarios relacionados
                .Include(c => c.CoberturasRiscoSeguro) // Carrega as coberturas relacionadas
                .Where(d => d.DataEnvio == null &&
                            d.LiberadoEnvio == true &&
                            d.AlteracaoSequencial != null) // Envia apenas alterações dos documentos
                .OrderBy(d => d.Id)
                .ToListAsync();
        }
        /// Inicia um novo log de processamento
        public async Task<long> IniciarLogProcessamentoAsync(string processo, string usuario)
        {
            if (_context.LogProcessamento == null)
                throw new InvalidOperationException("LogProcessamento não está configurado no contexto.");

            var log = new LogProcessamento
            {
                Guid = Guid.NewGuid(),
                Processo = processo,
                Usuario = usuario,
                DataInicio = DateTime.Now,
                DataFim = null
            };

            _context.LogProcessamento.Add(log);
            await _context.SaveChangesAsync();
            
            return log.Id;
        }

        /// Finaliza um log de processamento
        public async Task FinalizarLogProcessamentoAsync(long logId)
        {
            if (_context.LogProcessamento == null)
                return;

            var log = await _context.LogProcessamento.FindAsync(logId);
            if (log != null)
            {
                log.DataFim = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        /// Verifica se há processamentos em andamento
        public async Task<bool> TemProcessamentoEmAndamentoAsync()
        {
            return _context.LogProcessamento != null && 
                await _context.LogProcessamento.AnyAsync(log => log.DataFim == null);
        }
    }
}