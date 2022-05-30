using API.Consultas.IConsultas;
using API.EntitieModels;
using API.Ferramentas;
using API.Repositorios.IRepositorios;
using API.Servicos.IServicos;
using API.ViewModels.DTOs;
using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servicos
{
    public class ServicoDeAtor : IServicoDeAtor
    {
        #region Dependências

        private readonly IMapper _mapper;
        private readonly IConversor _conversor;
        private readonly IConsultaDeAtor _consultaDeAtor;
        private readonly IRepositorioDeAtor _repositorioDeAtor;

        #endregion

        public ServicoDeAtor(
            IMapper mapper,
            IConversor conversor, 
            IConsultaDeAtor consultaDeAtor,
            IRepositorioDeAtor repositorioDeAtor)
        {
            _mapper = mapper;
            _conversor = conversor;
            _consultaDeAtor = consultaDeAtor;
            _repositorioDeAtor = repositorioDeAtor;
        }

        public async Task<DTODeAtor> Incluir(DTODeAtor dto)
        {
            var preenchimentoCorreto = ValidarPreenchimento(dto);

            if (!preenchimentoCorreto)
            {
                return null;
            }

            var registroDuplicado = await _consultaDeAtor.ValidarDuplicidade(dto, true);

            if (registroDuplicado)
            {
                return null;
            }

            var ator = _mapper.Map<Ator>(dto);
            
            try
            {
                ator = await _repositorioDeAtor.Incluir(ator);

                if (ator.CodigoAtor == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return null;
            }

            var dtoAtorIncluido = await _consultaDeAtor.ObterPorId(ator.CodigoAtor);
            return dtoAtorIncluido;
        }

        public async Task<DTODeAtor> Editar(DTODeAtor dto)
        {
            var ator = await _repositorioDeAtor.BuscarPorId(dto.CodigoAtor);

            if (ator == null)
            {
                return null;
            }

            var preenchimentoCorreto = ValidarPreenchimento(dto);

            if (!preenchimentoCorreto)
            {
                return null;
            }

            var registroDuplicado = await _consultaDeAtor.ValidarDuplicidade(dto, false);

            if (registroDuplicado)
            {
                return null;
            }

            ator.Nome = _conversor.ConverterTextoIniciaisParaMaiusculo(dto.Nome);
            ator.DataNascimento = dto.DataNascimento;

            try
            {
                ator = await _repositorioDeAtor.Editar(ator);
            }
            catch (Exception)
            {
                return null;
            }

            var dtoAtorEditado = await _consultaDeAtor.ObterPorId(ator.CodigoAtor);
            return dtoAtorEditado;
        }

        public async Task<string> Exluir(DTODeAtor dto)
        {
            var ator = await _repositorioDeAtor.BuscarPorId(dto.CodigoAtor);

            if (ator == null)
            {
                return "Erro! O ator não foi encontrado.";
            }

            if (ator.FilmesAtores.Any())
            {
                return "Erro! o ator possui vínculos com filmes.";
            }

            try
            {
                await _repositorioDeAtor.Excluir(ator);
            }
            catch (Exception)
            {
                return "Um erro geral ocorreu! Contate o administrador do sistema.";
            }

            return null;
        }

        private static bool ValidarPreenchimento(DTODeAtor dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) 
                || dto.DataNascimento > DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}
