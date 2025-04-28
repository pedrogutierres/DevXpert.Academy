using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.Messages.Notifications;
using DevXpert.Academy.Core.Domain.Services;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.Services
{
    public sealed class PagamentoService : DomainService, IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoService(IPagamentoRepository pagamentoRepository, IMediatorHandler mediator, INotificationHandler<DomainNotification> notifications) 
            : base(pagamentoRepository.UnitOfWork, mediator, notifications)
        {
            _pagamentoRepository = pagamentoRepository;
        }

        public Task RegistrarPagamento(Pagamento pagamento)
        {
            throw new NotImplementedException();
        }

        public Task ProcessarPagamento(Guid id)
        {
            // TODO: comunicar com gateway de pagamento via anticorruption layer

            throw new NotImplementedException();
        }

        public Task CancelarPagamento(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task EstornarPagamento(Guid id)
        {
            // TODO: comunicar com gateway de pagamento via anticorruption layer

            throw new NotImplementedException();
        }
    }
}
