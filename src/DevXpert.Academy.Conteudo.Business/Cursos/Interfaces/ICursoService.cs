using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Interfaces
{
    public interface ICursoService
    {
        Task<bool> CadastrarCurso(Curso curso);
        Task<bool> AlterarCurso(Curso curso);
        Task<bool> ExcluirCurso(Guid id);
    }
}
