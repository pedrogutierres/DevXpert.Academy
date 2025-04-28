using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Financeiro.Shared.Events
{
    public class PagamentoAprovadoEvent : Event
    {
        public Guid MatriculaId => AggregateId;

        public PagamentoAprovadoEvent(Guid matriculaId) : base("Pagamento")
        {
            AggregateId = matriculaId;
        }
    }
}
