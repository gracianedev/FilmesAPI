using Microsoft.AspNetCore.Mvc;
using FilmesAPI.Models;
using FilmesAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    // Variável para guardar a conexão
    private FilmeContext _context;

    // "Injeção de Dependência"
    public FilmeController(FilmeContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AdicionarFilme([FromBody] Filme filme)
    {
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperarFilme), new { id = filme.Id }, filme);
    }

    [HttpGet]
   public IEnumerable<Filme> RecuperarFilmes([FromQuery] int skip = 0)
    {
        return _context.Filmes.Skip(skip).ToList();
    }


    [HttpGet("{id}")]
    public IActionResult RecuperarFilme(int id)
    {

        Filme filmeEncontrado = _context.Filmes.Find(id);
        if (filmeEncontrado != null)
        {
            return Ok(filmeEncontrado);
        }
        else
        {

            return NotFound();
        }
    }

    [HttpPut("{id}")]
    public IActionResult EditarFilme(int id, [FromBody] Filme filme)
    {
        if (filme.Id == id && filme.Id !=0)
        {
            return BadRequest("O ID da URL diverge do ID do corpo da requisição.");
        }

        try
        {
            Filme filmeEncontrado = _context.Filmes.Find(id);
            if (filmeEncontrado != null)
            {
                filmeEncontrado.Nome = filme.Nome;
                filmeEncontrado.Ano = filme.Ano;
                filmeEncontrado.Duracao = filme.Duracao;
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception e)
        {   
            Console.WriteLine(e.Message);
            return StatusCode(500, "Erro interno ao atualizar o filme.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult ExcluirFilme(int id)
    {
        try
        {
            Filme filmeEncontrado = _context.Filmes.Find(id);
            if (filmeEncontrado != null)
            {
                _context.Filmes.Remove(filmeEncontrado);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Filme id {id} não encontrado, não foi possível excluir o cadastro.");
            return StatusCode(500);
        }
    }
}