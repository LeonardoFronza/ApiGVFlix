using System.Threading.Tasks;

namespace API.Consultas.IConsultas
{
    public interface IConsultaParaValidar<DTO>
    {
        Task<bool> ValidarDuplicidade(DTO dto, bool ignorarDistincaoId);
    }
}