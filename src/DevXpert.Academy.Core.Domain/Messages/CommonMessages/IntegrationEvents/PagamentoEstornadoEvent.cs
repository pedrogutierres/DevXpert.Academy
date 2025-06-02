using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Core.Domain.Messages.CommonMessages.IntegrationEvents
{
    public class PagamentoEstornadoEvent : Event
    {
        public Guid MatriculaId => AggregateId;

        public PagamentoEstornadoEvent(Guid matriculaId) : base("Pagamento")
        {
            AggregateId = matriculaId;
        }
    }
}
