using DevXpert.Academy.Conteudo.Business.Cursos.Events;
using System.Linq;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Adapters
{
    internal static class CursoAdapter
    {
        public static CursoCadastradoEvent ToCursoCadastradoEvent(Curso curso)
        {
            return new CursoCadastradoEvent(curso.Id, curso.Titulo, curso.ConteudoProgramatico?.Descricao, curso.ConteudoProgramatico?.CargaHoraria ?? 0, curso.Aulas?.Select(a => new AulaCadastradaEvent(curso.Id, a.Id, a.Titulo, a.VideoUrl)).ToList());
        }
    }
}
