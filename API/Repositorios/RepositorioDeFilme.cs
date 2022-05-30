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
    public class RepositorioDeFilme : IRepositorioDeFilme
    {
        #region Dependências

        private readonly Contexto _contexto;

        #endregion

        public RepositorioDeFilme(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Filme>> Buscar(Expression<Func<Filme, bool>> expression)
        {
            var filmes = await _contexto.Filmes
                .Include(f => f.FilmesAtores)
                .Where(expression)
                .ToListAsync();

            return filmes;
        }

        public async Task<Filme> BuscarPorId(int id)
        {
            var filme = await _contexto.Filmes
                .Include(f => f.FilmesAtores)
                .FirstOrDefaultAsync(f => f.CodigoFilme == id);

            return filme;
        }

        public async Task<Filme> Incluir(Filme filme)
        {
            await _contexto.Filmes.AddAsync(filme);
            await _contexto.SaveChangesAsync();

            return filme;
        }

        public async Task<Filme> Editar(Filme filme)
        {
            _contexto.Filmes.Update(filme);
            await _contexto.SaveChangesAsync();

            return filme;
        }

        public async Task Excluir(Filme filme)
        {
            _contexto.Filmes.Remove(filme);
            await _contexto.SaveChangesAsync();
        }
    }
}
