using API.EntitieModels;
using API.Enumeradores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace API.Context.EntitieConfigurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Configurações da Tabela.
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.CodigoUsuario); // Chave primária.

            // Configurar as Colunas.
            builder.Property(u => u.CodigoUsuario)
                .UseIdentityColumn(1, 1)
                .IsRequired();

            builder.Property(u => u.Nome)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(u => u.DataNascimento)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(u => u.Senha)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(u => u.Role)
                .IsRequired();

            builder.Property(u => u.Excluido)
                .IsRequired();

            // Carga Inicial.
            builder.HasData(
                new Usuario
                {
                    CodigoUsuario = 1,
                    Nome = "Admin",
                    DataNascimento = DateTime.Now,
                    Email = "admin@gvflix.com",
                    Senha = "admin123",
                    Role = Role.Administrador,
                    Excluido = false
                });
        }
    }
}