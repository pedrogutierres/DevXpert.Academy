using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Financeiro.Shared.Events
{
    public class PagamentoRegistradoEvent : Event
    {
        public PagamentoRegistradoEvent(Guid pagamentoId) : base("Pagamento")
        {
            AggregateId = pagamentoId;
        }
    }
}
