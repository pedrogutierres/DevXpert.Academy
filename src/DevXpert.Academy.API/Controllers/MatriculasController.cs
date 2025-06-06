using AutoMapper;
using DevXpert.Academy.Alunos.Domain.Alunos.Interfaces;
using DevXpert.Academy.Alunos.Domain.Cursos.Interfaces;
using DevXpert.Academy.API.ViewModels.Matriculas;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.DomainObjects;
using DevXpert.Academy.Core.Domain.Exceptions;
using DevXpert.Academy.Core.Domain.Messages.CommonMessages.Notifications;
using DevXpert.Academy.Financeiro.Domain.Pagamentos.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevXpert.Academy.API.Controllers
{
    [Authorize]
    [Route("api/matriculas")]
    public class MatriculasController : MainController
    {
        private readonly IAlunoService _alunoService;
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMapper _mapper;

        public MatriculasController(
            IAlunoService alunoService,
            IAlunoRepository alunoRepository,
            IMapper mapper,
            INotificationHandler<DomainNotification> notifications,
            IUser user,
            IMediatorHandler mediator)
            : base(notifications, user, mediator)
        {
            _alunoService = alunoService;
            _alunoRepository = alunoRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Aluno")]
        [HttpPost("cursos/{cursoId:guid}/se-matricular")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AlunoSeMatricular([FromRoute] Guid cursoId, [FromBody] RealizarPagamentoViewModel pagamento, [FromServices] ICursoReadOnlyRepository cursoRepository)
        {
            var curso = await cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                return BadRequest("Curso não encontrado.");

            var matricula = await _alunoService.Matricular(_user.UsuarioId, cursoId);
            if (matricula == null)
                return BadRequest();

            if (!matricula.Ativa)
            {
                await _mediator.SendCommand(new RegistrarPagamentoCommand(Guid.NewGuid(), matricula.Id, curso.Valor, pagamento.DadosCartao_Nome, pagamento.DadosCartao_Numero, pagamento.DadosCartao_Vencimento, pagamento.DadosCartao_CcvCvc));
            }

            return Response(matricula.Id);
        }

        [Authorize(Roles = "Aluno")]
        [HttpPost("{matriculaId:guid}/realizar-pagamento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RealizarPagamentoMatricula([FromRoute] Guid matriculaId, RealizarPagamentoViewModel pagamento, [FromServices] ICursoReadOnlyRepository cursoRepository)
        {
            var matricula = await _alunoRepository.ObterMatricula(matriculaId) ?? throw new BusinessException("Matrícula não encontrada.");
            if (matricula.Ativa)
                return BadRequest("Matrícula já está ativa, não é necessário realizar um novo pagamento.");

            var pagamentoCommand = new RegistrarPagamentoCommand(Guid.NewGuid(), matriculaId, matricula.Curso.Valor, pagamento.DadosCartao_Nome, pagamento.DadosCartao_Numero, pagamento.DadosCartao_Vencimento, pagamento.DadosCartao_CcvCvc);

            await _mediator.SendCommand(pagamentoCommand);

            return Response(pagamentoCommand.AggregateId);
        }

        [Authorize]
        [HttpDelete("{matriculaId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelarMatricula([FromRoute] Guid matriculaId, [FromBody] CancelarMatriculaViewModel viewModel)
        {
            var aluno = await _alunoRepository.ObterAtravesDaMatricula(matriculaId, true) ?? throw new BusinessException("Aluno vinculado a matrícula não encontrado.");

            if (!_user.EhUmAdministrador() && aluno.Id != _user.UsuarioId)
                return BadRequest("Você não tem permissão para cancelar esta matrícula.");

            await _mediator.SendCommand(new SolicitarEstornoPagamentoDaMatriculaCommand(matriculaId, viewModel.Motivo));

            return Response(matriculaId);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("cursos/{cursoId:guid}/matricular-aluno/{alunoId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AdministradorMatricularAluno([FromRoute] Guid cursoId, [FromRoute] Guid alunoId, [FromServices] ICursoReadOnlyRepository cursoRepository)
        {
            var curso = await cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                return BadRequest("Curso não encontrado.");

            var matricula = await _alunoService.Matricular(alunoId, cursoId);
            if (matricula == null)
                return BadRequest();

            await _alunoService.AprovarMatricula(matricula.Id);

            return Response(matricula);
        }


    }
}
