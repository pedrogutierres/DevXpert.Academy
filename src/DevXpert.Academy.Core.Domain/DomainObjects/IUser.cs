using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DevXpert.Academy.Core.Domain.DomainObjects
{
    public interface IUser
    {
        string Nome { get; }
        Guid UsuarioId { get; }
        Guid? UsuarioIdNullValue();
        bool Autenticado();
        bool TemPermissao(string moduloEClaim, bool throwBusinessException = false);
        string Ip();
        IEnumerable<Claim> RetornarClaims();

        bool EhUmAluno();
        bool EhUmAdministrador();
    }
}
