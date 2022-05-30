using API.EntitieModels;
using API.Enumeradores;
using API.Ferramentas;
using API.ViewModels.DTOs;
using AutoMapper;

namespace API.AutoMapper
{
    public class MapeadorDeEntidade : Profile
    {
        #region Dependências

        private readonly IConversor _conversor;

        #endregion

        public MapeadorDeEntidade(IConversor conversor)
        {
            _conversor = conversor;

            #region Ator

            CreateMap<Ator, DTODeAtor>();

            CreateMap<DTODeAtor, Ator>()
                .ForMember(dest => dest.Nome, opt => 
                    opt.MapFrom(src => _conversor.ConverterTextoIniciaisParaMaiusculo(src.Nome)));

            CreateMap<Ator, ProjecaoDeAtor>()
                .ForMember(dest => dest.DataNascimento, opt => 
                    opt.MapFrom(src => _conversor.ConverterDateTimeParaDataString(src.DataNascimento)));

            #endregion

            #region Filme

            CreateMap<Filme, DTODeFilme>();

            CreateMap<DTODeFilme, Filme>()
                .ForMember(dest => dest.Titulo, opt =>
                    opt.MapFrom(src => _conversor.ConverterTextoIniciaisParaMaiusculo(src.Titulo)));

            CreateMap<Filme, ProjecaoDeFilme>();

            #endregion

            #region FilmeAtor

            CreateMap<FilmeAtor, DTODeFilmeAtor>().ReverseMap();
            CreateMap<FilmeAtor, ProjecaoDeFilmeAtor>()
                .ForMember(dest => dest.TituloFilme, opt => opt.MapFrom(src => src.Filme.Titulo))
                .ForMember(dest => dest.NomeAtor, opt => opt.MapFrom(src => src.Ator.Nome));

            #endregion

            #region Usuario

            CreateMap<Usuario, DTODeUsuario>()
                .ForMember(dest => dest.Senha, opt => opt.Ignore());

            CreateMap<DTODeUsuario, Usuario>()
                .ForMember(dest => dest.Nome, opt =>
                    opt.MapFrom(src => _conversor.ConverterTextoIniciaisParaMaiusculo(src.Nome)));

            CreateMap<Usuario, ProjecaoDeUsuario>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => _conversor.ConverterEnumeradorParaString(typeof(Role), src.Role)));

            #endregion

            #region Historico

            CreateMap<Historico, DTODeHistorico>().ReverseMap();
            CreateMap<Historico, ProjecaoDeHistorico>()
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.Nome))
                .ForMember(dest => dest.TituloFilme, opt => opt.MapFrom(src => src.Filme.Titulo))
                .ForMember(dest => dest.DataReproducao, opt =>
                    opt.MapFrom(src => _conversor.ConverterDateTimeOffsetParaDataHoraString(src.DataReproducao)));

            #endregion
        }
    }
}
