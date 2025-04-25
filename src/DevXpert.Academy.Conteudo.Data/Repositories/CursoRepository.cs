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

        public Task<bool> ExistePorTitulo(string titulo, Guid? cursoId = null)
        {
            if (cursoId.HasValue)
                return DbSet.AsNoTracking().IgnoreQueryFilters().AnyAsync(c => c.Titulo.Equals(titulo, StringComparison.InvariantCultureIgnoreCase) && c.Id != cursoId.Value);
            return DbSet.AsNoTracking().IgnoreQueryFilters().AnyAsync(c => c.Titulo.Equals(titulo, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
