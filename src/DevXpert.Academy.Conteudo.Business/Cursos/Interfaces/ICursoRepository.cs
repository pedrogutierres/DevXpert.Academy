using DevXpert.Academy.Core.Domain.Data;
using System;
using System.Threading.Tasks;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Interfaces
{
    public interface ICursoRepository : IRepository<Curso>
    {
        Task<bool> ExistePorTitulo(string titulo, Guid? cursoId = null);
    }
}
