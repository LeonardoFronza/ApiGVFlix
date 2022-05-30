using API.Consultas.IConsultas;
using API.EntitieModels;
using API.Repositorios.IRepositorios;
using API.Servicos.IServicos;
using API.ViewModels.DTOs;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace API.Servicos
{
    public class ServicoDeFilmeAtor : IServicoDeFilmeAtor
    {
        #region Dependencias

        private readonly IMapper _mapper;
        private readonly IConsultaDeAtor _consultaDeAtor;
        private readonly IConsultaDeFilme _consultaDeFilme;
        private readonly IConsultaDeFilmeAtor _consultaDeFilmeAtor;
        private readonly IRepositorioDeFilmeAtor _repositorioDeFilmeAtor;

        #endregion

        public ServicoDeFilmeAtor(
            IMapper mapper,
            IConsultaDeAtor consultaDeAtor,
            IConsultaDeFilme consultaDeFilme,
            IConsultaDeFilmeAtor consultaDeFilmeAtor, 
            IRepositorioDeFilmeAtor repositorioDeFilmeAtor)
        {
            _mapper = mapper;
            _consultaDeAtor = consultaDeAtor;
            _consultaDeFilme = consultaDeFilme;
            _consultaDeFilmeAtor = consultaDeFilmeAtor;
            _repositorioDeFilmeAtor = repositorioDeFilmeAtor;
        }

        public async Task<DTODeFilmeAtor> Incluir(DTODeFilmeAtor dto)
        {
            var preenchimentoIncorreto = await ValidarPreenchimentoIncorreto(dto);

            if (preenchimentoIncorreto)
            {
                return null;
            }

            var registroDuplicado = await _consultaDeFilmeAtor.ValidarDuplicidade(dto, true);

            if (registroDuplicado)
            {
                return null;
            }

            var filmeAtor = _mapper.Map<FilmeAtor>(dto);

            try
            {
                filmeAtor = await _repositorioDeFilmeAtor.Incluir(filmeAtor);

                if (filmeAtor.CodigoFilmeAtor == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return null;
            }

            var dtoFilmeAtorIncluido = await _consultaDeFilmeAtor.ObterPorId(filmeAtor.CodigoFilmeAtor);
            return dtoFilmeAtorIncluido;
        }

        public async Task<string> Exluir(DTODeFilmeAtor dto)
        {
            var filmeAtor = await _repositorioDeFilmeAtor.BuscarPorId(dto.CodigoFilmeAtor);

            if (filmeAtor == null)
            {
                return "Erro! O vínculo entre filme e ator não foi encontrado.";
            }

            try
            {
                await _repositorioDeFilmeAtor.Excluir(filmeAtor);
            }
            catch (Exception)
            {
                return "Um erro geral ocorreu! Entre em contato com o administrador do sistema.";
            }

            return string.Empty;
        }

        private async Task<bool> ValidarPreenchimentoIncorreto(DTODeFilmeAtor dto)
        {
            var filme = await _consultaDeFilme.ObterPorId(dto.CodigoFilme);
            var ator = await _consultaDeAtor.ObterPorId(dto.CodigoAtor);

            if (filme == null || ator == null)
            {
                return true;
            }

            return false;
        }
    }
}
