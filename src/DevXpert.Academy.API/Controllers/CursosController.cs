using AutoMapper;
using DevXpert.Academy.API.ViewModels.Cursos;
using DevXpert.Academy.Conteudo.Domain.Cursos.Interfaces;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.DomainObjects;
using DevXpert.Academy.Core.Domain.Messages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevXpert.Academy.API.Controllers
{
    [Route("api/cursos")]
    public class CursosController : MainController
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMapper _mapper;

        public CursosController(
            ICursoRepository cursoRepository,
            IMapper mapper,
            INotificationHandler<DomainNotification> notifications, 
            IUser user,
            IMediatorHandler mediator) 
            : base(notifications, user, mediator)
        {
            _cursoRepository = cursoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<CursoViewModel>> ObterCursos()
        {
            return _mapper.Map<List<CursoViewModel>>(await _cursoRepository.Buscar(p => true).OrderBy(p => p.Titulo).ToListAsync());
        }
    }
}
