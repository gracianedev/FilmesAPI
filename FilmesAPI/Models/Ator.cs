using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.Models
{
    public class Ator
    {
    [Key]
    [Required]
    public int Id { get; set; }

    public string PrimeiroNome { get; set; } 

    public string UltimoNome { get; set; } 

    public string Genero { get; set; }


    // Propriedade de Navegação: Um ator tem vários "vínculos" com ElencoFilme
    public virtual ICollection<Elenco> ElencoFilme { get; set; }

    }
}