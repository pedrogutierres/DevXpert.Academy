using AutoMapper;
using DevXpert.Academy.Alunos.Domain.Alunos.Interfaces;
using DevXpert.Academy.Alunos.Domain.Cursos.Interfaces;
using DevXpert.Academy.API.ViewModels.Matriculas;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.DomainObjects;
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

            var matriculaId = await _alunoService.SeMatricular(cursoId);
            if (!matriculaId.HasValue)
                return BadRequest();

            await _mediator.SendCommand(new RegistrarPagamentoCommand(Guid.NewGuid(), matriculaId.Value, curso.Valor, pagamento.DadosCartao_Nome, pagamento.DadosCartao_Numero, pagamento.DadosCartao_Vencimento, pagamento.DadosCartao_CcvCvc));

            return Response(matriculaId);
        }

        [Authorize(Roles = "Aluno")]
        [HttpPost("cursos/{cursoId:guid}/matricula/{matriculaId:guid}/realizar-pagamento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RealizarPagamentoMatricula([FromRoute] Guid cursoId, [FromRoute] Guid matriculaId, RealizarPagamentoViewModel pagamento, [FromServices] ICursoReadOnlyRepository cursoRepository)
        {
            var curso = await cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                return BadRequest("Curso não encontrado.");

            var aluno = await _alunoRepository.ObterPorId(_user.UsuarioId);
            if (aluno == null)
                return BadRequest("Seu cadastro de aluno não foi encontrado.");

            var matricula = aluno.Matriculas?.FirstOrDefault(p => p.Id == matriculaId);
            if (matricula == null)
                return BadRequest("Matrícula não encontrada, realize uma nova matricula.");

            if (matricula.Ativa)
                return BadRequest("Matrícula já está ativa, não é necessário realizar um novo pagamento.");

            var pagamentoCommand = new RegistrarPagamentoCommand(Guid.NewGuid(), matriculaId, curso.Valor, pagamento.DadosCartao_Nome, pagamento.DadosCartao_Numero, pagamento.DadosCartao_Vencimento, pagamento.DadosCartao_CcvCvc);

            await _mediator.SendCommand(pagamentoCommand);

            return Response(pagamentoCommand.AggregateId);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("cursos/{cursoId:guid}/matricular-aluno/{alunoId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AlunoSeMatricular([FromRoute] Guid cursoId, [FromRoute] Guid alunoId, [FromServices] ICursoReadOnlyRepository cursoRepository)
        {
            var curso = await cursoRepository.ObterPorId(cursoId);
            if (curso == null)
                return BadRequest("Curso não encontrado.");

            var matriculaId = await _alunoService.Matricular(alunoId, cursoId);
            if (!matriculaId.HasValue)
                return BadRequest();

            return Response(matriculaId);
        }
    }
}
