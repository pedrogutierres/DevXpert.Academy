using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Events
{
    internal sealed class CursoTituloAlteradoEvent : Event
    {
        public Guid Id => AggregateId;
        public string Titulo { get; private set; }

        public CursoTituloAlteradoEvent(Guid id, string titulo) : base("Aula")
        {
            AggregateId = id;
            Titulo = titulo;
        }
    }
}
