﻿// <auto-generated />
using System;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(BotContext))]
    [Migration("20200526155150_Add_Statisctic_Table")]
    partial class Add_Statisctic_Table
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.Enities.Statistic", b =>
                {
                    b.Property<Guid>("StatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StatDate")
                        .HasColumnType("datetime2");

                    b.HasKey("StatId");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("Models.Enities.Users", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StatisticStatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("UserChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserLogin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserRole")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("StatisticStatId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Models.Enities.Users", b =>
                {
                    b.HasOne("Models.Enities.Statistic", null)
                        .WithMany("Users")
                        .HasForeignKey("StatisticStatId");
                });
#pragma warning restore 612, 618
        }
    }
}
