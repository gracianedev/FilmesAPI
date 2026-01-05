using Microsoft.AspNetCore.Mvc;
using FilmesAPI.Models;
using FilmesAPI.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FilmesAPI.DTO;
using FilmesAPI.Profiles;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    // Variável para guardar a conexão
    private FilmeContext _context;

    //Variável para guarda o mapeamento
    private IMapper _mapper;

    // "Injeção de Dependência"
    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionarFilme([FromBody] CriacaoFilmeDTO novoFilmeDTO)
    {
        Filme filme = _mapper.Map<Filme>(novoFilmeDTO);
        filme.FilmesGenero = new List<FilmesGenero>();
        if (novoFilmeDTO.GeneroIds != null)
        {
            foreach (var idGenero in novoFilmeDTO.GeneroIds)
            {
                // Para cada ID, cria um "vínculo" na tabela de junção
                var vinculo = new FilmesGenero
                {
                    IdGenero = idGenero
                };
                filme.FilmesGenero.Add(vinculo);
            }
        }
        _context.Filmes.Add(filme);
        _context.SaveChanges();

        var filmeCompleto = _context.Filmes
            .Include(f => f.FilmesGenero)
            .ThenInclude(fg => fg.Genero)
            .FirstOrDefault(f => f.Id == filme.Id);
        LeituraFilmeDTO filmeDTO = _mapper.Map<LeituraFilmeDTO>(filmeCompleto);
        return CreatedAtAction(nameof(RecuperarFilme), new { id = filme.Id }, filmeDTO);
    }

    [HttpGet]
    public IEnumerable<LeituraFilmeDTO> RecuperarFilmes([FromQuery] int skip = 0)
    {
        var listaFilmes = _context.Filmes
            .Include(f => f.FilmesGenero)
            .ThenInclude(fg => fg.Genero).Skip(skip).ToList();


        return _mapper.Map<List<LeituraFilmeDTO>>(listaFilmes);
    }


    [HttpGet("{id}")]
    public IActionResult RecuperarFilme(int id)
    {

        var filmeEncontrado = _context.Filmes
            .Include(f => f.FilmesGenero)
            .ThenInclude(fg => fg.Genero)
            .FirstOrDefault(f => f.Id == id);
        if (filmeEncontrado != null)
        {
            LeituraFilmeDTO filmeDTO = _mapper.Map<LeituraFilmeDTO>(filmeEncontrado);
            return Ok(filmeDTO);
        }
        else
        {

            return NotFound();
        }
    }

    [HttpPut("{id}")]
    public IActionResult EditarFilme(int id, [FromBody] AtualizacaoFilmeDTO filmeDTO)
    {
        Filme filme = _mapper.Map<Filme>(filmeDTO);

        try
        {
            Filme filmeEncontrado = _context.Filmes
            .Include(f => f.FilmesGenero)
            .ThenInclude(fg => fg.Genero)
            .FirstOrDefault(f => f.Id == id);

            if (filmeEncontrado != null)
            {
                // Remoção dos vínculos com Genero antes de atualizar
                var vinculo = filmeEncontrado.FilmesGenero;
                _context.FilmesGenero.RemoveRange(vinculo);

                _mapper.Map(filmeDTO, filmeEncontrado);
                if (filmeDTO.GeneroIds != null)
                {
                    foreach (var idGenero in filmeDTO.GeneroIds)
                    {
                        var novoVinculo = new FilmesGenero
                        {
                            IdGenero = idGenero,
                            IdFilme = id
                        };
                    _context.FilmesGenero.Add(novoVinculo);
                    }
                }
                _context.SaveChanges();
                        var filmeCompleto = _context.Filmes
                        .Include(f => f.FilmesGenero)
                        .ThenInclude(fg => fg.Genero)
                        .FirstOrDefault(f => f.Id == id);
                        LeituraFilmeDTO leituraFilmeDTO = _mapper.Map<LeituraFilmeDTO>(filmeCompleto);
                        return Ok(leituraFilmeDTO);
                    
                }
                else
                {
                    return NotFound();
                }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException?.Message ?? e.Message);
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
                // Busca e remoção dos vínculos com a tabela relacional FilmesGenero
                var vinculo = _context.FilmesGenero
                    .Where(fg => fg.IdFilme == id)
                    .ToList();
                _context.FilmesGenero.RemoveRange(vinculo);


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
            Console.WriteLine(e.InnerException?.Message ?? e.Message);
            Console.WriteLine($"Filme id {id} não encontrado, não foi possível excluir o cadastro.");
            return StatusCode(500);
        }
    }
}