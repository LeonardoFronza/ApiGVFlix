using System;
namespace API.ViewModels.DTOs
{
    public class DTODeFilmeAtor
    {
        public int CodigoFilmeAtor { get; set; }
        public int CodigoFilme { get; set; }
        public int CodigoAtor { get; set; }

        #region Propriedades de Navegação.

        public DTODeFilme Filme { get; set; }
        public DTODeAtor Ator { get; set; }

        #endregion
    }
}