using DevXpert.Academy.Alunos.Domain.Alunos.Interfaces;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.DomainObjects;
using DevXpert.Academy.Core.Domain.Exceptions;
using DevXpert.Academy.Core.Domain.Messages.Notifications;
using DevXpert.Academy.Core.Domain.Services;
using MediatR;
using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Alunos.Domain.Alunos.Services
{
    public class AlunoService : DomainService, IAlunoService
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

        public async Task<bool> Matricular(Aluno aluno, Guid cursoId)
        {
            if (!_user.EhUmAdministrador())
                throw new BusinessException("Você precisa estar logado como administrador para matricular o aluno em um curso.");

            if (!aluno.Ativo)
                throw new BusinessException("Aluno deve estar com cadastro ativo para se matricular em um curso.");

            if (aluno.EstaMatriculado(cursoId))
                throw new BusinessException("O aluno já está matriculado neste curso.");

            aluno.Matricular(new Matricula(Guid.NewGuid(), cursoId, aluno.Id));

            if (!EntidadeValida(aluno))
                return false;

            return await CommitAsync();
        }

        public async Task<bool> SeMatricular(Guid cursoId)
        {
            if (!_user.Autenticado())
                throw new BusinessException("Você precisa estar logado para se matricular em um curso.");

            var aluno = await _alunoRepository.ObterPorId(_user.UsuarioId, true) ?? throw new BusinessException("Aluno não encontrado.");

            if (aluno.EstaMatriculado(cursoId))
                throw new BusinessException("Você já está matriculado neste curso.");

            aluno.Matricular(new Matricula(Guid.NewGuid(), cursoId, aluno.Id));

            if (!EntidadeValida(aluno))
                return false;

            return await CommitAsync();
        }
    }
}
