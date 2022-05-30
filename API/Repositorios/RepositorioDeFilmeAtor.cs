using API.Context;
using API.EntitieModels;
using API.Repositorios.IRepositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Repositorios
{
    public class RepositorioDeFilmeAtor : IRepositorioDeFilmeAtor
    {
        #region Dependências

        private readonly Contexto _contexto;

        #endregion

        public RepositorioDeFilmeAtor(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<FilmeAtor>> Buscar(Expression<Func<FilmeAtor, bool>> expression)
        {
            var filmeAtor = await _contexto.FilmesAtores
                .Include(fa => fa.Ator)
                .Include(fa => fa.Filme)
                .Where(expression)
                .ToListAsync();

            return filmeAtor;
        }

        public async Task<FilmeAtor> BuscarPorId(int id)
        {
            var filmeAtor = await _contexto.FilmesAtores
                .Include(fa => fa.Ator)
                .Include(fa => fa.Filme)
                .FirstOrDefaultAsync(fa => fa.CodigoFilmeAtor == id);

            return filmeAtor;
        }

        public async Task<FilmeAtor> Incluir(FilmeAtor filmeAtor)
        {
            await _contexto.FilmesAtores.AddAsync(filmeAtor);
            await _contexto.SaveChangesAsync();

            return filmeAtor;
        }

        public async Task<FilmeAtor> Editar(FilmeAtor filmeAtor)
        {
            _contexto.FilmesAtores.Update(filmeAtor);
            await _contexto.SaveChangesAsync();

            return filmeAtor;
        }

        public async Task Excluir(FilmeAtor filmeAtor)
        {
            _contexto.FilmesAtores.Remove(filmeAtor);
            await _contexto.SaveChangesAsync();
        }
    }
}
