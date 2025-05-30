﻿using AutoMapper;
using DevXpert.Academy.API.ViewModels.Cursos;
using DevXpert.Academy.Conteudo.Business.Cursos;
using DevXpert.Academy.Conteudo.Business.Cursos.ValuesObjects;

namespace DevXpert.Academy.API.Mappers
{
    public class ConteudoDomainToViewModelMappingProfile : Profile
    {
        public ConteudoDomainToViewModelMappingProfile()
        {
            CreateMap<Curso, CursoViewModel>();
            CreateMap<ConteudoProgramatico, ConteudoProgramaticoViewModel>();
            CreateMap<Aula, AulaViewModel>();
        }
    }
}

