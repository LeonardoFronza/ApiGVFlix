using System;
using System.Collections.Generic;

namespace API.EntitieModels
{
    public class Ator
    {
        public Ator()
        {
            FilmesAtores = new List<FilmeAtor>();
        }

        public int CodigoAtor { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public virtual ICollection<FilmeAtor> FilmesAtores { get; set; }
    }
}