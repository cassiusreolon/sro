using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sro.Services;

namespace sro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentoController : ControllerBase
    {
        private readonly DocumentoService _documentoService;

        public DocumentoController(DocumentoService documentoService)
        {
            _documentoService = documentoService;
        }

        [HttpPost("enviar")]
        public async Task<IActionResult> EnviarDocumentosRegistro()
        {
            await _documentoService.EnviarDocumentosRegistroAsync();
            return Ok(new { mensagem = "Documentos enviados com sucesso (se houver documentos para envio)." });
        }
    }
}