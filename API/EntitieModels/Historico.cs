using System;

namespace API.EntitieModels
{
    public class Historico
    {
        public int CodigoHistorico { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoFilme { get; set; }
        public DateTimeOffset DataReproducao { get; set; }
        public int Nota { get; set; }

        #region Propriedades de Navegação.

        public virtual Usuario Usuario { get; set; }
        public virtual Filme Filme { get; set; }

        #endregion
    }
}