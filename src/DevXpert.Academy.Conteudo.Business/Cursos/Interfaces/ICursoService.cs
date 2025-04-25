using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Interfaces
{
    public interface ICursoService
    {
        Task<bool> Cadastrar(Curso curso);
        Task<bool> Alterar(Curso curso);
        Task<bool> Ativar(Guid id);
        Task<bool> Inativar(Guid id);
    }
}
