using API.Context.EntitieConfigurations;
using API.EntitieModels;
using Microsoft.EntityFrameworkCore;

namespace API.Context
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Ator> Atores {get;set;}
        public DbSet<FilmeAtor> FilmesAtores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Historico> Historicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do schema padrão do banco de dados (para não ser 'dbo').
            modelBuilder.HasDefaultSchema("GVFlix");

            // Configuração das tabelas e relacionamentos.
            modelBuilder.ApplyConfiguration(new FilmeConfiguration());
            modelBuilder.ApplyConfiguration(new AtorConfiguration());
            modelBuilder.ApplyConfiguration(new FilmeAtorConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new HistoricoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
