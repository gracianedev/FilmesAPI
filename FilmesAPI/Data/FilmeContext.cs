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
}