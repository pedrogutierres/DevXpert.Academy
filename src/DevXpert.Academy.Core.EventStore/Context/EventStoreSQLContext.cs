using DevXpert.Academy.Core.EventSourcing.EventStore.Mappings;
using Microsoft.EntityFrameworkCore;
using DevXpert.Academy.Core.Domain.Data;

namespace DevXpert.Academy.Core.EventSourcing.EventStore.Context
{
    public class EventStoreSQLContext : DbContext
    {
        public DbSet<StoredEvent> StoredEvent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StoredEventMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
