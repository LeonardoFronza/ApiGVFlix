using API.Enumeradores;
using System;

namespace API.EntitieModels
{
    public class Usuario
    {
        public int CodigoUsuario { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public Role Role { get; set; }
        public bool Excluido { get; set; }
    }
}