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
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        #region Dependencias

        private readonly IConsultaDeUsuario _consultaDeUsuario;
        private readonly IServicoDeUsuario _servicoDeUsuario;

        #endregion

        public UsuarioController(IConsultaDeUsuario consultaDeUsuario, IServicoDeUsuario servicoDeUsuario)
        {
            _consultaDeUsuario = consultaDeUsuario;
            _servicoDeUsuario = servicoDeUsuario;
        }

        #region Métodos Http

        [HttpGet("{codigoUsuario}")]
        public async Task<IActionResult> GetObterPorId(int codigoUsuario)
        {
            try
            {
                var usuario = await _consultaDeUsuario.ObterPorId(codigoUsuario);

                if (usuario == null)
                {
                    return NotFound("Erro! O usuário não foi encontrado.");
                }

                return Ok(usuario);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpPost("Logar")]
        public async Task<IActionResult> PostLogar([FromBody] DTODeRequisicaoLogin dto)
        {
            try
            {
                var usuario = await _consultaDeUsuario.ObterPorLogin(dto);

                if (usuario == null)
                {
                    return NotFound("Erro! email ou senha inválidos.");
                }

                return Ok(usuario);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTODeUsuario dto)
        {
            try
            {
                var usuario = await _servicoDeUsuario.Incluir(dto);

                if (usuario == null)
                {
                    return BadRequest("Erro! Não foi possível incluir o usuário.");
                }

                return CreatedAtAction(nameof(GetObterPorId), new { usuario.CodigoUsuario }, usuario);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DTODeRequisicaoGenerica<DTODeUsuario> requisicao)
        {
            try
            {
                if (requisicao.CodigoUsuarioLogado != requisicao.Dto.CodigoUsuario)
                {
                    var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                    if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                    {
                        return BadRequest("Apenas o próprio usuário ou administradores podem editar registros de usuários no sistema.");
                    }
                }

                var usuario = await _servicoDeUsuario.Editar(requisicao.Dto);

                if (usuario == null)
                {
                    return BadRequest("Erro! Não foi possível editar o usuário.");
                }

                return Ok(usuario);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DTODeRequisicaoGenerica<DTODeUsuario> requisicao)
        {
            try
            {
                if (requisicao.CodigoUsuarioLogado != requisicao.Dto.CodigoUsuario)
                {
                    var usuarioLogado = await _consultaDeUsuario.ObterPorId(requisicao.CodigoUsuarioLogado);

                    if (usuarioLogado == null || usuarioLogado.Role != Role.Administrador)
                    {
                        return BadRequest("Apenas o próprio usuário ou administradores podem excluir registros de usuários no sistema.");
                    }
                }

                var mensagemErro = await _servicoDeUsuario.Exluir(requisicao.Dto);

                if (!string.IsNullOrWhiteSpace(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok("Usuário excluído com sucesso!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Um erro geral ocorreu. Contate o administrador do sistema.");
            }
        }

        #endregion
    }
}