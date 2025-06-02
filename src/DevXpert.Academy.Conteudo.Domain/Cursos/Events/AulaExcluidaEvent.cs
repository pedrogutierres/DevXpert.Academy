using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Conteudo.Domain.Cursos.Events
{
    internal sealed class AulaExcluidaEvent : Event
    {
        public Guid Id { get; private set; }

        public AulaExcluidaEvent(Guid aggregateId, Guid id) : base("Curso")
        {
            AggregateId = aggregateId;
            Id = id;
        }
    }
}
