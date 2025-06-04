using DevXpert.Academy.Conteudo.Domain.Cursos.Interfaces;
using DevXpert.Academy.Conteudo.Domain.Cursos.Validations;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.Messages.CommonMessages.Notifications;
using DevXpert.Academy.Core.Domain.Services;
using MediatR;
using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Conteudo.Domain.Cursos.Services
{
    public class CursoService : DomainService, ICursoService
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoService(
            IMediatorHandler mediator, 
            INotificationHandler<DomainNotification> notifications, 
            ICursoRepository cursoRepository)
            : base(cursoRepository.UnitOfWork, mediator, notifications)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<bool> Cadastrar(Curso curso)
        {
            if (!EntidadeValida(curso))
                return false;

            if (!await EntidadeAptaParaTransacionar(curso, new CursoAptoParaCadastrarValidation(_cursoRepository)))
                return false;

            await _cursoRepository.Cadastrar(curso);

            return await CommitAsync();
        }

        /// <summary>
        /// A entidade curso deve estar já adicionado ao rastreador do ef core
        /// </summary>
        public async Task<bool> Alterar(Curso curso)
        {
            if (!EntidadeValida(curso))
                return false;

            if (!await EntidadeAptaParaTransacionar(curso, new CursoAptoParaAlterarValidation(_cursoRepository)))
                return false;

            return await CommitAsync();
        }

        public async Task<bool> Ativar(Guid id)
        {
            var curso = await ObterEntidade(_cursoRepository, id, "Ativar", "Curso não encontrado", true);
            if (curso == null)
                return false;

            curso.Ativar();

            return await CommitAsync(ignoreNoChangeUpdated: true);
        }

        public async Task<bool> Inativar(Guid id)
        {
            var curso = await ObterEntidade(_cursoRepository, id, "Inativar", "Curso não encontrado", true);
            if (curso == null)
                return false;

            curso.Inativar();

            return await CommitAsync(ignoreNoChangeUpdated: true);
        }
    }
}
