using API.EntitieModels;
using API.ViewModels.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Consultas.IConsultas
{
    public interface IConsultaDeHistorico : IConsultaBase<DTODeHistorico, ProjecaoDeHistorico>
    {
        Task<IEnumerable<ProjecaoDeHistorico>> ListarPorUsuario(int codigoUsuario, string filtroTituloFilme);

        Task<bool> ValidarExistenciaHistoricoDoFilme(int codigoFilme);
        Task<bool> ValidarExistenciaHistoricoDoUsuario(int codigoUsuario);
    }
}