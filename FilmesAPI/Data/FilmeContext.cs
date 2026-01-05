using Microsoft.EntityFrameworkCore;
using FilmesAPI.Models;

namespace FilmesAPI.Data;

public class FilmeContext : DbContext
{
    public FilmeContext(DbContextOptions<FilmeContext> opts)
        : base(opts)
    {
        
    }

    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<FilmesGenero> FilmesGenero { get; set; }

    // Configuração dos relacionamentos das tabelas conforme DB (Fluent API)
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Define que FilmesGenero se liga com Filme
        builder.Entity<FilmesGenero>()
            .HasOne(fg => fg.Filme)
            .WithMany(f => f.FilmesGenero)
            .HasForeignKey(fg => fg.IdFilme)
            .OnDelete(DeleteBehavior.Cascade);

        // Define que FilmesGenero se liga com Genero
        builder.Entity<FilmesGenero>()
            .HasOne(fg => fg.Genero)
            .WithMany(g => g.FilmesGenero)
            .HasForeignKey(fg => fg.IdGenero);

        // Mapeamento da propriedade "Nome" da classe Genero para a coluna "Genero" do DB
        builder.Entity<Genero>().Property(g => g.Nome).HasColumnName("Genero");
    }
}