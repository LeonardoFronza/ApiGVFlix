using API.Enumeradores;
using System;

namespace API.ViewModels.DTOs
{
    public class DTODeUsuario
    {
        public int CodigoUsuario { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public Role Role { get; set; }
    }
}