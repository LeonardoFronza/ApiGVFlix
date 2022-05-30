using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Repositorios.IRepositorios
{
    public interface IRepositorioBase<T>
    {
        Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> expression);

        Task<T> BuscarPorId(int id);

        Task<T> Incluir(T entidade);

        Task<T> Editar(T entidade);

        Task Excluir(T entidade);
    }
}
