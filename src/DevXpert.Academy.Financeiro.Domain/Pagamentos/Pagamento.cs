using DevXpert.Academy.Core.Domain.DomainObjects;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.ValuesObejcts;
using System;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos
{
    public class Pagamento : Entity<Pagamento>
    {
        public Guid MatriculaId { get; private set; }
        public decimal Valor { get; private set; }
        public DadosCartao DadosCartao { get; private set; }
        public PagamentoSituacao Situacao { get; private set; }

        public override bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
