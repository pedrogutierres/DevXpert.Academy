using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.Commands
{
    public class EstornarPagamentoCommand : Command<bool>
    {
        public EstornarPagamentoCommand(Guid id)
        {
            AggregateId = id;
        }
    }
}
