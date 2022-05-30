using API.EntitieModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace API.Context.EntitieConfigurations
{
    public class FilmeConfiguration : IEntityTypeConfiguration<Filme>
    {
        public void Configure(EntityTypeBuilder<Filme> builder)
        {
            // Configurações da Tabela.
            builder.ToTable("Filmes");
            builder.HasKey(f => f.CodigoFilme); // Chave primária.

            // Configurar as Colunas.
            builder.Property(f => f.CodigoFilme)
                .UseIdentityColumn(1, 1)
                .IsRequired();

            builder.Property(f => f.Titulo)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(f => f.Descricao)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(f => f.DataLancamento)
                .IsRequired();

            builder.Property(f => f.UrlImagem)
                .HasColumnType("nvarchar(max)") // Caso não especificar o tamanho de string com .HasMaxLength, por padrão é 'nvarchar(max)'. Coloquei para ficar claro.
                .IsRequired();

            builder.Property(f => f.Excluido)
                .IsRequired();

            // Carga Inicial.
            builder.HasData(
                new Filme
                {
                    CodigoFilme = 1,
                    Titulo = "Filme1",
                    Descricao = "DescricaoFilme1",
                    DataLancamento = DateTime.Now,
                    UrlImagem = "https://media-exp1.licdn.com/dms/image/C4E0BAQFFxN23wILsEw/company-logo_200_200/0/1648731270519?e=2147483647&v=beta&t=_KEZSCAMqGkm_X_09BocNUTs8hQRcr_9Ub2yFl0Epig",
                    Excluido = false
                });
        }
    }
}
