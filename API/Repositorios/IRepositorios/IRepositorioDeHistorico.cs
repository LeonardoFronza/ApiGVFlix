using API.EntitieModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Repositorios.IRepositorios
{
    public interface IRepositorioDeHistorico
    {
        Task<IEnumerable<Historico>> Buscar(Expression<Func<Historico, bool>> expression);

        Task<Historico> BuscarPorId(int id);

        Task<Historico> Incluir(Historico historico);
    }
}
