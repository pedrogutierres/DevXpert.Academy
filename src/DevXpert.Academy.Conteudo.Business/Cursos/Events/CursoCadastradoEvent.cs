using DevXpert.Academy.Core.Domain.Messages;
using System;
using System.Collections.Generic;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Events
{
    internal sealed class CursoCadastradoEvent : Event
    {
        public Guid Id => AggregateId;
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public int CargaHoraria { get; private set; }
        public List<AulaCadastradaEvent> Aulas { get; private set; }

        public CursoCadastradoEvent(Guid id, string titulo, string descricao, int cargaHorario, List<AulaCadastradaEvent> aulas) : base("Curso")
        {
            AggregateId = id;
            Titulo = titulo;
            Descricao = descricao;
            CargaHoraria = cargaHorario;
            Aulas = aulas;
        }
    }
}
