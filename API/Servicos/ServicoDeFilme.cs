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
    public class ServicoDeFilme : IServicoDeFilme
    {
        #region Dependências

        private readonly IMapper _mapper;
        private readonly IConversor _conversor;
        private readonly IConsultaDeFilme _consultaDeFilme;
        private readonly IConsultaDeHistorico _consultaDeHistorico;
        private readonly IRepositorioDeFilme _repositorioDeFilme;

        #endregion

        public ServicoDeFilme(
            IMapper mapper,
            IConversor conversor,
            IConsultaDeFilme consultaDeFilme,
            IConsultaDeHistorico consultaDeHistorico,
            IRepositorioDeFilme repositorioDeFilme)
        {
            _mapper = mapper;
            _conversor = conversor;
            _consultaDeFilme = consultaDeFilme;
            _consultaDeHistorico = consultaDeHistorico;
            _repositorioDeFilme = repositorioDeFilme;
        }

        public async Task<DTODeFilme> Incluir(DTODeFilme dto)
        {
            var preenchimentoCorreto = ValidarPreenchimento(dto);

            if (!preenchimentoCorreto)
            {
                return null;
            }

            var registroDuplicado = await _consultaDeFilme.ValidarDuplicidade(dto, true);

            if (registroDuplicado)
            {
                return null;
            }

            var filme = _mapper.Map<Filme>(dto);

            try
            {
                filme = await _repositorioDeFilme.Incluir(filme);

                if (filme.CodigoFilme == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return null;
            }

            var dtoFilmeIncluido = await _consultaDeFilme.ObterPorId(filme.CodigoFilme);
            return dtoFilmeIncluido;
        }

        public async Task<DTODeFilme> Editar(DTODeFilme dto)
        {
            var filme = await _repositorioDeFilme.BuscarPorId(dto.CodigoFilme);

            if (filme == null)
            {
                return null;
            }

            var preenchimentoCorreto = ValidarPreenchimento(dto);

            if (!preenchimentoCorreto)
            {
                return null;
            }

            var registroDuplicado = await _consultaDeFilme.ValidarDuplicidade(dto, false);

            if (registroDuplicado)
            {
                return null;
            }

            filme.Titulo = _conversor.ConverterTextoIniciaisParaMaiusculo(dto.Titulo);
            filme.Descricao = dto.Descricao;
            filme.UrlImagem = dto.UrlImagem;
            filme.DataLancamento = dto.DataLancamento;

            try
            {
                filme = await _repositorioDeFilme.Editar(filme);
            }
            catch (Exception)
            {
                return null;
            }

            var dtoFilmeIncluido = await _consultaDeFilme.ObterPorId(filme.CodigoFilme);
            return dtoFilmeIncluido;
        }

        public async Task<string> Exluir(DTODeFilme dto)
        {
            var filme = await _repositorioDeFilme.BuscarPorId(dto.CodigoFilme);

            if (filme == null)
            {
                return "Erro! O filme não foi encontrado.";
            }

            if (filme.FilmesAtores.Any())
            {
                return "Erro! O filme possui vínculos com atores.";
            }

            var existeHistoricoDoFilme = await _consultaDeHistorico.ValidarExistenciaHistoricoDoFilme(filme.CodigoFilme);

            try
            {
                if (existeHistoricoDoFilme)
                {
                    filme.Excluido = true;
                    await _repositorioDeFilme.Editar(filme);
                }
                else
                {
                    await _repositorioDeFilme.Excluir(filme);
                }
            }
            catch (Exception)
            {
                return "Um erro geral ocorreu! Contate o administrador do sistema.";
            }

            return null;
        }

        private static bool ValidarPreenchimento(DTODeFilme dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo)
                || string.IsNullOrWhiteSpace(dto.Descricao)
                || string.IsNullOrWhiteSpace(dto.UrlImagem)
                || dto.DataLancamento > DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}
