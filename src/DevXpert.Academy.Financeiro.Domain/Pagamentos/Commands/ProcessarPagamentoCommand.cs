using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.Commands
{
    public class ProcessarPagamentoCommand : Command<bool>
    {
        public ProcessarPagamentoCommand(Guid id)
        {
            AggregateId = id;
        }
    }
}
