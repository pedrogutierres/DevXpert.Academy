﻿using Microsoft.EntityFrameworkCore;

namespace DevXpert.Academy.Core.EventSourcing.EventStore.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, EntityTypeConfiguration<TEntity> configuration) where TEntity : class
        {
            configuration.Map(modelBuilder.Entity<TEntity>());
        }
    }

}
