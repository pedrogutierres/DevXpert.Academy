using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Events
{
    internal sealed class AulaCadastradaEvent : Event
    {
        public Guid Id { get; private set; }
        public string Titulo { get; private set; }
        public string VideoUrl { get; private set; }

        public AulaCadastradaEvent(Guid aggregateId, Guid id, string titulo, string videoUrl) : base("Aula")
        {
            AggregateId = aggregateId;
            Id = id;
            Titulo = titulo;
            VideoUrl = videoUrl;
        }
    }
}
