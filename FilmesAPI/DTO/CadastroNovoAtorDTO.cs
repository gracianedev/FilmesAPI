using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FilmesAPI.Models;

namespace FilmesAPI.DTO
{
    public class CadastroNovoAtorDTO
    {
    [Required (ErrorMessage = "O primeiro nome é obrigatório.")]
    public string PrimeiroNome { get; set; } 

    [Required (ErrorMessage = "O primeiro nome é obrigatório.")]
    public string UltimoNome { get; set; } 

    public string Genero { get; set; }

    }
}