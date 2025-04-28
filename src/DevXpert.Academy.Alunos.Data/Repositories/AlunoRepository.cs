using DevXpert.Academy.Alunos.Domain.Alunos;
using DevXpert.Academy.Alunos.Domain.Alunos.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevXpert.Academy.Alunos.Data.Repositories
{
    public class AlunoRepository : Repository<Aluno>, IAlunoRepository
    {
        public AlunoRepository(AlunosContext context) : base(context)
        { }

        public Task<Aluno> ObterAtravesDaMatricula(Guid matriculaId)
        {
            return DbSet.FirstOrDefaultAsync(p => p.Matriculas.Any(m => m.Id == matriculaId));
        }
    }
}
