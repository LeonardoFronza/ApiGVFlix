using API.ViewModels.DTOs;
using System.Threading.Tasks;

namespace API.Servicos.IServicos
{
    public interface IServicoDeFilmeAtor
    {
        Task<DTODeFilmeAtor> Incluir(DTODeFilmeAtor dto);

        Task<string> Exluir(DTODeFilmeAtor dto);
    }
}
