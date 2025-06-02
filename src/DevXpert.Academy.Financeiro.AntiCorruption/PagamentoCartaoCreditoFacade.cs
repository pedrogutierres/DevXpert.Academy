
using DevXpert.Academy.Financeiro.Domain.Pagamentos;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Interfaces;

namespace DevXpert.Academy.Financeiro.AntiCorruption
{
    public class PagamentoCartaoCreditoFacade : IPagamentoCartaoCreditoFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configManager;

        public PagamentoCartaoCreditoFacade(IPayPalGateway payPalGateway, IConfigurationManager configManager)
        {
            _payPalGateway = payPalGateway;
            _configManager = configManager;
        }

        public Pagamento ProcessarPagamento(Pagamento pagamento)
        {
            var apiKey = _configManager.GetValue("apiKey");
            var encriptionKey = _configManager.GetValue("encriptionKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, pagamento.DadosCartao.Token);

            var pagamentoResult = _payPalGateway.CommitTransaction(cardHashKey, pagamento.Id.ToString(), pagamento.Valor);

            if (pagamentoResult)
                pagamento.AprovarPagamento();
            else
                pagamento.RecusarPagamento("Pagamento recusado pelo gateway PayPal");

            return pagamento;
        }

        public Pagamento EstornarPagamento(Pagamento pagamento, string motivo)
        {
            var apiKey = _configManager.GetValue("apiKey");
            var encriptionKey = _configManager.GetValue("encriptionKey");

            if (_payPalGateway.RollbackTransaction(pagamento.Id.ToString()))
                pagamento.EstornarPagamento(motivo ?? "Pagamento estornado");

            return pagamento;
        }
    }
}