using DevXpert.Academy.Core.Data;
using DevXpert.Academy.Core.Domain.Communication.Mediatr;
using DevXpert.Academy.Core.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevXpert.Academy.Conteudo.Data
{
    public class ConteudoContext : SQLDbContext, IUnitOfWork
    {
        public ConteudoContext(IMediatorHandler mediator) : base(mediator)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConteudoContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> CommitAsync()
        {
            return await base.SaveChangesAsync() > 0;
        }

        public void DettachAllEntities()
        {
            ChangeTracker.Clear();
        }
    }
}
