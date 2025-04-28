using System;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.ValuesObejcts
{
    public sealed record PagamentoSituacao(PagamentoSituacaoEnum situacao, DateTime dataHoraProcessamento, string mensagem);

    public enum PagamentoSituacaoEnum
    {
        Pendente,
        Aprovado,
        Recusado,
        Cancelado
    }
}
