using DevXpert.Academy.Conteudo.Business.Cursos.Validations;
using DevXpert.Academy.Conteudo.Business.Cursos.ValuesObjects;
using DevXpert.Academy.Core.Domain.DomainObjects;
using System;
using System.Collections.Generic;

namespace DevXpert.Academy.Conteudo.Business.Cursos
{
    public class Curso : Entity<Curso>, IAggregateRoot
    {
        public string Titulo { get; private set; }
        public List<Aula> Aulas { get; private set; }
        public ConteudoProgramatico ConteudoProgramatico { get; private set; }

        public Curso(Guid id, string titulo, List<Aula> aulas, ConteudoProgramatico conteudoProgramatico)
        {
            Id = id;
            Titulo = titulo;
            Aulas = aulas;
            ConteudoProgramatico = conteudoProgramatico;
        }

        public void AdicionarAula(Aula aula)
        {
            Aulas ??= [];
            Aulas.Add(aula);
        }

        public void RemoverAula(Aula aula)
        {
            Aulas?.Remove(aula);
        }

        public override bool EhValido()
        {
            ValidationResult = new CursoEstaConsistenteValidation().Validate(this);

            foreach (var aula in Aulas)
            {
                if (!aula.EhValido())
                    AdicionarValidationResultErros(aula.ValidationResult);
            }

            return ValidationResult.IsValid;
        }
    }
}
