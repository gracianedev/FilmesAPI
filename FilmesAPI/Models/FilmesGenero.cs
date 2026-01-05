using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace FilmesAPI.Models
{
    public class FilmesGenero
    {
    [Key]
    [Required]
    public int Id { get; set; }

    // Chave estrangeira para Filme
    public int? IdFilme { get; set; }
    public virtual Filme Filme { get; set; }

    // Chave estrangeira para Genero
    public int? IdGenero { get; set; }
    public virtual Genero Genero { get; set; }
    }
}