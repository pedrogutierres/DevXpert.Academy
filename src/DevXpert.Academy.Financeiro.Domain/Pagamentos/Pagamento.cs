using DevXpert.Academy.Core.Domain.DomainObjects;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.ValuesObejcts;
using System;
using System.Collections.Generic;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos
{
    public class Pagamento : Entity<Pagamento>, IAggregateRoot
    {
        public Guid MatriculaId { get; private set; }
        public decimal Valor { get; private set; }
        public DadosCartao DadosCartao { get; private set; }
        public PagamentoSituacao Situacao { get; private set; }

        public List<PagamentoSituacao> HistoricoTransacoes { get; private set; }

        private Pagamento() { }
        public Pagamento(Guid id, Guid matriculaId, decimal valor, DadosCartao dadosCartao)
        {
            Id = id;
            MatriculaId = matriculaId;
            Valor = valor;
            DadosCartao = dadosCartao;
            Situacao = new PagamentoSituacao(PagamentoSituacaoEnum.Pendente, DateTime.Now, "Pagamento pendente");
            HistoricoTransacoes = [Situacao];
        }

        public override bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
