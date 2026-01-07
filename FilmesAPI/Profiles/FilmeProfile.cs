using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FilmesAPI.Models;
using FilmesAPI.DTO;

namespace FilmesAPI.Profiles
{
    public class FilmeProfile : Profile
    {
        public FilmeProfile()
        {
            CreateMap<CriacaoFilmeDTO, Filme>();

            CreateMap<Filme, LeituraFilmeDTO>();

            CreateMap<AtualizacaoFilmeDTO, Filme>();

            CreateMap<CadastroNovoAtorDTO, Ator>();

            CreateMap<Ator, LeituraAtorDTO>();

            CreateMap<Genero, LeituraGeneroDTO>();

            CreateMap<CadastroNovoGeneroDTO, Genero>();

            CreateMap<Filme, LeituraFilmeDTO>()
                .ForMember(filmeDto => filmeDto.Genero,
                    opt => opt.MapFrom(filme => filme.FilmesGenero.Select(fg => fg.Genero)))
                .ForMember(filmeDto => filmeDto.Atores,
                    opt => opt.MapFrom(filme => filme.ElencoFilme.Select(ef=> ef.Ator)));

             CreateMap<Filme, AtualizacaoFilmeDTO>()
                .ForMember(filmeDto => filmeDto.GeneroIds,
                    opt => opt.MapFrom(filme => filme.FilmesGenero.Select(fg => fg.Genero)));

        }
    }
}