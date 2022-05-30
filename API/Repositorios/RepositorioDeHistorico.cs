using API.Context;
using API.EntitieModels;
using API.Repositorios.IRepositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Repositorios
{
    public class RepositorioDeHistorico : IRepositorioDeHistorico
    {
        #region Dependências

        private readonly Contexto _contexto;

        #endregion

        public RepositorioDeHistorico(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Historico>> Buscar(Expression<Func<Historico, bool>> expression)
        {
            var historico = await _contexto.Historicos
                .Where(expression)
                .ToListAsync();

            return historico;
        }

        public async Task<Historico> BuscarPorId(int id)
        {
            var historico = await _contexto.Historicos.FindAsync(id);
            return historico;
        }

        public async Task<Historico> Incluir(Historico historico)
        {
            await _contexto.Historicos.AddAsync(historico);
            await _contexto.SaveChangesAsync();

            return historico;
        }
    }
}