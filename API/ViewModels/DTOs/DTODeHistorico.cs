using System;

namespace API.ViewModels.DTOs
{
    public class DTODeHistorico
    {
        public int CodigoHistorico { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoFilme { get; set; }
        public DateTimeOffset DataReproducao { get; set; }
        public int Nota { get; set; }

        #region Propriedades de Navegação.

        public DTODeUsuario Usuario { get; set; }
        public DTODeFilme Filme { get; set; }

        #endregion
    }
}