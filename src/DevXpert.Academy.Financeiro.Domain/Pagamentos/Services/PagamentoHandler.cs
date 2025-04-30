using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Interfaces;
using DevXpert.Academy.Financeiro.Shared.Commands;
using DevXpert.Academy.Financeiro.Shared.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.Services
{
    // TODO: Analisar refatoração do handler separando os events e commands em ..CommandHandler e ..EventHandler
    public sealed class PagamentoHandler :
        IRequestHandler<RegistrarPagamentoCommand, bool>,
        INotificationHandler<PagamentoRegistradoEvent>,
        IRequestHandler<ProcessarPagamentoCommand, bool>,
        INotificationHandler<PagamentoAprovadoEvent>,
        INotificationHandler<PagamentoRecusadoEvent>,
        INotificationHandler<PagamentoCanceladoEvent>,
        IRequestHandler<EstornarPagamentoCommand, bool>,
        INotificationHandler<PagamentoEstornadoEvent>
    {
        private readonly IPagamentoService _pagamentoService;
        private readonly IMediatorHandler _mediator;

        public PagamentoHandler(IPagamentoService pagamentoService, IMediatorHandler mediator)
        {
            _pagamentoService = pagamentoService;
            _mediator = mediator;
        }

        public async Task<bool> Handle(RegistrarPagamentoCommand request, CancellationToken cancellationToken)
        {
            var pagamento = new Pagamento(request.AggregateId, request.MatriculaId, request.Valor, new ValuesObejcts.DadosCartao(request.DadosCartao_Nome, request.DadosCartao_Numero, request.DadosCartao_Vencimento, request.DadosCartao_CcvCvc));

            await _pagamentoService.RegistrarPagamento(pagamento);

            return true;
        }

        public async Task Handle(PagamentoRegistradoEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(new ProcessarPagamentoCommand(notification.AggregateId), cancellationToken);
        }

        public async Task<bool> Handle(ProcessarPagamentoCommand request, CancellationToken cancellationToken)
        {
            await _pagamentoService.ProcessarPagamento(request.AggregateId);

            return true;
        }

        public Task Handle(PagamentoAprovadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PagamentoRecusadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(PagamentoCanceladoEvent notification, CancellationToken cancellationToken)
        {
            await _pagamentoService.CancelarPagamento(notification.AggregateId);
        }

        public async Task<bool> Handle(EstornarPagamentoCommand request, CancellationToken cancellationToken)
        {
            await _pagamentoService.EstornarPagamento(request.AggregateId);

            return true;
        }

        public Task Handle(PagamentoEstornadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
