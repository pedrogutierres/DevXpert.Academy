using DevXpert.Academy.Conteudo.Business.Cursos.Interfaces;
using DevXpert.Academy.Conteudo.Business.Cursos.Validations;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.Messages.Notifications;
using DevXpert.Academy.Core.Domain.Services;
using MediatR;
using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Services
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

        public Task<bool> Alterar(Curso curso)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Ativar(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Inativar(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
