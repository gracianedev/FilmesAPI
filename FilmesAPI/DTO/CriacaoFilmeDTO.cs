using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.DTO
{
    public class CriacaoFilmeDTO
    {
        [Required(ErrorMessage = "O nome do filme é obrigatório")]
        public string Nome { get; set; }

        public List<int> GeneroIds { get; set; }

        [Range(70, 600, ErrorMessage = "A duração deve ter entre 70 e 600 minutos")]
        public int Duracao { get; set; }

        [Required(ErrorMessage = "O ano do filme é obrigatório")]
        public int Ano { get; set; }

    }
}