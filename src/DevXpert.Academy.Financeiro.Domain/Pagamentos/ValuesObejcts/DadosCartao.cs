using System;
using System.Text;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.ValuesObejcts
{
    public sealed record DadosCartao
    {
        public string Token { get; private set; }

        private DadosCartao() { }
        public DadosCartao(string Nome, string Numero, string Vencimento, string CcvCvc)
        {
            Token = TokenizarDadosCartao(Nome, Numero, Vencimento, CcvCvc);
        }

        private static string TokenizarDadosCartao(string Nome, string Numero, string Vencimento, string CcvCvc) // TODO: utilizar uma facade para tal
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Nome}|{Numero}|{Vencimento}|{CcvCvc}"));
        }
    }
}
