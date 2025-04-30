using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Alunos.Domain.Alunos.Interfaces
{
    public interface IAlunoService
    {
        Task<bool> Cadastrar(Aluno aluno);
        Task<Guid?> Matricular(Aluno aluno, Guid cursoId);
        Task<Guid?> SeMatricular(Guid cursoId);
    }
}
