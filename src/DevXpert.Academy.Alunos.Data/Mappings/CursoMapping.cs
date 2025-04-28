using DevXpert.Academy.Alunos.Domain.Cursos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpert.Academy.Alunos.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Matriculas)
                .WithOne(p => p.Curso)
                .HasForeignKey(p => p.CursoId);

            builder.ToTable("Cursos");
        }
    }
}
