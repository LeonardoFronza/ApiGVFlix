using API.EntitieModels;
using API.ViewModels.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Consultas.IConsultas
{
    public interface IConsultaDeFilme : IConsultaBase<DTODeFilme, ProjecaoDeFilme>
    {
        Task<IEnumerable<ProjecaoDeFilme>> ListarComFiltro(string filtroTitulo);
    }
}