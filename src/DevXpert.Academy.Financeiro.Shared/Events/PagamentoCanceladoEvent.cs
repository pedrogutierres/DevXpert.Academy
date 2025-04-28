using DevXpert.Academy.Core.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevXpert.Academy.Financeiro.Shared.Events
{
    public class PagamentoCanceladoEvent : Event
    {
        public PagamentoCanceladoEvent() : base("Pagamento")
        { }
    }
}
