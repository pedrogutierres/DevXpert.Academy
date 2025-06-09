using DevXpert.Academy.Alunos.Domain.Alunos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpert.Academy.Alunos.Data.Mappings
{
    public class AulaConcluidaMapping : IEntityTypeConfiguration<AulaConcluida>
    {
        public void Configure(EntityTypeBuilder<AulaConcluida> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.ToTable("AlunosAulasConcluidas");
        }
    }
}
