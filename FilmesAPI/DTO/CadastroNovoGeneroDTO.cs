using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.DTO
{
    public class CadastroNovoGeneroDTO
    {

    [Required(ErrorMessage = "O nome do gênero é obrigatório")]
    public string Nome { get; set; }
    }
}
        
    
