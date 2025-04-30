using DevXpert.Academy.Conteudo.Business.Cursos;
using DevXpert.Academy.Conteudo.Business.Cursos.ValuesObjects;

namespace DevXpert.Academy.Conteudo.Business.Tests.Cursos
{
    public class CursoTests
    {
        [Fact(DisplayName = "Validar curso com dados consistentes")]
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

            var curso = new Curso(id, "Curso de ASP.NET", new ConteudoProgramatico("CURSO PARA ASP.NET CORE", 20), 100, aulas);

            // Act
            var result = curso.EhValido();

            // Assert
            Assert.True(result);
            Assert.Equal(aulas.Count, curso.Aulas?.Count ?? 0);
        }

        [Fact(DisplayName = "Validar curso com informações esperadas")]
        [Trait("Domain", "Cursos")]
        public void Cursos_ValidarCurso_DeveTerInformacoesEsperadas()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string titulo = "Curso de ASP.NET";
            string descricao = "CURSO PARA ASP.NET CORE";
            int cargaHoraria = 20;

            // Act
            var curso = new Curso(id, titulo, new ConteudoProgramatico(descricao, cargaHoraria), 100, null);
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
            var curso = new Curso(Guid.NewGuid(), "Curso de ASP.NET", new ConteudoProgramatico("CURSO PARA ASP.NET CORE", 20), 100, null);

            // Act

            // Assert
            Assert.False(curso.Ativo);
        }

        [Fact(DisplayName = "Validar aula com informações esperadas")]
        [Trait("Domain", "Aulas")]
        public void Aulas_ValidarAula_DeveTerInformacoesEsperadas()
        {
            // Arrange
            Guid cursoId = Guid.NewGuid();
            Guid id = Guid.NewGuid();
            string titulo = "Aula 1";
            string videoUrl = "https://www.youtube.com/watch?v=1";

            // Act
            var aula = new Aula(id, cursoId, titulo, videoUrl);

            // Assert
            Assert.Equal(id, aula.Id);
            Assert.Equal(cursoId, aula.CursoId);
            Assert.Equal(titulo, aula.Titulo);
            Assert.Equal(videoUrl, aula.VideoUrl);
        }

        [Fact(DisplayName = "Validar curso não pode ser ativado sem cursos")]
        [Trait("Domain", "Cursos")]
        public void Cursos_ValidarCurso_DeveNaoPodeSerAtivadoSemCursos()
        {
            // Arrange
            var curso = new Curso(Guid.NewGuid(), "Curso de ASP.NET", new ConteudoProgramatico("CURSO PARA ASP.NET CORE", 20), 100, null);

            // Act
            curso.Ativar();
            curso.EhValido();

            // Assert
            Assert.False(curso.ValidationResult.IsValid);
            Assert.Contains(curso.ValidationResult.Errors, e => e.ErrorMessage == $"O curso {curso.Titulo} deve ter aulas para ser ativado.");
        }

        [Fact(DisplayName = "Validar curso com aula adicionado com sucesso")]
        [Trait("Domain", "Cursos")]
        public void Cursos_ValidarCurso_DeveAdicionarAulaComSucesso()
        {
            // Arrange
            var curso = new Curso(Guid.NewGuid(), "Curso de ASP.NET", new ConteudoProgramatico("CURSO PARA ASP.NET CORE", 20), 100, null);

            var aula = new Aula(Guid.NewGuid(), curso.Id, "Aula 1", "https://www.youtube.com/watch?v=1");

            // Act
            curso.AdicionarAula(aula);
            curso.Ativar();
            curso.EhValido();

            // Assert
            Assert.True(curso.Ativo);
            Assert.True(curso.ValidationResult.IsValid);
            Assert.Contains(curso.Aulas, e => e.Id == aula.Id);
        }

        [Fact(DisplayName = "Validar curso com aula removida com sucesso")]
        [Trait("Domain", "Cursos")]
        public void Cursos_ValidarCurso_DeveRemoverAulaComSucesso()
        {
            // Arrange
            Guid cursoId = Guid.NewGuid();
            var aula = new Aula(Guid.NewGuid(), cursoId, "Aula 1", "https://www.youtube.com/watch?v=1");
            var curso = new Curso(cursoId, "Curso de ASP.NET", new ConteudoProgramatico("CURSO PARA ASP.NET CORE", 20), 100, [aula]);

            // Act
            curso.RemoverAula(aula);
            curso.EhValido();

            // Assert
            Assert.False(curso.Ativo);
            Assert.True(curso.ValidationResult.IsValid);
            Assert.DoesNotContain(curso.Aulas, e => e.Id == aula.Id);
            Assert.True(curso.Aulas.Count == 0);
        }
    }
}
