using API.EntitieModels;
using API.ViewModels.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Consultas.IConsultas
{
    public interface IConsultaDeAtor : IConsultaBase<DTODeAtor, ProjecaoDeAtor>
    {
        Task<IEnumerable<ProjecaoDeAtor>> ListarComFiltro(string filtroNome);
    }
}
