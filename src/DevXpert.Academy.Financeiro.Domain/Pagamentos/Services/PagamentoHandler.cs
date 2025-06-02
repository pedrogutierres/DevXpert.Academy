using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.Exceptions;
using DevXpert.Academy.Core.Domain.Extensions;
using DevXpert.Academy.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using DevXpert.Academy.Core.Domain.Messages.CommonMessages.Notifications;
using DevXpert.Academy.Core.Domain.Services;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Commands;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Interfaces;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.ValuesObejcts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.Services
{
    // TODO: Analisar refatoração do handler separando os events e commands em ..CommandHandler e ..EventHandler
    public sealed class PagamentoHandler :
        DomainService,
        IRequestHandler<RegistrarPagamentoCommand, bool>,
        INotificationHandler<PagamentoRegistradoEvent>,
        IRequestHandler<ProcessarPagamentoCommand, bool>,
        INotificationHandler<PagamentoAprovadoEvent>,
        INotificationHandler<PagamentoRecusadoEvent>,
        INotificationHandler<PagamentoCanceladoEvent>,
        IRequestHandler<EstornarPagamentoCommand, bool>,
        INotificationHandler<PagamentoEstornadoEvent>
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPagamentoCartaoCreditoFacade _pagamentoCartaoCredito;

        public PagamentoHandler(
            IPagamentoRepository pagamentoRepository,
            IPagamentoCartaoCreditoFacade pagamentoCartaoCredito,
            IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications)
              : base(pagamentoRepository.UnitOfWork, mediator, notifications)
        {
            _pagamentoRepository = pagamentoRepository;
            _pagamentoCartaoCredito = pagamentoCartaoCredito;
        }

        public async Task<bool> Handle(RegistrarPagamentoCommand request, CancellationToken cancellationToken)
        {
            var pagamento = new Pagamento(request.AggregateId, request.MatriculaId, request.Valor, new ValuesObejcts.DadosCartao(request.DadosCartao_Nome, request.DadosCartao_Numero, request.DadosCartao_Vencimento, request.DadosCartao_CcvCvc));

            if (!EntidadeValida(pagamento))
                return false;

            await _pagamentoRepository.Cadastrar(pagamento);

            return await _uow.CommitAsync();
        }

        public async Task Handle(PagamentoRegistradoEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(new ProcessarPagamentoCommand(notification.AggregateId, notification.MatriculaId), cancellationToken);
        }

        public async Task<bool> Handle(ProcessarPagamentoCommand request, CancellationToken cancellationToken)
        {
            var pagamento = await _pagamentoRepository.ObterPorId(request.AggregateId, true) ?? throw new BusinessException("Pagamento não encontrado para processar.");

            if (pagamento.Situacao.Situacao.SwitchInline(PagamentoSituacaoEnum.Pendente, PagamentoSituacaoEnum.Recusado))
            {
                await NotificarErro("Pagamento", "Apenas pagamento pendente ou recusado anteriormente pode ser processado novamente.");
                return false;
            }

            pagamento = _pagamentoCartaoCredito.ProcessarPagamento(pagamento);

            if (pagamento.Situacao.Situacao.SwitchInline(PagamentoSituacaoEnum.Aprovado, PagamentoSituacaoEnum.Recusado))
                throw new NotImplementedException($"Situação esperada: aprovado ou recusado. Recebida: {pagamento.Situacao.Situacao}");
            
            if (await _uow.CommitAsync())
            {
                if (PagamentoSituacaoEnum.Recusado.Equals(pagamento.Situacao))
                {
                    await NotificarErro("Pagamento", "O pagamento foi recusado.");
                    return false;
                }

                return true;
            }

            return false;
        }

        public Task Handle(PagamentoAprovadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PagamentoRecusadoEvent notification, CancellationToken cancellationToken)
        {
            // TODO: notificar usuário sobre o motivo da recusa

            return Task.CompletedTask;
        }

        public Task Handle(PagamentoCanceladoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<bool> Handle(EstornarPagamentoCommand request, CancellationToken cancellationToken)
        {
            var pagamento = await _pagamentoRepository.ObterPorId(request.AggregateId, true) ?? throw new BusinessException("Pagamento não encontrado para processar.");

            if (!PagamentoSituacaoEnum.Aprovado.Equals(pagamento.Situacao))
                throw new BusinessException("Somente pagamentos aprovados podem ser estornados.");

            pagamento = _pagamentoCartaoCredito.EstornarPagamento(pagamento, request.Motivo);

            if (PagamentoSituacaoEnum.Estornado.Equals(pagamento.Situacao))
            {
                await NotificarErro("Pagamento", "O pagamento não pôde ser estornado.");
                return false;
            }

            return await _uow.CommitAsync();
        }

        public Task Handle(PagamentoEstornadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
