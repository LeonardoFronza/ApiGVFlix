using API.Consultas.IConsultas;
using API.Enumeradores;
using API.Servicos.IServicos;
using API.ViewModels.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/atores")]
    public class AtorController : ControllerBase
    {
        #region Dependencias

        private readonly IConsultaDeAtor _consultaDeAtor;
        private readonly IConsultaDeUsuario _consultaDeUsuario;
        private readonly IServicoDeAtor _servicoDeAtor;

        #endregion

        public AtorController(
            IConsultaDeAtor consultaDeAtor,
            IConsultaDeUsuario consultaDeUsuario,
            IServicoDeAtor servicoDeAtor)
        {
            _consultaDeAtor = consultaDeAtor;
            _consultaDeUsuario = consultaDeUsuario;
            _servicoDeAtor = servicoDeAtor;
        }

        #region Métodos Http

        [HttpGet("{codigoAtor}")]
        public async Task<IActionResult> GetObterPorId(int codigoAtor)
        {
            try
            {
                var ator = await _consultaDeAtor.ObterPorId(codigoAtor);

                if (ator == null)
                {
                    return NotFound("Erro! O ator não foi encontrado.");
                }

                return Ok(ator);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListar([FromQuery] string filtro)
        {
            try
            {
                var registros = await _consultaDeAtor.ListarComFiltro(filtro);
                return Ok(registros);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTODeRequisicaoGenerica<DTODeAtor> requisicao)
        {
            try
            {
                var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                {
                    return BadRequest("Apenas usuários administradores podem incluir atores no sistema.");
                }

                var ator = await _servicoDeAtor.Incluir(requisicao.Dto);

                if (ator == null)
                {
                    return BadRequest("Erro! Não foi possível incluir o ator.");
                }

                return CreatedAtAction(nameof(GetObterPorId), new { ator.CodigoAtor }, ator);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DTODeRequisicaoGenerica<DTODeAtor> requisicao)
        {
            try
            {
                var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                {
                    return BadRequest("Apenas usuários administradores podem editar atores no sistema.");
                }

                var ator = await _servicoDeAtor.Editar(requisicao.Dto);

                if (ator == null)
                {
                    return BadRequest("Erro! Não foi possível editar o ator.");
                }

                return Ok(ator);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DTODeRequisicaoGenerica<DTODeAtor> requisicao)
        {
            try
            {
                var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                {
                    return BadRequest("Apenas usuários administradores podem excluir atores no sistema.");
                }

                var mensagemErro = await _servicoDeAtor.Exluir(requisicao.Dto);

                if (!string.IsNullOrWhiteSpace(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok("Ator excluído com sucesso!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        #endregion
    }
}
