using System;
using System.Collections.Generic;

namespace API.EntitieModels
{
    public class Filme
    {
        public Filme()
        {
            FilmesAtores = new List<FilmeAtor>();
        }

        public int CodigoFilme { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataLancamento { get; set; }
        public string UrlImagem { get; set; }
        public bool Excluido { get; set; }
        public virtual ICollection<FilmeAtor> FilmesAtores { get; set; }
    }
}
