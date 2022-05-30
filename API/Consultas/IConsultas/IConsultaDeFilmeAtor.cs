using API.EntitieModels;
using API.ViewModels.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Consultas.IConsultas
{
    public interface IConsultaDeFilmeAtor : IConsultaBase<DTODeFilmeAtor, ProjecaoDeFilmeAtor>
    {
        Task<IEnumerable<ProjecaoDeFilmeAtor>> ListarPorFilmeId(int codigoFilme);
        Task<IEnumerable<ProjecaoDeFilmeAtor>> ListarPorFilmeDescricao(string filtroTituloFilme);
        Task<IEnumerable<ProjecaoDeFilmeAtor>> ListarPorAtorId(int codigoAtor);
        Task<IEnumerable<ProjecaoDeFilmeAtor>> ListarPorAtorDescricao(string filtroNomeAtor);
    }
}