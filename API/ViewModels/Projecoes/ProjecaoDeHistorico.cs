using System;

namespace API.EntitieModels
{
    public class ProjecaoDeHistorico
    {
        public int CodigoHistorico { get; set; }
        public string NomeUsuario { get; set; }
        public string TituloFilme { get; set; }
        public string DataReproducao { get; set; }
        public int Nota { get; set; }
    }
}
