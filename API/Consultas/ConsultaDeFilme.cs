using API.Consultas.IConsultas;
using API.Context;
using API.EntitieModels;
using API.Ferramentas;
using API.ViewModels.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Consultas
{
    public class ConsultaDeFilme : IConsultaDeFilme
    {
        #region Dependências

        private readonly IMapper _mapper;
        private readonly IConversor _conversor;
        private readonly Contexto _contexto;

        #endregion

        public ConsultaDeFilme(IMapper mapper, IConversor conversor, Contexto contexto)
        {
            _mapper = mapper;
            _contexto = contexto;
            _conversor = conversor;
        }

        public async Task<DTODeFilme> ObterPorId(int id)
        {
            var consulta = await (
                from filme in _contexto.Filmes.Include(f => f.FilmesAtores)
                where !filme.Excluido
                    && filme.CodigoFilme == id
                select _mapper.Map<DTODeFilme>(filme)
                ).FirstOrDefaultAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeFilme>> Listar()
        {
            var consulta = await (
                from filme in _contexto.Filmes
                where !filme.Excluido
                select _mapper.Map<ProjecaoDeFilme>(filme)
                ).ToListAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeFilme>> ListarComFiltro(string filtroTitulo)
        {
            if (string.IsNullOrWhiteSpace(filtroTitulo))
            {
                return await Listar();
            }

            // Acredito que não precise, pois a consulta ocorre no banco (validar necessidade).
            filtroTitulo = _conversor.ConverterTextoIniciaisParaMaiusculo(filtroTitulo);

            var consulta = await (
                from filme in _contexto.Filmes
                where !filme.Excluido
                    && filme.Titulo.StartsWith(filtroTitulo)
                select _mapper.Map<ProjecaoDeFilme>(filme)
                ).ToListAsync();

            return consulta;
        }

        public async Task<bool> ValidarDuplicidade(DTODeFilme dto, bool ignorarDistincaoId)
        {
            // Acredito que não precise, pois a consulta ocorre no banco (validar necessidade).
            var tituloFilme = _conversor.ConverterTextoIniciaisParaMaiusculo(dto.Titulo);

            var registroDuplicado = await (
                from filme in _contexto.Filmes
                where !filme.Excluido
                    && (ignorarDistincaoId || filme.CodigoFilme != dto.CodigoFilme)
                    && filme.Titulo.Equals(tituloFilme)
                    && filme.DataLancamento == dto.DataLancamento
                select 1
                ).AnyAsync();

            return registroDuplicado;
        }
    }
}