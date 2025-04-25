using DevXpert.Academy.Core.Domain.DomainObjects;

namespace DevXpert.Academy.Alunos.Domain.Cursos
{
    public sealed class Curso : ReadOnlyEntity<Curso>, IAggregateRoot
    {
        public string Titulo { get; private set; }
    }
}
