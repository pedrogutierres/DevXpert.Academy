using DevXpert.Academy.Alunos.Domain.Alunos.Validations;
using DevXpert.Academy.Core.Domain.DomainObjects;
using DevXpert.Academy.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevXpert.Academy.Alunos.Domain.Alunos
{
    public sealed class Aluno : Entity<Aluno>, IAggregateRoot
    {
        public string Nome { get; private set; }

        public List<Matricula> Matriculas { get; private set; }

        private Aluno() { }
        public Aluno(Guid id, string nome)
        {
            Id = id;
            Nome = nome;

            // TODO: event
        }

        public void Matricular(Matricula matricula)
        {
            if (EstaMatriculado(matricula.CursoId))
                throw new BusinessException("Você já está matriculado neste curso.");

            Matriculas ??= [];
            Matriculas.Add(matricula);

            // TODO: event
        }
        public bool EstaMatriculado(Guid cursoId) => Matriculas?.Any(p => p.CursoId == cursoId) ?? false;

        public override bool EhValido()
        {
            ValidationResult = new AlunoEstaConsistenteValidation().Validate(this);

            // TODO: validar matriculas

            return ValidationResult.IsValid;
        }
    }
}
