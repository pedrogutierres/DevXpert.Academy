using DevXpert.Academy.Conteudo.Business.Cursos.Interfaces;
using DevXpert.Academy.Conteudo.Business.Cursos.Validations.Specifications;
using DevXpert.Academy.Core.Domain.Validations;
using FluentValidation;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Validations
{
    public class CursoAptoParaCadastrarValidation : DomainValidator<Curso>
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoAptoParaCadastrarValidation(ICursoRepository cursoRepository) : base()
        {
            _cursoRepository = cursoRepository;

            ValidarTituloDisponivel();
        }

        private void ValidarTituloDisponivel()
        {
            RuleFor(p => p.Titulo)
                .IsValidAsync(e => new CursoDeveTerTituloDisponivelSpecification(e, _cursoRepository))
                .WithMessage("O CPF/CNPJ não está disponível.");
        }
    }
}
