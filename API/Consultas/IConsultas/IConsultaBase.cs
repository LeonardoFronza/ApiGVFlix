using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Consultas.IConsultas
{
    public interface IConsultaBase<DTO, Projecao> : IConsultaParaValidar<DTO>
    {
        Task<DTO> ObterPorId(int id);

        Task<IEnumerable<Projecao>> Listar();
    }
}
