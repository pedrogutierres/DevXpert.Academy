using DevXpert.Academy.Conteudo.Business.Cursos.Validations;
using DevXpert.Academy.Core.Domain.DomainObjects;
using System;

namespace DevXpert.Academy.Conteudo.Business.Cursos
{
    public sealed class Aula : Entity<Aula>
    {
        public string Titulo { get; private set; }
        public string VideoUrl { get; private set; }

        public Aula(Guid id, string titulo, string videoUrl)
        {
            Id = id;
            Titulo = titulo;
            VideoUrl = videoUrl;
        }

        public override bool EhValido()
        {
            ValidationResult = new AulaEstaConsistenteValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
