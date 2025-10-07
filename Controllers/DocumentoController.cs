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
            var documentosRequestDto = await _documentoService.EnviarDocumentosRegistroAsync();
            if (documentosRequestDto == null || !documentosRequestDto.Any())
                return Ok(new { mensagem = "Nenhum documento para enviar!" });

            return Ok(new {
                data = new {
                    documento = documentosRequestDto 
                }
            });
        }
    }
}