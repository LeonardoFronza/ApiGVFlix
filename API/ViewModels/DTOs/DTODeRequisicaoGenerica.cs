using System;
namespace API.ViewModels.DTOs
{
    public class DTODeRequisicaoGenerica<T>
    {
        public int CodigoUsuarioLogado { get; set; }
        public T Dto { get; set; }
    }
}
