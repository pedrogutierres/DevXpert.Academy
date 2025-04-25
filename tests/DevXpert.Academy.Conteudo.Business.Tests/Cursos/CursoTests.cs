using DevXpert.Academy.Conteudo.Business.Cursos;
using DevXpert.Academy.Conteudo.Business.Cursos.ValuesObjects;

namespace DevXpert.Academy.Conteudo.Business.Tests.Cursos
{
    public class CursoTests
    {
        [Fact(DisplayName = "Validar curso")]
        [Trait("Domain", "Cursos")]
        public void Cursos_ValidarCurso_DeveEstarValido()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var aulas = new List<Aula>
            {
                new(Guid.NewGuid(), id, "Aula 1", "https://www.youtube.com/watch?v=1"),
                new(Guid.NewGuid(), id, "Aula 2", "https://www.youtube.com/watch?v=2")
            };

            var curso = new Curso(id, "Curso de ASP.NET", new ConteudoProgramatico("CURSO PARA ASP.NET CORE", 20), aulas);

            // Act
            var result = curso.EhValido();

            // Assert
            Assert.True(result);
            Assert.Equal(aulas.Count, curso.Aulas?.Count ?? 0);
        }

        [Fact(DisplayName = "Validar curso com informações esperadas")]
        [Trait("Domain", "Cursos")]
        public void Cursos_ValidarCurso_DeveTerInformacoesExperadas()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string titulo = "Curso de ASP.NET";
            string descricao = "CURSO PARA ASP.NET CORE";
            int cargaHoraria = 20;

            // Act
            var curso = new Curso(id, titulo, new ConteudoProgramatico(descricao, cargaHoraria), null);
            curso.Ativar();

            // Assert
            Assert.Equal(id, curso.Id);
            Assert.Equal(titulo, curso.Titulo);
            Assert.Equal(descricao, curso.ConteudoProgramatico?.Descricao);
            Assert.Equal(cargaHoraria, curso.ConteudoProgramatico?.CargaHoraria ?? 0);
            Assert.True(curso.Ativo);
        }

        [Fact(DisplayName = "Validar curso deve entrar como inativado")]
        [Trait("Domain", "Cursos")]
        public void Cursos_ValidarCurso_DeveEntrarComoInativado()
        {
            // Arrange
            var curso = new Curso(Guid.NewGuid(), "Curso de ASP.NET", new ConteudoProgramatico("CURSO PARA ASP.NET CORE", 20), null);

            // Act

            // Assert
            Assert.False(curso.Ativo);
        }

        [Fact(DisplayName = "Validar aula com informações esperadas")]
        [Trait("Domain", "Aulas")]
        public void Aulas_ValidarAula_DeveTerInformacoesExperadas()
        {
            // Arrange
            Guid cursoId = Guid.NewGuid();
            Guid id = Guid.NewGuid();
            string titulo = "CURSO PARA ASP.NET CORE";
            string videoUrl = "https://www.youtube.com/watch?v=1";

            // Act
            var aula = new Aula(id, cursoId, titulo, videoUrl);

            // Assert
            Assert.Equal(id, aula.Id);
            Assert.Equal(cursoId, aula.CursoId);
            Assert.Equal(titulo, aula.Titulo);
            Assert.Equal(videoUrl, aula.VideoUrl);
        }
    }
}
