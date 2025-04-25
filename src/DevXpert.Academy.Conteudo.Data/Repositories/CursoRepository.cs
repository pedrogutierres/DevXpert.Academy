using DevXpert.Academy.Conteudo.Business.Cursos;
using DevXpert.Academy.Conteudo.Business.Cursos.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Conteudo.Data.Repositories
{
    public class CursoRepository : Repository<Curso>, ICursoRepository
    {
        public CursoRepository(ConteudoContext context) : base(context)
        { }

        public override Task Excluir(Guid id) => throw new NotSupportedException("Exclusão de curso não suportada. O curso deve ser inativado.");

        public Task<bool> ExistePorTitulo(string titulo, Guid? cursoId = null)
        {
            if (cursoId.HasValue)
                return DbSet.AsNoTracking().IgnoreQueryFilters().AnyAsync(c => c.Titulo.Equals(titulo, StringComparison.InvariantCultureIgnoreCase) && c.Id != cursoId.Value);
            return DbSet.AsNoTracking().IgnoreQueryFilters().AnyAsync(c => c.Titulo.Equals(titulo, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
