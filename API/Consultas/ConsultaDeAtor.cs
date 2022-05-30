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
    public class ConsultaDeAtor : IConsultaDeAtor
    {
        #region Dependências

        private readonly IMapper _mapper;
        private readonly IConversor _conversor;
        private readonly Contexto _contexto;

        #endregion

        public ConsultaDeAtor(IMapper mapper, IConversor conversor, Contexto contexto)
        {
            _mapper = mapper;
            _contexto = contexto;
            _conversor = conversor;
        }

        public async Task<DTODeAtor> ObterPorId(int id)
        {
            var consulta = await (
                from ator in _contexto.Atores.Include(a => a.FilmesAtores)
                where ator.CodigoAtor == id
                select _mapper.Map<DTODeAtor>(ator)
                ).FirstOrDefaultAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeAtor>> Listar()
        {
            var consulta = await(
                from ator in _contexto.Atores
                select _mapper.Map<ProjecaoDeAtor>(ator)
                ).ToListAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeAtor>> ListarComFiltro(string filtroNome)
        {
            if (string.IsNullOrWhiteSpace(filtroNome))
            {
                return await Listar();
            }

            filtroNome = _conversor.ConverterTextoIniciaisParaMaiusculo(filtroNome);

            var consulta = await (
                from ator in _contexto.Atores
                where ator.Nome.StartsWith(filtroNome)
                select _mapper.Map<ProjecaoDeAtor>(ator)
                ).ToListAsync();

            return consulta;
        }

        public async Task<bool> ValidarDuplicidade(DTODeAtor dto, bool ignorarDistincaoId)
        {
            var nomeFormatado = _conversor.ConverterTextoIniciaisParaMaiusculo(dto.Nome);

            var registroDuplicado = await (
                from ator in _contexto.Atores
                where (ignorarDistincaoId || ator.CodigoAtor != dto.CodigoAtor)
                    && ator.Nome.Equals(nomeFormatado)
                    && ator.DataNascimento == dto.DataNascimento
                select 1
                ).AnyAsync();

            return registroDuplicado;
        }
    }
}
