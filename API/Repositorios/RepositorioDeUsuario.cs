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
    public class RepositorioDeUsuario : IRepositorioDeUsuario
    {
        #region Dependências

        private readonly Contexto _contexto;

        #endregion

        public RepositorioDeUsuario(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Usuario>> Buscar(Expression<Func<Usuario, bool>> expression)
        {
            var usuario = await _contexto.Usuarios
                .Where(expression)
                .ToListAsync();

            return usuario;
        }

        public async Task<Usuario> BuscarPorId(int id)
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);
            return usuario;
        }

        public async Task<Usuario> Incluir(Usuario usuario)
        {
            await _contexto.Usuarios.AddAsync(usuario);
            await _contexto.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario> Editar(Usuario usuario)
        {
            _contexto.Usuarios.Update(usuario);
            await _contexto.SaveChangesAsync();

            return usuario;
        }

        public async Task Excluir(Usuario usuario)
        {
            _contexto.Usuarios.Remove(usuario);
            await _contexto.SaveChangesAsync();
        }
    }
}
