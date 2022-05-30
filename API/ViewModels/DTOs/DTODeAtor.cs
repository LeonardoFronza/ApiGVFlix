using System;
using System.Collections.Generic;

namespace API.ViewModels.DTOs
{
    public class DTODeAtor
    {
        public DTODeAtor()
        {
            FilmesAtores = new List<DTODeFilmeAtor>();
        }

        public int CodigoAtor { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public ICollection<DTODeFilmeAtor> FilmesAtores { get; set; }
    }
}