using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.Models
{
    public class Elenco
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string Papel { get; set; }

        // Chave estrangeira para Ator
        public int? IdAtor { get; set; }
        public virtual Ator Ator { get; set; }

        // Chave estrangeira para Filme
        public int? IdFilme { get; set; }
        public virtual Filme Filme { get; set; }
    }
}
