using API.Consultas.IConsultas;
using API.Context;
using API.Ferramentas;
using API.ViewModels.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Consultas
{
    public class ConsultaDeUsuario : IConsultaDeUsuario
    {
        #region Dependências

        private readonly IMapper _mapper;
        private readonly IConversor _conversor;
        private readonly Contexto _contexto;

        #endregion

        public ConsultaDeUsuario(IMapper mapper, IConversor conversor, Contexto contexto)
        {
            _mapper = mapper;
            _contexto = contexto;
            _conversor = conversor;
        }

        public async Task<DTODeUsuario> ObterPorId(int id)
        {
            var consulta = await (
                from usuario in _contexto.Usuarios
                where !usuario.Excluido
                    && usuario.CodigoUsuario == id
                select _mapper.Map<DTODeUsuario>(usuario)
                ).FirstOrDefaultAsync();

            return consulta;
        }

        public async Task<DTODeUsuario> ObterPorLogin(DTODeRequisicaoLogin login)
        {
            var email = login.Email;
            var senha = login.Senha;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                return null;
            }

            email = _conversor.ConverterTextoParaMinusculo(email);

            var consulta = await (
                from usuario in _contexto.Usuarios
                where !usuario.Excluido
                    && usuario.Email.Equals(email)
                    && usuario.Senha.Equals(senha)
                select _mapper.Map<DTODeUsuario>(usuario)
                ).SingleOrDefaultAsync();

            return consulta;
        }

        public async Task<bool> ValidarDuplicidade(DTODeUsuario dto, bool ignorarDistincaoId)
        {
            var nomeFormatado = _conversor.ConverterTextoIniciaisParaMaiusculo(dto.Nome);
            var emailFormatado = _conversor.ConverterTextoParaMinusculo(dto.Email);

            var registroDuplicado = await (
                from usuario in _contexto.Usuarios
                where !usuario.Excluido
                    && (ignorarDistincaoId || usuario.CodigoUsuario != dto.CodigoUsuario)
                    && ((usuario.Nome.Equals(nomeFormatado) && usuario.DataNascimento == dto.DataNascimento)
                        || usuario.Email.Equals(emailFormatado))
                select 1
                ).AnyAsync();

            return registroDuplicado;
        }
    }
}
