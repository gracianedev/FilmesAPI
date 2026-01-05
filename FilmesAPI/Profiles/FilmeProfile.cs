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

            CreateMap<LeituraFilmeDTO, Filme>();

            CreateMap<AtualizacaoFilmeDTO, Filme>();


            CreateMap<Genero, LeituraGeneroDTO>();

            CreateMap<Filme, LeituraFilmeDTO>()
                .ForMember(filmeDto => filmeDto.Genero,
                    opt => opt.MapFrom(filme => filme.FilmesGenero.Select(fg => fg.Genero)));

             CreateMap<Filme, AtualizacaoFilmeDTO>()
                .ForMember(filmeDto => filmeDto.GeneroIds,
                    opt => opt.MapFrom(filme => filme.FilmesGenero.Select(fg => fg.Genero)));

        }
    }
}