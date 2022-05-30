using API.EntitieModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Context.EntitieConfigurations
{
    public class HistoricoConfiguration : IEntityTypeConfiguration<Historico>
    {
        public void Configure(EntityTypeBuilder<Historico> builder)
        {
            // Configurações da Tabela.
            builder.ToTable("Historicos");
            builder.HasKey(h => h.CodigoHistorico); // Chave primária.

            //Configuração de relacionamentos.
            builder.HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.CodigoUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Filme)
                .WithMany()
                .HasForeignKey(h => h.CodigoFilme)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar as Colunas.
            builder.Property(h => h.CodigoHistorico)
                .UseIdentityColumn(1, 1)
                .IsRequired();

            builder.Property(h => h.CodigoUsuario)
                .IsRequired();

            builder.Property(h => h.CodigoFilme)
                .IsRequired();

            builder.Property(h => h.DataReproducao)
                .IsRequired();

            builder.Property(h => h.Nota)
                .IsRequired();
        }
    }
}
