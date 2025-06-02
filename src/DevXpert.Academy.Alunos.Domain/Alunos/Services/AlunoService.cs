using DevXpert.Academy.Alunos.Domain.Alunos.Interfaces;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.DomainObjects;
using DevXpert.Academy.Core.Domain.Exceptions;
using DevXpert.Academy.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using DevXpert.Academy.Core.Domain.Messages.CommonMessages.Notifications;
using DevXpert.Academy.Core.Domain.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevXpert.Academy.Alunos.Domain.Alunos.Services
{
    public class AlunoService : DomainService, IAlunoService,
        INotificationHandler<PagamentoAprovadoEvent>,
        INotificationHandler<PagamentoEstornadoEvent>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IUser _user;

        public AlunoService(
            IAlunoRepository alunoRepository,
            IUser user,
            IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications)
            : base(alunoRepository.UnitOfWork, mediator, notifications)
        {
            _alunoRepository = alunoRepository;
            _user = user;
        }

        public Task<bool> Cadastrar(Aluno aluno)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid?> Matricular(Aluno aluno, Guid cursoId)
        {
            if (!_user.EhUmAdministrador())
                throw new BusinessException("Você precisa estar logado como administrador para matricular o aluno em um curso.");

            if (aluno.EstaMatriculado(cursoId))
                throw new BusinessException("O aluno já está matriculado neste curso.");

            var matricula = new Matricula(Guid.NewGuid(), aluno.Id, cursoId);

            aluno.Matricular(matricula);

            if (!EntidadeValida(aluno))
                return null;

            if (await CommitAsync())
                return matricula.Id;

            return null;
        }

        public async Task<Guid?> SeMatricular(Guid cursoId)
        {
            if (!_user.Autenticado())
                throw new BusinessException("Você precisa estar logado para se matricular em um curso.");

            var aluno = await _alunoRepository.ObterPorId(_user.UsuarioId, true) ?? throw new BusinessException("Aluno não encontrado.");

            if (aluno.EstaMatriculado(cursoId))
                throw new BusinessException("Você já está matriculado neste curso.");

            var matricula = new Matricula(Guid.NewGuid(), aluno.Id, cursoId);

            aluno.Matricular(matricula);

            if (!EntidadeValida(aluno))
                return null;

            if (await CommitAsync())
                return matricula.Id;

            return null;
        }

        public async Task Handle(PagamentoAprovadoEvent notification, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterAtravesDaMatricula(notification.MatriculaId) ?? throw new BusinessException("Aluno não encontrado.");

            var matricula = aluno.Matriculas.FirstOrDefault(p => p.Id == notification.MatriculaId) ?? throw new BusinessException("Matrícula não encontrada.");

            if (matricula.Liberada)
                return;

            matricula.Liberar();

            if (!EntidadeValida(aluno))
                return;

            await CommitAsync();
        }

        public Task Handle(PagamentoEstornadoEvent notification, CancellationToken cancellationToken) => BloquearMatricula(notification.MatriculaId);
        public Task Handle(PagamentoCanceladoEvent notification, CancellationToken cancellationToken) => BloquearMatricula(notification.MatriculaId);

        private async Task<bool> BloquearMatricula(Guid matriculaId)
        {
            var aluno = await _alunoRepository.ObterAtravesDaMatricula(matriculaId) ?? throw new BusinessException("Aluno não encontrado.");

            var matricula = aluno.Matriculas.FirstOrDefault(p => p.Id == matriculaId) ?? throw new BusinessException("Matrícula não encontrada.");

            if (!matricula.Liberada)
                return false;

            matricula.Bloquear();

            if (!EntidadeValida(aluno))
                return false;

            return await CommitAsync();
        }
    }
}
