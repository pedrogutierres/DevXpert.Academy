using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Conteudo.Domain.Cursos.Events
{
    public sealed class CursoValorAlteradoEvent : Event
    {
        public decimal Valor { get; private set; }

        public CursoValorAlteradoEvent(Guid id, decimal valor) : base("Curso")
        {
            AggregateId = id;
            Valor = valor;
        }
    }
}
