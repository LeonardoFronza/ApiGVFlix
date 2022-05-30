using API.Consultas.IConsultas;
using API.Servicos.IServicos;
using API.ViewModels.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/historicos")]
    public class HistoricoController : ControllerBase
    {
        #region Dependencias

        private readonly IConsultaDeHistorico _consultaDeHistorico;
        private readonly IServicoDeHistorico _servicoDeHistorico;

        #endregion

        public HistoricoController(IConsultaDeHistorico consultaDeHistorico, IServicoDeHistorico servicoDeHistorico)
        {
            _consultaDeHistorico = consultaDeHistorico;
            _servicoDeHistorico = servicoDeHistorico;
        }

        #region Métodos Http

        [HttpGet("{codigoHistorico}")]
        public async Task<IActionResult> GetObterPorId(int codigoHistorico)
        {
            try
            {
                var historico = await _consultaDeHistorico.ObterPorId(codigoHistorico);

                if (historico == null)
                {
                    return NotFound("Erro! O histórico não foi encontrado.");
                }

                return Ok(historico);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpGet("ListarPorUsuario/{codigoUsuario}")]
        public async Task<IActionResult> Get(int codigoUsuario, [FromQuery] string filtroTitulo)
        {
            try
            {
                var registros = await _consultaDeHistorico.ListarPorUsuario(codigoUsuario, filtroTitulo);
                return Ok(registros);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTODeHistorico dto)
        {
            try
            {
                var historico = await _servicoDeHistorico.Incluir(dto);

                if (historico == null)
                {
                    return BadRequest("Erro! Não foi possível incluir o histórico.");
                }

                return CreatedAtAction(nameof(GetObterPorId), new { historico.CodigoHistorico }, historico);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        #endregion
    }
}
