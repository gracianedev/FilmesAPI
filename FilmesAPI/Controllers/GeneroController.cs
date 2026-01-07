using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.DTO;
using FilmesAPI.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;


namespace FilmesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneroController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public GeneroController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        [HttpPost]
        public async Task<IActionResult> AdicionarGenero([FromBody] CadastroNovoGeneroDTO generoDto)
        {
            try
            {
                Genero genero = _mapper.Map<Genero>(generoDto);
                _context.Generos.Add(genero);
                await _context.SaveChangesAsync();
                var generoRetorno = _mapper.Map<LeituraGeneroDTO>(genero);

                return CreatedAtAction(nameof(RecuperarGeneroPorId), new { id = generoRetorno.Id }, generoRetorno);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possível cadastrar o gênero: {e.Message}");
                return StatusCode(500, $"Erro interno ao adicionar o gênero: {e.Message}");
            }
        }

        [HttpGet]
        public async Task<IEnumerable<LeituraGeneroDTO>> RecuperarGeneros()
        {
            var listaGeneros = await _context.Generos.ToListAsync();
            return _mapper.Map<List<LeituraGeneroDTO>>(listaGeneros);
        }

        // GET: Buscar Gênero por ID (Necessário para o retorno do POST funcionar)
        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarGeneroPorId(int id)
        {
            var genero = await _context.Generos.FirstOrDefaultAsync(g => g.Id == id);
            if (genero == null) return NotFound();

            var generoDto = _mapper.Map<LeituraGeneroDTO>(genero);
            return Ok(generoDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarGenero(int id)
        {
            var genero = await _context.Generos.FirstOrDefaultAsync(g => g.Id == id);
            if (genero == null) return NotFound();

            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}