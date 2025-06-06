using DevXpert.Academy.API.ResponseType;
using DevXpert.Academy.API.Tests.Config;
using DevXpert.Academy.API.ViewModels.Alunos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace DevXpert.Academy.API.Tests
{
    [TestCaseOrderer("DevXpert.Academy.API.Tests.Config.PriorityOrderer", "DevXpert.Academy.API.Tests")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class MatriculasControllersTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;
        private static Guid _cursoId = Guid.Empty;
        private static Guid _matriculaId = Guid.Empty;

        public MatriculasControllersTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Cadastrar curso como Administrador com dados válidos deve retornar sucesso")]
        [Trait("Cursos", "Integração API - Cursos")]
        [TestPriority(1)]
        public async Task Cursos_CadastrarCurso_ComoAdministrador_ComDadosValidos_DeveRetornarSucesso()
        {
            // Arrange
            await _testsFixture.RealizarLoginDeAdministrador();

            var novoCurso = new
            {
                Titulo = $"Curso Teste {Guid.NewGuid()}",
                Valor = 199.99m,
                ConteudoProgramatico = new
                {
                    Descricao = "Descrição do curso de teste.",
                    CargaHoraria = 40
                },
                Aulas = new List<object>
                {
                    new { Titulo = "Aula Inicial 1", VideoUrl = "http://video.com/aula-inicial-1" },
                    new { Titulo = "Aula Inicial 2", VideoUrl = "http://video.com/aula-inicial-2" }
                }
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/cursos", novoCurso);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadFromJsonAsync<ResponseSuccess>();
            Assert.NotNull(responseContent);
            Assert.NotNull(responseContent.Id);
            Assert.NotEqual(Guid.Empty, responseContent.Id);
            _cursoId = responseContent.Id.Value;
        }

        [Fact(DisplayName = "Aluno se matricular em curso deve retornar sucesso e registrar pagamento")]
        [Trait("Matriculas", "Integração API - Matrículas")]
        [TestPriority(10)]
        public async Task Matriculas_AlunoSeMatricular_DeveRetornarSucessoERegistrarPagamento()
        {
            // Arrange
            await _testsFixture.RealizarLoginDeAluno();

            Assert.NotEqual(Guid.Empty, _testsFixture.UsuarioId);
            Assert.NotEqual(Guid.Empty, _cursoId);

            var pagamento = new
            {
                DadosCartao_Nome = "Aluno Teste",
                DadosCartao_Numero = "1111222233334444",
                DadosCartao_Vencimento = "12/26",
                DadosCartao_CcvCvc = "123"
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/matriculas/cursos/{_cursoId}/se-matricular", pagamento);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadFromJsonAsync<ResponseSuccess>();
            Assert.NotNull(responseContent);
            Assert.NotNull(responseContent.Id);
            Assert.NotEqual(Guid.Empty, responseContent.Id);
            _matriculaId = responseContent.Id.Value;
        }

        [Fact(DisplayName = "Aluno se matricular em curso inexistente deve retornar BadRequest")]
        [Trait("Matriculas", "Integração API - Matrículas")]
        [TestPriority(11)]
        public async Task Matriculas_AlunoSeMatricular_CursoInexistente_DeveRetornarBadRequest()
        {
            // Arrange
            await _testsFixture.RealizarLoginDeAluno();
            var cursoIdInexistente = Guid.NewGuid();

            var pagamento = new
            {
                DadosCartao_Nome = "Aluno Teste",
                DadosCartao_Numero = "1111222233334444",
                DadosCartao_Vencimento = "12/26",
                DadosCartao_CcvCvc = "123"
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/matriculas/cursos/{cursoIdInexistente}/se-matricular", pagamento);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var errorContent = JsonConvert.DeserializeObject<ResponseError>(await response.Content.ReadAsStringAsync());
            Assert.Contains("Curso não encontrado.", errorContent.Errors.SelectMany(v => v.Value));
        }

        [Fact(DisplayName = "Aluno se matricular em curso que já está matriculado (ativo) deve retornar sucesso sem novo pagamento")]
        [Trait("Matriculas", "Integração API - Matrículas")]
        [TestPriority(12)]
        public async Task Matriculas_AlunoSeMatricular_JaMatriculadoAtivo_DeveRetornarSucessoSemNovoPagamento()
        {
            // Arrange
            await _testsFixture.RealizarLoginDeAluno();
            Assert.NotEqual(Guid.Empty, _testsFixture.UsuarioId);
            Assert.NotEqual(Guid.Empty, _cursoId);
            Assert.NotEqual(Guid.Empty, _matriculaId);

            var pagamento = new
            {
                DadosCartao_Nome = "Aluno Teste",
                DadosCartao_Numero = "1111222233334444",
                DadosCartao_Vencimento = "12/26",
                DadosCartao_CcvCvc = "123"
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/matriculas/cursos/{_cursoId}/se-matricular", pagamento);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadFromJsonAsync<ResponseSuccess>();
            Assert.NotNull(responseContent);
            Assert.Equal(_matriculaId, responseContent.Id);
        }

        [Fact(DisplayName = "Realizar pagamento de matrícula já ativa deve retornar BadRequest")]
        [Trait("Matriculas", "Integração API - Matrículas")]
        [TestPriority(21)]
        public async Task Matriculas_RealizarPagamento_MatriculaAtiva_DeveRetornarBadRequest()
        {
            // Arrange
            await _testsFixture.RealizarLoginDeAluno();
            Assert.NotEqual(Guid.Empty, _matriculaId);

            var pagamento = new
            {
                DadosCartao_Nome = "Aluno Teste Pagamento",
                DadosCartao_Numero = "5555666677778888",
                DadosCartao_Vencimento = "12/28",
                DadosCartao_CcvCvc = "456"
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/matriculas/{_matriculaId}/realizar-pagamento", pagamento);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var errorContent = JsonConvert.DeserializeObject<ResponseError>(await response.Content.ReadAsStringAsync());
            Assert.Contains("Matrícula já está ativa, não é necessário realizar um novo pagamento.", errorContent.Errors.SelectMany(v => v.Value));
        }

        [Fact(DisplayName = "Administrador matricular aluno em curso inexistente deve retornar BadRequest")]
        [Trait("Matriculas", "Integração API - Matrículas")]
        [TestPriority(31)]
        public async Task Matriculas_AdministradorMatricularAluno_CursoInexistente_DeveRetornarBadRequest()
        {
            // Arrange
            await _testsFixture.RealizarLoginDeAdministrador();
            Assert.NotEqual(Guid.Empty, _testsFixture.UsuarioId);
            var cursoIdInexistente = Guid.NewGuid();

            // Act
            var response = await _testsFixture.Client.PostAsync($"/api/matriculas/cursos/{cursoIdInexistente}/matricular-aluno/{_testsFixture.UsuarioId}", null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var errorContent = JsonConvert.DeserializeObject<ResponseError>(await response.Content.ReadAsStringAsync());
            Assert.Contains("Curso não encontrado.", errorContent.Errors.SelectMany(v => v.Value));
        }

        [Fact(DisplayName = "Administrador matricular aluno inexistente deve retornar BadRequest")]
        [Trait("Matriculas", "Integração API - Matrículas")]
        [TestPriority(32)]
        public async Task Matriculas_AdministradorMatricularAluno_AlunoInexistente_DeveRetornarBadRequest()
        {
            // Arrange
            await _testsFixture.RealizarLoginDeAdministrador();
            Assert.NotEqual(Guid.Empty, _cursoId);
            var alunoIdInexistente = Guid.NewGuid();

            // Act
            var response = await _testsFixture.Client.PostAsync($"/api/matriculas/cursos/{_cursoId}/matricular-aluno/{alunoIdInexistente}", null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var errorContent = JsonConvert.DeserializeObject<ResponseError>(await response.Content.ReadAsStringAsync());
            Assert.Contains("Aluno não encontrado.", errorContent.Errors.SelectMany(v => v.Value)); // Mensagem da BusinessException
        }

        [Fact(DisplayName = "Cancelar matricula por Aluno deve retornar sucesso e solicitar estorno")]
        [Trait("Matriculas", "Integração API - Matrículas")]
        [TestPriority(40)]
        public async Task Matriculas_CancelarMatricula_PorAluno_DeveRetornarSucessoESolicitarEstorno()
        {
            // Arrange
            await _testsFixture.RealizarLoginDeAluno();
            Assert.NotEqual(Guid.Empty, _matriculaId); // A matrícula deve existir

            var cancelamento = new
            {
                Motivo = "Desistência do curso pelo aluno."
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/matriculas/{_matriculaId}")
            {
                Content = JsonContent.Create(cancelamento)
            };
            var response = await _testsFixture.Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadFromJsonAsync<ResponseSuccess>();
            Assert.NotNull(responseContent);
            Assert.Equal(_matriculaId, responseContent.Id);
        }
    }
}