using System;

namespace API.EntitieModels
{
    public class FilmeAtor
    {
        public int CodigoFilmeAtor { get; set; }
        public int CodigoFilme { get; set; }
        public int CodigoAtor { get; set; }

        #region Propriedades de Navegação.

        public virtual Filme Filme { get; set; }
        public virtual Ator Ator { get; set; }

        #endregion
    }
}