using DevXpert.Academy.Alunos.Domain.Alunos.ValuesObjects;
using DevXpert.Academy.Alunos.Domain.Cursos;
using DevXpert.Academy.Core.Domain.DomainObjects;
using System;

namespace DevXpert.Academy.Alunos.Domain.Alunos
{
    public class Matricula : Entity
    {
        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public DateTime? DataHoraConclusao { get; private set; }
        public bool Ativa { get; private set; }
        public bool Concluido { get; private set; }
        public Certificado Certificado { get; private set; }

        public virtual Aluno Aluno { get; private set; }
        public virtual Curso Curso { get; private set; }

        private Matricula() { }
        public Matricula(Guid id, Guid alunoId, Guid cursoId)
        {
            Id = id;
            AlunoId = alunoId;
            CursoId = cursoId;
            DataHoraCriacao = DateTime.Now;
            Certificado = null;
            Ativa = false;
            Concluido = false;

            // TODO: event
        }

        public void Ativar()
        {
            Ativa = true;

            // TODO: event
        }

        public void Bloquear()
        {
            Ativa = false;

            // TODO: event
        }

        public void Concluir()
        {
            Concluido = true;
            DataHoraConclusao = DateTime.Now;
            Certificado = new Certificado(DataHoraConclusao.Value);

            // TODO: event
        }

        public override bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
