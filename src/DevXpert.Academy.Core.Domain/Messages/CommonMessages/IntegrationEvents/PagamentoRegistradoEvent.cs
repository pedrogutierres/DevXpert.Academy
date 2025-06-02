using System;

namespace DevXpert.Academy.Core.Domain.Messages.CommonMessages.IntegrationEvents
{
    public class PagamentoRegistradoEvent : Event
    {
        public PagamentoRegistradoEvent(Guid pagamentoId) : base("Pagamento")
        {
            AggregateId = pagamentoId;
        }
    }
}
