using System;
using System.Collections.Generic;

namespace API.ViewModels.DTOs
{
    public class DTODeFilme
    {
        public DTODeFilme()
        {
            FilmesAtores = new List<DTODeFilmeAtor>();
        }

        public int CodigoFilme { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataLancamento { get; set; }
        public string UrlImagem { get; set; }
        public ICollection<DTODeFilmeAtor> FilmesAtores { get; set; }
    }
}