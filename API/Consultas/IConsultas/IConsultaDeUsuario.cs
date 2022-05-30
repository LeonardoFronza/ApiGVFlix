using API.ViewModels.DTOs;
using System.Threading.Tasks;

namespace API.Consultas.IConsultas
{
    public interface IConsultaDeUsuario : IConsultaParaValidar<DTODeUsuario>
    {
        Task<DTODeUsuario> ObterPorId(int id);
        Task<DTODeUsuario> ObterPorLogin(DTODeRequisicaoLogin login);
    }
}
