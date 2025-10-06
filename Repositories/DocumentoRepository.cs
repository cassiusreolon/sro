using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var connectionString = _context.Database.GetDbConnection().ConnectionString;

            if (_context.Documento == null)
                return new List<Documento>();

            return await _context.Documento
                //.Where(d => d.DataEnvio == null && d.LiberadoEnvio == true) // Exemplo de filtro para documentos não enviados
                .ToListAsync();
        }
    }
}