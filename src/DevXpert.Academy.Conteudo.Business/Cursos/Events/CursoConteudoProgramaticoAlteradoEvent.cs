using DevXpert.Academy.Core.Domain.Messages;
using System;

namespace DevXpert.Academy.Conteudo.Business.Cursos.Events
{
    internal sealed class CursoConteudoProgramaticoAlteradoEvent : Event
    {
        public Guid Id => AggregateId;
        public string Descricao { get; private set; }
        public int CargaHoraria { get; private set; }

        public CursoConteudoProgramaticoAlteradoEvent(Guid id, string descricao, int cargaHoraria) : base("Curso")
        {
            AggregateId = id;
            Descricao = descricao;
            CargaHoraria = cargaHoraria;
        }
    }
}
