using API.Consultas.IConsultas;
using API.Context;
using API.EntitieModels;
using API.Ferramentas;
using API.ViewModels.DTOs;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Consultas
{
    public class ConsultaDeHistorico : IConsultaDeHistorico
    {
        #region Dependências

        private readonly IMapper _mapper;
        private readonly IConversor _conversor;
        private readonly Contexto _contexto;

        #endregion

        public ConsultaDeHistorico(IMapper mapper, IConversor conversor, Contexto contexto)
        {
            _mapper = mapper;
            _contexto = contexto;
            _conversor = conversor;
        }

        public async Task<DTODeHistorico> ObterPorId(int id)
        {
            var consulta = await (
                from historico in _contexto.Historicos.Include(h => h.Filme).Include(H => H.Usuario)
                where historico.CodigoHistorico == id
                select _mapper.Map<DTODeHistorico>(historico)
                ).FirstOrDefaultAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeHistorico>> Listar()
        {
            var consulta = await (
                from historico in _contexto.Historicos.Include(h => h.Filme).Include(h => h.Usuario)
                select _mapper.Map<ProjecaoDeHistorico>(historico)
                ).ToListAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeHistorico>> Listar(string filtroTitulo)
        {
            if (string.IsNullOrWhiteSpace(filtroTitulo))
            {
                return await Listar();
            }

            // Acredito que não precise, pois a consulta ocorre no banco (validar necessidade).
            filtroTitulo = _conversor.ConverterTextoIniciaisParaMaiusculo(filtroTitulo);

            var consulta = await (
                from historico in _contexto.Historicos.Include(h => h.Filme).Include(h => h.Usuario)
                where historico.Filme.Titulo.StartsWith(filtroTitulo)
                select _mapper.Map<ProjecaoDeHistorico>(historico)
                ).ToListAsync();

            return consulta;
        }

        public async Task<IEnumerable<ProjecaoDeHistorico>> ListarPorUsuario(int codigoUsuario, string filtroTituloFilme)
        {
            filtroTituloFilme = _conversor.ConverterTextoIniciaisParaMaiusculo(filtroTituloFilme);

            var consulta = await (
                from historico in _contexto.Historicos.Include(h => h.Filme).Include(h => h.Usuario)
                where historico.CodigoUsuario == codigoUsuario
                    && (string.IsNullOrWhiteSpace(filtroTituloFilme) || historico.Filme.Titulo.StartsWith(filtroTituloFilme))
                select _mapper.Map<ProjecaoDeHistorico>(historico)
                ).ToListAsync();

            return consulta;
        }

        public async Task<bool> ValidarDuplicidade(DTODeHistorico dto, bool ignorarDistincaoId)
        {
            var registroDuplicado = await (
                from historico in _contexto.Historicos.Include(h => h.Filme).Include(h => h.Usuario)
                where (ignorarDistincaoId || historico.CodigoHistorico != dto.CodigoHistorico)
                    && historico.CodigoFilme == dto.CodigoFilme
                    && historico.CodigoUsuario == dto.CodigoUsuario
                    && historico.DataReproducao == dto.DataReproducao
                select 1
                ).AnyAsync();

            return registroDuplicado;
        }

        public async Task<bool> ValidarExistenciaHistoricoDoFilme(int codigoFilme)
        {
            var existeHistoricoDoFilme = await (
                from historico in _contexto.Historicos
                where historico.CodigoFilme == codigoFilme
                select 1
                ).AnyAsync();

            return existeHistoricoDoFilme;
        }

        public async Task<bool> ValidarExistenciaHistoricoDoUsuario(int codigoUsuario)
        {
            var existeHistoricoDoUsuario = await (
                from historico in _contexto.Historicos
                where historico.CodigoUsuario == codigoUsuario
                select 1
                ).AnyAsync();

            return existeHistoricoDoUsuario;
        }
    }
}