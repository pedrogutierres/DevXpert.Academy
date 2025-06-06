﻿// <auto-generated />
using System;
using DevXpert.Academy.Core.EventSourcing.EventStore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevXpert.Academy.Core.EventSourcing.Migrations
{
    [DbContext(typeof(EventStoreSQLContext))]
    partial class EventStoreSQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("DevXpert.Academy.Core.Domain.DataModels.StoredEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AggregateRoot")
                        .HasColumnType("TEXT");

                    b.Property<string>("Data")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageType")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Action");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT")
                        .HasColumnName("CreationDate");

                    b.Property<Guid?>("User")
                        .HasColumnType("TEXT")
                        .HasColumnName("UserId");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("StoredEvent");
                });
#pragma warning restore 612, 618
        }
    }
}
