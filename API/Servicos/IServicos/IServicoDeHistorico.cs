using API.ViewModels.DTOs;
using System.Threading.Tasks;

namespace API.Servicos.IServicos
{
    public interface IServicoDeHistorico
    {
        Task<DTODeHistorico> Incluir(DTODeHistorico dto);
    }
}
