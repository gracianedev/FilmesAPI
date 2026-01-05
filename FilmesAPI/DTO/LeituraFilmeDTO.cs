using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FilmesAPI.DTO
{
    public class LeituraFilmeDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<LeituraGeneroDTO> Genero{ get; set; }
        
        public int Duracao { get; set; }
        public int Ano { get; set; }

        // Campo que não existe no db mostra ao usuário quando foi a busca
        public string HoraDaConsulta { get; set; } = DateTime.UtcNow.ToString("r");

    }
}