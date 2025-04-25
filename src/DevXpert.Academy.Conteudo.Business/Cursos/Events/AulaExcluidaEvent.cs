using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Events
{
    internal sealed class AulaExcluidaEvent : Event
    {
        public Guid Id { get; private set; }

        public AulaExcluidaEvent(Guid aggregateId, Guid id) : base("Aula")
        {
            AggregateId = aggregateId;
            Id = id;
        }
    }
}
