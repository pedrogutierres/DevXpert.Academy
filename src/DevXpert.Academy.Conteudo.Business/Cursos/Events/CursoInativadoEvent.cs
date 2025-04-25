using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Events
{
    internal sealed class CursoInativadoEvent : Event
    {
        public Guid Id => AggregateId;

        public CursoInativadoEvent(Guid id) : base("Curso")
        {
            AggregateId = id;
        }
    }
}
