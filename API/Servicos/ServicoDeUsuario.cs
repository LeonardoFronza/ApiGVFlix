using API.Consultas.IConsultas;
using API.EntitieModels;
using API.Enumeradores;
using API.Ferramentas;
using API.Repositorios.IRepositorios;
using API.Servicos.IServicos;
using API.ViewModels.DTOs;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace API.Servicos
{
    public class ServicoDeUsuario : IServicoDeUsuario
    {
        #region Dependências

        private readonly IMapper _mapper;
        private readonly IConversor _conversor;
        private readonly IConsultaDeUsuario _consultaDeUsuario;
        private readonly IConsultaDeHistorico _consultaDeHistorico;
        private readonly IRepositorioDeUsuario _repositorioDeUsuario;

        #endregion

        public ServicoDeUsuario(
            IMapper mapper,
            IConversor conversor,
            IConsultaDeUsuario consultaDeUsuario,
            IConsultaDeHistorico consultaDeHistorico,
            IRepositorioDeUsuario repositorioDeUsuario)
        {
            _mapper = mapper;
            _conversor = conversor;
            _consultaDeUsuario = consultaDeUsuario;
            _consultaDeHistorico = consultaDeHistorico;
            _repositorioDeUsuario = repositorioDeUsuario;
        }

        public async Task<DTODeUsuario> Incluir(DTODeUsuario dto)
        {
            var preenchimentoCorreto = ValidarPreenchimento(dto);

            if (!preenchimentoCorreto)
            {
                return null;
            }

            var registroDuplicado = await _consultaDeUsuario.ValidarDuplicidade(dto, true);

            if (registroDuplicado)
            {
                return null;
            }

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.Role = Role.Cliente;

            try
            {
                usuario = await _repositorioDeUsuario.Incluir(usuario);

                if (usuario.CodigoUsuario == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return null;
            }

            var dtoUsuarioIncluido = await _consultaDeUsuario.ObterPorId(usuario.CodigoUsuario);
            return dtoUsuarioIncluido;
        }

        public async Task<DTODeUsuario> Editar(DTODeUsuario dto)
        {
            var usuario = await _repositorioDeUsuario.BuscarPorId(dto.CodigoUsuario);

            if (usuario == null)
            {
                return null;
            }

            var preenchimentoCorreto = ValidarPreenchimento(dto);

            if (!preenchimentoCorreto)
            {
                return null;
            }

            var registroDuplicado = await _consultaDeUsuario.ValidarDuplicidade(dto, false);

            if (registroDuplicado)
            {
                return null;
            }

            usuario.Nome = _conversor.ConverterTextoIniciaisParaMaiusculo(dto.Nome);
            usuario.Email = dto.Email;
            usuario.Senha = dto.Senha;
            usuario.DataNascimento = dto.DataNascimento;

            try
            {
                usuario = await _repositorioDeUsuario.Editar(usuario);
            }
            catch (Exception)
            {
                return null;
            }

            var dtoUsuarioIncluido = await _consultaDeUsuario.ObterPorId(usuario.CodigoUsuario);
            return dtoUsuarioIncluido;
        }

        public async Task<string> Exluir(DTODeUsuario dto)
        {
            var usuario = await _repositorioDeUsuario.BuscarPorId(dto.CodigoUsuario);

            if (usuario == null)
            {
                return "Erro! O usuário não foi encontrado.";
            }

            if (usuario.Role == Role.Administrador)
            {
                return "Erro! Apenas usuários do tipo cliente podem ser excluídos.";
            }

            var existeHistoricoDoUsuario = await _consultaDeHistorico.ValidarExistenciaHistoricoDoUsuario(usuario.CodigoUsuario);

            try
            {
                if (existeHistoricoDoUsuario)
                {
                    usuario.Excluido = true;
                    await _repositorioDeUsuario.Editar(usuario);
                }
                else
                {
                    await _repositorioDeUsuario.Excluir(usuario);
                }
            }
            catch (Exception)
            {
                return "Um erro geral ocorreu! Contate o administrador do sistema.";
            }

            return null;
        }

        private bool ValidarPreenchimento(DTODeUsuario dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome)
                || string.IsNullOrWhiteSpace(dto.Email)
                || string.IsNullOrWhiteSpace(dto.Senha)
                || dto.DataNascimento > DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}
