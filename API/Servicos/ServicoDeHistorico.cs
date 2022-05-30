using API.Consultas.IConsultas;
using API.EntitieModels;
using API.Ferramentas;
using API.Repositorios.IRepositorios;
using API.Servicos.IServicos;
using API.ViewModels.DTOs;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace API.Servicos
{
    public class ServicoDeHistorico : IServicoDeHistorico
    {
        #region Dependências

        private readonly IMapper _mapper;
        private readonly IConsultaDeFilme _consultaDeFilme;
        private readonly IConsultaDeHistorico _consultaDeHistorico;
        private readonly IConsultaDeUsuario _consultaDeUsuario;
        private readonly IRepositorioDeHistorico _repositorioDeHistorico;

        #endregion

        public ServicoDeHistorico(
            IMapper mapper,
            IConsultaDeFilme consultaDeFilme,
            IConsultaDeHistorico consultaDeHistorico,
            IConsultaDeUsuario consultaDeUsuario,
            IRepositorioDeHistorico repositorioDeHistorico)
        {
            _mapper = mapper;
            _consultaDeFilme = consultaDeFilme;
            _consultaDeHistorico = consultaDeHistorico;
            _consultaDeUsuario = consultaDeUsuario;
            _repositorioDeHistorico = repositorioDeHistorico;
        }

        public async Task<DTODeHistorico> Incluir(DTODeHistorico dto)
        {
            var preenchimentoIncorreto = await ValidarPreenchimentoIncorreto(dto);

            if (preenchimentoIncorreto)
            {
                return null;
            }

            var historico = _mapper.Map<Historico>(dto);
            historico.DataReproducao = DateTimeOffset.UtcNow;

            try
            {
                historico = await _repositorioDeHistorico.Incluir(historico);

                if (historico.CodigoHistorico == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return null;
            }

            var dtoHistoricoIncluido = await _consultaDeHistorico.ObterPorId(historico.CodigoHistorico);
            return dtoHistoricoIncluido;
        }

        private async Task<bool> ValidarPreenchimentoIncorreto(DTODeHistorico dto)
        {
            var filme = await _consultaDeFilme.ObterPorId(dto.CodigoFilme);
            var usuario = await _consultaDeUsuario.ObterPorId(dto.CodigoUsuario);

            if (filme == null
                || usuario == null
                || dto.Nota < 0 
                || dto.Nota > 10)
            {
                return true;
            }

            return false;
        }
    }
}
