using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.Exceptions;
using DevXpert.Academy.Core.Domain.Messages.Notifications;
using DevXpert.Academy.Core.Domain.Services;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Interfaces;
using DevXpert.Academy.Financeiro.Shared.Events;
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

        public async Task RegistrarPagamento(Pagamento pagamento)
        {
            if (!EntidadeValida(pagamento))
                return;

            await _pagamentoRepository.Cadastrar(pagamento);

            await _uow.CommitAsync();
        }

        public async Task ProcessarPagamento(Guid id)
        {
            var pagamento = await _pagamentoRepository.ObterPorId(id, true);
            if (pagamento == null)
                throw new BusinessException("Pagamento não encontrado para processar.");

            // TODO: comunicar com gateway de pagamento via anticorruption layer

            await _mediator.RaiseEvent(new PagamentoAprovadoEvent(id, pagamento.MatriculaId));
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
