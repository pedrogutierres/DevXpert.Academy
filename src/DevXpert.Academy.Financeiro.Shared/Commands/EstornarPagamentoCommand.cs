using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Financeiro.Shared.Commands
{
    public class EstornarPagamentoCommand : Command<bool>
    {
        public EstornarPagamentoCommand(Guid id)
        {
            AggregateId = id;
        }
    }
}
