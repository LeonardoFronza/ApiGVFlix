using System;
using System.Threading.Tasks;

namespace API.Servicos.IServicos
{
    public interface IServicoBase<DTO>
    {
        Task<DTO> Incluir(DTO dto);

        Task<DTO> Editar(DTO dto);

        Task<string> Exluir(DTO dto);
    }
}