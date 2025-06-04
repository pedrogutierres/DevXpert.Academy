using DevXpert.Academy.API.Configurations;
using DevXpert.Academy.API.ViewModels.Usuarios;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.DomainObjects;
using DevXpert.Academy.Core.Domain.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevXpert.Academy.API.Controllers
{
    [Route("api/usuarios")]
    public class UsuariosController : MainController
    {
        private readonly JwtTokenGenerate _jwtTokenGenerate;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsuariosController(
            JwtTokenGenerate jwtTokenGenerate,
            SignInManager<IdentityUser> signInManager,
            INotificationHandler<DomainNotification> notifications,
            IUser user,
            IMediatorHandler mediator)
            : base(notifications, user, mediator)
        {
            _jwtTokenGenerate = jwtTokenGenerate;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, lockoutOnFailure: false);

            if (result.Succeeded)
                return Ok(await _jwtTokenGenerate.GerarToken(login.Email));

            return BadRequest(new ProblemDetails
            {
                Title = "Falha na autenticação",
                Detail = "E-mail e/ou senha inválidos."
            });
        }
    }
}
