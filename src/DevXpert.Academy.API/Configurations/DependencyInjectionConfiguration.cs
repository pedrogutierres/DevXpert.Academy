using DevXpert.Academy.Alunos.Data.Repositories;
using DevXpert.Academy.Alunos.Domain.Alunos.Interfaces;
using DevXpert.Academy.Alunos.Domain.Alunos.Services;
using DevXpert.Academy.Alunos.Domain.Cursos.Interfaces;
using DevXpert.Academy.API.Models;
using DevXpert.Academy.Conteudo.Domain.Cursos.Interfaces;
using DevXpert.Academy.Conteudo.Domain.Cursos.Services;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.DomainObjects;
using DevXpert.Academy.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using DevXpert.Academy.Core.Domain.Messages.CommonMessages.Notifications;
using DevXpert.Academy.Core.EventSourcing.EventStore.Context;
using DevXpert.Academy.Core.EventSourcing.EventStore.EventSourcing;
using DevXpert.Academy.Core.EventSourcing.EventStore.Repository;
using DevXpert.Academy.Financeiro.Data.Repositories;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Commands;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Interfaces;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DevXpert.Academy.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDIConfiguration(this IServiceCollection services)
        {
            // ASPNET
            services.AddHttpContextAccessor();

            // JWT
            //services.AddScoped<JwtTokenGenerate>();
            //services.AddScoped<AutenticacaoService>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventSourcingRepository, EventSourcingRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSQLContext>();

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();

            // DI / IoC - Alunos
            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IAlunoService, AlunoService>();
            services.AddScoped<ICursoReadOnlyRepository, CursoRepository>();

            // DI / IoC - Conteudo
            services.AddScoped<ICursoRepository, Conteudo.Data.Repositories.CursoRepository>();
            services.AddScoped<ICursoService, CursoService>();

            // DI / IoC - Financeiro
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoService, PagamentoService>();

            // Handlers
            services.AddScoped<IRequestHandler<RegistrarPagamentoCommand, bool>, PagamentoHandler>();
            services.AddScoped<IRequestHandler<ProcessarPagamentoCommand, bool>, PagamentoHandler>();
            services.AddScoped<IRequestHandler<EstornarPagamentoCommand, bool>, PagamentoHandler>();

            // Events
            services.AddScoped<INotificationHandler<PagamentoRegistradoEvent>, PagamentoHandler>();
            services.AddScoped<INotificationHandler<PagamentoAprovadoEvent>, PagamentoHandler>();
            services.AddScoped<INotificationHandler<PagamentoAprovadoEvent>, AlunoService>();
            services.AddScoped<INotificationHandler<PagamentoRecusadoEvent>, PagamentoHandler>();
            services.AddScoped<INotificationHandler<PagamentoEstornadoEvent>, PagamentoHandler>();
            services.AddScoped<INotificationHandler<PagamentoEstornadoEvent>, AlunoService>();
            services.AddScoped<INotificationHandler<PagamentoCanceladoEvent>, PagamentoHandler>();
        }
    }
}
