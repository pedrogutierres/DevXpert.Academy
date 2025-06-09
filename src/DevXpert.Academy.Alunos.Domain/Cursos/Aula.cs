using DevXpert.Academy.Core.Domain.DomainObjects;

namespace DevXpert.Academy.Alunos.Domain.Cursos
{
    public class Aula : ReadOnlyEntity
    {
        public virtual Curso Curso { get; private set; }

        private Aula() { }
    }
}
