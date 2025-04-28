using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Financeiro.Shared.Events
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
