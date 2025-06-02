using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Conteudo.Domain.Cursos.Events
{
    internal sealed class CursoTituloAlteradoEvent : Event
    {
        public Guid Id => AggregateId;
        public string Titulo { get; private set; }

        public CursoTituloAlteradoEvent(Guid id, string titulo) : base("Curso")
        {
            AggregateId = id;
            Titulo = titulo;
        }
    }
}
