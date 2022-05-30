using API.EntitieModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Context.EntitieConfigurations
{
    public class FilmeAtorConfiguration : IEntityTypeConfiguration<FilmeAtor>
    {
        public void Configure(EntityTypeBuilder<FilmeAtor> builder)
        {
            // Configurações da Tabela.
            builder.ToTable("FilmesAtores");
            builder.HasKey(fa => fa.CodigoFilmeAtor); // Chave primária.

            //Configuração de relacionamentos.
            builder.HasOne(fa => fa.Filme)
                .WithMany(f => f.FilmesAtores)
                .HasForeignKey(fa => fa.CodigoFilme)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(fa => fa.Ator)
                .WithMany(a => a.FilmesAtores)
                .HasForeignKey(fa => fa.CodigoAtor)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar as Colunas.
            builder.Property(fa => fa.CodigoFilmeAtor)
                .UseIdentityColumn(1, 1)
                .IsRequired();

            builder.Property(fa => fa.CodigoFilme)
                .IsRequired();

            builder.Property(fa => fa.CodigoAtor)
                .IsRequired();
        }
    }
}
