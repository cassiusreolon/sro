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
            try
            {
                var responseDto = await _documentoService.EnviarDocumentosRegistroAsync();
                
                if (responseDto.Sucesso)
                {
                    return Ok(new 
                    {
                        status = "sucesso",
                        mensagem = "Documentos enviados com sucesso para a B3",
                        identificadorLote = responseDto.IdentificadorLote,
                        timestamp = DateTime.Now
                    });
                }
                else if (responseDto.TemErros)
                {
                    return BadRequest(new 
                    {
                        status = "erro_validacao",
                        mensagem = "Erro de validação na B3",
                        erros = responseDto.Erros,
                        timestamp = DateTime.Now
                    });
                }
                else
                {
                    return Ok(new 
                    {
                        status = "sem_documentos",
                        mensagem = "Nenhum documento para enviar",
                        identificadorLote = responseDto.IdentificadorLote,
                        timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                {
                    status = "erro_interno",
                    mensagem = "Erro interno no processamento",
                    erro = ex.Message,
                    timestamp = DateTime.Now
                });
            }
        }
    }
}