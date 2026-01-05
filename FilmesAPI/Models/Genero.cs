using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Genero
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo de gênero é obrigatório")]
    public string Nome { get; set; } // No db está "Genero"

    // Propriedade de Navegação: Um gênero tem vários "vínculos" de filmes
    public virtual ICollection<FilmesGenero> FilmesGenero { get; set; }
}
