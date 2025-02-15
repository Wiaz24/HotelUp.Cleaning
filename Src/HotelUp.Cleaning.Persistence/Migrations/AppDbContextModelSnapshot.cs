﻿// <auto-generated />
using System;
using System.Collections.Generic;
using HotelUp.Cleaning.Persistence.Const;
using HotelUp.Cleaning.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;

#nullable disable

namespace HotelUp.Cleaning.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("cleaning")
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "cleaning_type", new[] { "cyclic", "on_demand" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "task_status", new[] { "pending", "in_progress", "done" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HotelUp.Cleaning.Persistence.Entities.Cleaner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Cleaners", "cleaning");
                });

            modelBuilder.Entity("HotelUp.Cleaning.Persistence.Entities.CleaningTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CleanerId")
                        .HasColumnType("uuid");

                    b.Property<CleaningType>("CleaningType")
                        .HasColumnType("cleaning.cleaning_type");

                    b.Property<DateTime>("RealisationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ReservationId")
                        .HasColumnType("uuid");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("integer");

                    b.Property<TaskStatus>("Status")
                        .HasColumnType("cleaning.task_status");

                    b.HasKey("Id");

                    b.HasIndex("CleanerId");

                    b.HasIndex("ReservationId");

                    b.ToTable("CleaningTasks", "cleaning");
                });

            modelBuilder.Entity("HotelUp.Cleaning.Persistence.Entities.Reservation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<int>>("RoomNumbers")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Reservations", "cleaning");
                });

            modelBuilder.Entity("HotelUp.Cleaning.Persistence.Entities.CleaningTask", b =>
                {
                    b.HasOne("HotelUp.Cleaning.Persistence.Entities.Cleaner", "Cleaner")
                        .WithMany("CleaningTasks")
                        .HasForeignKey("CleanerId");

                    b.HasOne("HotelUp.Cleaning.Persistence.Entities.Reservation", "Reservation")
                        .WithMany()
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cleaner");

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("HotelUp.Cleaning.Persistence.Entities.Cleaner", b =>
                {
                    b.Navigation("CleaningTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
