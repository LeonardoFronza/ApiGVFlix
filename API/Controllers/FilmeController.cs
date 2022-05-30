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
    [Route("api/filmes")]
    public class FilmeController : ControllerBase
    {
        #region Dependencias

        private readonly IConsultaDeFilme _consultaDeFilme;
        private readonly IConsultaDeUsuario _consultaDeUsuario;
        private readonly IServicoDeFilme _servicoDeFilme;

        #endregion

        public FilmeController(
            IConsultaDeFilme consultaDeFilme,
            IConsultaDeUsuario consultaDeUsuario,
            IServicoDeFilme servicoDeFilme)
        {
            _consultaDeFilme = consultaDeFilme;
            _consultaDeUsuario = consultaDeUsuario;
            _servicoDeFilme = servicoDeFilme;
        }

        #region Métodos Http

        [HttpGet("{codigoFilme}")]
        public async Task<IActionResult> GetObterPorId(int codigoFilme)
        {
            try
            {
                var filme = await _consultaDeFilme.ObterPorId(codigoFilme);

                if (filme == null)
                {
                    return NotFound("Erro! O filme não foi encontrado.");
                }

                return Ok(filme);
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
                var registros = await _consultaDeFilme.ListarComFiltro(filtro);
                return Ok(registros);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTODeRequisicaoGenerica<DTODeFilme> requisicao)
        {
            try
            {
                var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                {
                    return BadRequest("Apenas usuários administradores podem incluir filmes no sistema.");
                }

                var filme = await _servicoDeFilme.Incluir(requisicao.Dto);

                if (filme == null)
                {
                    return BadRequest("Erro! Não foi possível incluir o filme.");
                }

                return CreatedAtAction(nameof(GetObterPorId), new { filme.CodigoFilme }, filme);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DTODeRequisicaoGenerica<DTODeFilme> requisicao)
        {
            try
            {
                var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                {
                    return BadRequest("Apenas usuários administradores podem editar filmes no sistema.");
                }

                var filme = await _servicoDeFilme.Editar(requisicao.Dto);

                if (filme == null)
                {
                    return BadRequest("Erro! Não foi possível editar o filme.");
                }

                return Ok(filme);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DTODeRequisicaoGenerica<DTODeFilme> requisicao)
        {
            try
            {
                var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                {
                    return BadRequest("Apenas usuários administradores podem excluir filmes no sistema.");
                }

                var mensagemErro = await _servicoDeFilme.Exluir(requisicao.Dto);

                if (!string.IsNullOrWhiteSpace(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok("Filme excluído com sucesso!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        #endregion
    }
}
