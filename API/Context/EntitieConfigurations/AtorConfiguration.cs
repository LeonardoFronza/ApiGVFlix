using API.EntitieModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace API.Context.EntitieConfigurations
{
    public class AtorConfiguration : IEntityTypeConfiguration<Ator>
    {
        public void Configure(EntityTypeBuilder<Ator> builder)
        {
            // Configurações da Tabela.
            builder.ToTable("Atores");
            builder.HasKey(a => a.CodigoAtor); // Chave primária.

            // Configurar as Colunas.
            builder.Property(a => a.CodigoAtor)
                .UseIdentityColumn(1, 1)
                .IsRequired();

            builder.Property(a => a.Nome)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(a => a.DataNascimento)
                .IsRequired();

            // Carga Inicial.
            builder.HasData(
                new Ator
                {
                    CodigoAtor = 1,
                    Nome = "Ator1",
                    DataNascimento = DateTime.Now
                });
        }
    }
}
