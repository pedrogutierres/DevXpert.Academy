using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Conteudo.Domain.Cursos.Events
{
    internal sealed class CursoAtivadoEvent : Event
    {
        public Guid Id => AggregateId;

        public CursoAtivadoEvent(Guid id) : base("Curso")
        {
            AggregateId = id;
        }
    }
}
