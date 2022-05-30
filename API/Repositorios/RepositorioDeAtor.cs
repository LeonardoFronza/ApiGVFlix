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
    public class RepositorioDeAtor : IRepositorioDeAtor
    {
        #region Dependências

        private readonly Contexto _contexto;

        #endregion

        public RepositorioDeAtor(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Ator>> Buscar(Expression<Func<Ator, bool>> expression)
        {
            var atores = await _contexto.Atores
                .Include(a => a.FilmesAtores)
                .Where(expression)
                .ToListAsync();

            return atores;
        }

        public async Task<Ator> BuscarPorId(int id)
        {
            var ator = await _contexto.Atores
                .Include(a => a.FilmesAtores)
                .FirstOrDefaultAsync(a => a.CodigoAtor == id);

            return ator;
        }

        public async Task<Ator> Incluir(Ator entidade)
        {
            await _contexto.Atores.AddAsync(entidade);
            await _contexto.SaveChangesAsync();

            return entidade;
        }

        public async Task<Ator> Editar(Ator entidade)
        {
            _contexto.Atores.Update(entidade);
            await _contexto.SaveChangesAsync();

            return entidade;
        }

        public async Task Excluir(Ator entidade)
        {
            _contexto.Atores.Remove(entidade);
            await _contexto.SaveChangesAsync();
        }
    }
}
