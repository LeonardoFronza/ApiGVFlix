﻿// <auto-generated />
using System;
using API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Migrations
{
    [DbContext(typeof(Contexto))]
    partial class ContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("GVFlix")
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("API.EntitieModels.Ator", b =>
                {
                    b.Property<int>("CodigoAtor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("CodigoAtor");

                    b.ToTable("Atores");

                    b.HasData(
                        new
                        {
                            CodigoAtor = 1,
                            DataNascimento = new DateTime(2022, 4, 24, 13, 23, 22, 52, DateTimeKind.Local).AddTicks(8281),
                            Nome = "Ator1"
                        });
                });

            modelBuilder.Entity("API.EntitieModels.Filme", b =>
                {
                    b.Property<int>("CodigoFilme")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DataLancamento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Excluido")
                        .HasColumnType("bit");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UrlImagem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodigoFilme");

                    b.ToTable("Filmes");

                    b.HasData(
                        new
                        {
                            CodigoFilme = 1,
                            DataLancamento = new DateTime(2022, 4, 24, 13, 23, 22, 48, DateTimeKind.Local).AddTicks(8306),
                            Descricao = "DescricaoFilme1",
                            Excluido = false,
                            Titulo = "Filme1",
                            UrlImagem = "https://media-exp1.licdn.com/dms/image/C4E0BAQFFxN23wILsEw/company-logo_200_200/0/1648731270519?e=2147483647&v=beta&t=_KEZSCAMqGkm_X_09BocNUTs8hQRcr_9Ub2yFl0Epig"
                        });
                });

            modelBuilder.Entity("API.EntitieModels.FilmeAtor", b =>
                {
                    b.Property<int>("CodigoFilmeAtor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CodigoAtor")
                        .HasColumnType("int");

                    b.Property<int>("CodigoFilme")
                        .HasColumnType("int");

                    b.HasKey("CodigoFilmeAtor");

                    b.HasIndex("CodigoAtor");

                    b.HasIndex("CodigoFilme");

                    b.ToTable("FilmesAtores");
                });

            modelBuilder.Entity("API.EntitieModels.Historico", b =>
                {
                    b.Property<int>("CodigoHistorico")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CodigoFilme")
                        .HasColumnType("int");

                    b.Property<int>("CodigoUsuario")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("DataReproducao")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Nota")
                        .HasColumnType("int");

                    b.HasKey("CodigoHistorico");

                    b.HasIndex("CodigoFilme");

                    b.HasIndex("CodigoUsuario");

                    b.ToTable("Historicos");
                });

            modelBuilder.Entity("API.EntitieModels.Usuario", b =>
                {
                    b.Property<int>("CodigoUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Excluido")
                        .HasColumnType("bit");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("CodigoUsuario");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            CodigoUsuario = 1,
                            DataNascimento = new DateTime(2022, 4, 24, 13, 23, 22, 64, DateTimeKind.Local).AddTicks(5646),
                            Email = "admin@gvflix.com",
                            Excluido = false,
                            Nome = "Admin",
                            Role = 1,
                            Senha = "admin123"
                        });
                });

            modelBuilder.Entity("API.EntitieModels.FilmeAtor", b =>
                {
                    b.HasOne("API.EntitieModels.Ator", "Ator")
                        .WithMany("FilmesAtores")
                        .HasForeignKey("CodigoAtor")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.EntitieModels.Filme", "Filme")
                        .WithMany("FilmesAtores")
                        .HasForeignKey("CodigoFilme")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Ator");

                    b.Navigation("Filme");
                });

            modelBuilder.Entity("API.EntitieModels.Historico", b =>
                {
                    b.HasOne("API.EntitieModels.Filme", "Filme")
                        .WithMany()
                        .HasForeignKey("CodigoFilme")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.EntitieModels.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("CodigoUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Filme");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("API.EntitieModels.Ator", b =>
                {
                    b.Navigation("FilmesAtores");
                });

            modelBuilder.Entity("API.EntitieModels.Filme", b =>
                {
                    b.Navigation("FilmesAtores");
                });
#pragma warning restore 612, 618
        }
    }
}
