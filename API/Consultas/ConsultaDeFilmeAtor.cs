using API.Consultas.IConsultas;
using API.Context;
using API.EntitieModels;
using API.Ferramentas;
using API.ViewModels.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Consultas
{
    public class ConsultaDeFilmeAtor : IConsultaDeFilmeAtor
    {
        #region Dependências

        private readonly IMapper _mapper;
        private readonly IConversor _conversor;
        private readonly Contexto _contexto;

        #endregion

        public ConsultaDeFilmeAtor(IMapper mapper, IConversor conversor, Contexto contexto)
        {
            _mapper = mapper;
            _contexto = contexto;
            _conversor = conversor;
        }

        public async Task<DTODeFilmeAtor> ObterPorId(int id)
        {
            var consulta = await (
                from filmeAtor in _contexto.FilmesAtores.Include(fa => fa.Filme).Include(fa => fa.Ator)
                where filmeAtor.CodigoFilmeAtor == id
                select _mapper.Map<DTODeFilmeAtor>(filmeAtor)
                ).FirstOrDefaultAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeFilmeAtor>> Listar()
        {
            var consulta = await (
                from filmeAtor in _contexto.FilmesAtores.Include(fa => fa.Filme).Include(fa => fa.Ator)
                select _mapper.Map<ProjecaoDeFilmeAtor>(filmeAtor)
                ).ToListAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeFilmeAtor>> ListarPorFilmeId(int codigoFilme)
        {
            var consulta = await(
                from filmeAtor in _contexto.FilmesAtores.Include(fa => fa.Filme).Include(fa => fa.Ator)
                where filmeAtor.CodigoFilme == codigoFilme
                select _mapper.Map<ProjecaoDeFilmeAtor>(filmeAtor)
                ).ToListAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeFilmeAtor>> ListarPorFilmeDescricao(string filtroTituloFilme)
        {
            if (string.IsNullOrWhiteSpace(filtroTituloFilme))
            {
                return await Listar();
            }

            filtroTituloFilme = _conversor.ConverterTextoIniciaisParaMaiusculo(filtroTituloFilme);

            var consulta = await (
                from filmeAtor in _contexto.FilmesAtores.Include(fa => fa.Filme).Include(fa => fa.Ator)
                where filmeAtor.Filme.Titulo.StartsWith(filtroTituloFilme)
                select _mapper.Map<ProjecaoDeFilmeAtor>(filmeAtor)
                ).ToListAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeFilmeAtor>> ListarPorAtorId(int codigoAtor)
        {
            var consulta = await (
                from filmeAtor in _contexto.FilmesAtores.Include(fa => fa.Filme).Include(fa => fa.Ator)
                where filmeAtor.CodigoAtor == codigoAtor
                select _mapper.Map<ProjecaoDeFilmeAtor>(filmeAtor)
                ).ToListAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeFilmeAtor>> ListarPorAtorDescricao(string filtroNomeAtor)
        {
            if (string.IsNullOrWhiteSpace(filtroNomeAtor))
            {
                return await Listar();
            }

            filtroNomeAtor = _conversor.ConverterTextoIniciaisParaMaiusculo(filtroNomeAtor);

            var consulta = await (
                from filmeAtor in _contexto.FilmesAtores.Include(fa => fa.Filme).Include(fa => fa.Ator)
                where filmeAtor.Ator.Nome.StartsWith(filtroNomeAtor)
                select _mapper.Map<ProjecaoDeFilmeAtor>(filmeAtor)
                ).ToListAsync();

            return consulta;
        }

        public async Task<bool> ValidarDuplicidade(DTODeFilmeAtor dto, bool ignorarDistincaoId)
        {
            var registroDuplicado = await (
                from filmeAtor in _contexto.FilmesAtores.Include(fa => fa.Filme).Include(fa => fa.Ator)
                where (ignorarDistincaoId || filmeAtor.CodigoFilmeAtor != dto.CodigoFilmeAtor)
                    && filmeAtor.CodigoFilme == dto.CodigoFilme
                    && filmeAtor.CodigoAtor == dto.CodigoAtor
                select 1
                ).AnyAsync();

            return registroDuplicado;
        }
    }
}