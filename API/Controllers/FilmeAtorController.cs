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
    [Route("api/filmesatores")]
    public class FilmeAtorController : ControllerBase
    {
        #region Dependencias

        private readonly IConsultaDeFilmeAtor _consultaDeFilmeAtor;
        private readonly IConsultaDeUsuario _consultaDeUsuario;
        private readonly IServicoDeFilmeAtor _servicoDeFilmeAtor;

        #endregion

        public FilmeAtorController(
            IConsultaDeFilmeAtor consultaDeFilmeAtor, 
            IConsultaDeUsuario consultaDeUsuario,
            IServicoDeFilmeAtor servicoDeFilmeAtor)
        {
            _consultaDeFilmeAtor = consultaDeFilmeAtor;
            _consultaDeUsuario = consultaDeUsuario;
            _servicoDeFilmeAtor = servicoDeFilmeAtor;
        }

        #region Métodos Http

        [HttpGet("{codigoFilmeAtor}")]
        public async Task<IActionResult> GetObterPorId(int codigoFilmeAtor)
        {
            try
            {
                var filmeAtor = await _consultaDeFilmeAtor.ObterPorId(codigoFilmeAtor);

                if (filmeAtor == null)
                {
                    return NotFound("Erro! O vínculo entre filme e ator não foi encontrado.");
                }

                return Ok(filmeAtor);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListar()
        {
            try
            {
                var registros = await _consultaDeFilmeAtor.Listar();
                return Ok(registros);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpGet("ListarPorFilme/{codigoFilme}")]
        public async Task<IActionResult> GetListarPorFilmeId(int codigoFilme)
        {
            try
            {
                var registros = await _consultaDeFilmeAtor.ListarPorFilmeId(codigoFilme);
                return Ok(registros);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpGet("ListarPorFilme")]
        public async Task<IActionResult> GetListarPorFilmeDescricao([FromQuery] string descricao)
        {
            try
            {
                var registros = await _consultaDeFilmeAtor.ListarPorFilmeDescricao(descricao);
                return Ok(registros);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpGet("ListarPorAtor/{codigoAtor}")]
        public async Task<IActionResult> GetListarPorAtorId(int codigoAtor)
        {
            try
            {
                var registros = await _consultaDeFilmeAtor.ListarPorAtorId(codigoAtor);
                return Ok(registros);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }        

        [HttpGet("ListarPorAtor")]
        public async Task<IActionResult> GetListarPorAtorDescricao([FromQuery] string descricao)
        {
            try
            {
                var registros = await _consultaDeFilmeAtor.ListarPorAtorDescricao(descricao);
                return Ok(registros);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTODeRequisicaoGenerica<DTODeFilmeAtor> requisicao)
        {
            try
            {
                var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                {
                    return BadRequest("Apenas usuários administradores podem incluir vínculos de filmes e atores no sistema.");
                }

                var filmeAtor = await _servicoDeFilmeAtor.Incluir(requisicao.Dto);

                if (filmeAtor == null)
                {
                    return BadRequest("Erro! Não foi possível incluir o vínculo entre filme e ator.");
                }

                return CreatedAtAction(nameof(GetObterPorId), new { filmeAtor.CodigoFilmeAtor }, filmeAtor);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DTODeRequisicaoGenerica<DTODeFilmeAtor> requisicao)
        {
            try
            {
                var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                {
                    return BadRequest("Apenas usuários administradores podem excluir vínculos de filmes e atores no sistema.");
                }

                var mensagemErro = await _servicoDeFilmeAtor.Exluir(requisicao.Dto);

                if (!string.IsNullOrWhiteSpace(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok("Vínculo entre filme e ator excluído com sucesso!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        #endregion
    }
}
