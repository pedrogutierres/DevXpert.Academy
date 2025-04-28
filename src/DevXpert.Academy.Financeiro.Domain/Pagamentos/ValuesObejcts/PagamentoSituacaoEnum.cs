namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.ValuesObejcts
{
    public sealed record PagamentoSituacao(PagamentoSituacaoEnum situacao, string mensagem);

    public enum PagamentoSituacaoEnum
    {
        Pendente,
        Aprovado,
        Recusado,
        Cancelado
    }
}
