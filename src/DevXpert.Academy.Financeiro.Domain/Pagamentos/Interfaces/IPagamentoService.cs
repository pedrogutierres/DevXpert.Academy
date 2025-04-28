using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.Interfaces
{
    public interface IPagamentoService
    {
        Task RegistrarPagamento(Pagamento pagamento);
        Task ProcessarPagamento(Guid id);
        Task EstornarPagamento(Guid id);
        Task CancelarPagamento(Guid id);
    }
}
