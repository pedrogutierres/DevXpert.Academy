using DevXpert.Academy.Alunos.Domain.Alunos;
using DevXpert.Academy.Alunos.Domain.Alunos.Interfaces;

namespace DevXpert.Academy.Alunos.Data.Repositories
{
    public class AlunoRepository : Repository<Aluno>, IAlunoRepository
    {
        public AlunoRepository(AlunosContext context) : base(context)
        { }
    }
}
