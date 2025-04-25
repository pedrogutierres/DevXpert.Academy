using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Alunos.Domain.Alunos.Interfaces
{
    public interface IAlunoService
    {
        Task<bool> Cadastrar(Aluno aluno);
        Task<bool> Matricular(Aluno aluno, Guid cursoId);
        Task<bool> SeMatricular(Guid cursoId);
    }
}
