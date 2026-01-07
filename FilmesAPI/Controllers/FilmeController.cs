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
    public async Task<IActionResult> AdicionarFilme([FromBody] CriacaoFilmeDTO novoFilmeDTO)
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
        await _context.SaveChangesAsync();

        var filmeCompleto = await _context.Filmes
            .Include(f => f.FilmesGenero)
            .ThenInclude(fg => fg.Genero)
            .FirstOrDefaultAsync(f => f.Id == filme.Id);
        LeituraFilmeDTO filmeDTO = _mapper.Map<LeituraFilmeDTO>(filmeCompleto);
        return CreatedAtAction(nameof(RecuperarFilme), new { id = filme.Id }, filmeDTO);
    }

[HttpGet]
public async Task<IEnumerable<LeituraFilmeDTO>> RecuperarFilme(
    [FromQuery] int skip = 0, 
    [FromQuery] int take = 10,
    [FromQuery] string? nomeFilme = null,
    [FromQuery] string? nomeGenero = null, // Filtro por Gênero
    [FromQuery] string? nomeAtor = null,   // Filtro por Ator
    [FromQuery] int? anoInicial = null,    // Filtro de intervalo de anos
    [FromQuery] int? anoFinal = null,
    [FromQuery] string? ordenarPor = null  // Ordenação dinâmica
)
{
    // 1. Prepara a query base carregando os relacionamentos
    // O AsQueryable() é importante para não ir ao banco ainda
    var query = _context.Filmes
        .Include(f => f.FilmesGenero)
            .ThenInclude(fg => fg.Genero)
        .Include(f => f.ElencoFilme)
            .ThenInclude(ef => ef.Ator)
        .AsQueryable();

    // 2. Aplica Filtros 

    // Filtro por Nome do Filme
    if (!string.IsNullOrEmpty(nomeFilme))
    {
        query = query.Where(f => f.Nome.Contains(nomeFilme));
    }

    // Filtro por Gênero
    if (!string.IsNullOrEmpty(nomeGenero))
    {
        query = query.Where(f => f.FilmesGenero.Any(fg => fg.Genero.Nome == nomeGenero));
    }

    // Filtro por Ator
    if (!string.IsNullOrEmpty(nomeAtor))
    {
        query = query.Where(f => f.ElencoFilme.Any(ef => ef.Ator.PrimeiroNome.Contains(nomeAtor) || ef.Ator.UltimoNome.Contains(nomeAtor)));
    }

    // Filtros de Ano
    if (anoInicial.HasValue)
    {
        query = query.Where(f => f.Ano >= anoInicial.Value);
    }
    if (anoFinal.HasValue)
    {
        query = query.Where(f => f.Ano <= anoFinal.Value);
    }

    // 3. Aplica Ordenação 
    if (!string.IsNullOrEmpty(ordenarPor))
    {
        switch (ordenarPor.ToLower())
        {
            case "ano":
                query = query.OrderBy(f => f.Ano); // Crescente
                break;
            case "anodesc":
                query = query.OrderByDescending(f => f.Ano); // Decrescente
                break;
            case "duracao":
                query = query.OrderBy(f => f.Duracao);
                break;
            default:
                query = query.OrderBy(f => f.Nome);
                break;
        }
    }

    // 4. Paginação e Execução
    // O SQL só é gerado e executado AGORA, no ToListAsync()
    var filmes = await query
        .Skip(skip)
        .Take(take)
        .ToListAsync();

    // 5. Mapeamento para DTO
    return _mapper.Map<List<LeituraFilmeDTO>>(filmes);
}

    [HttpGet("{id}")]
    public async Task<IActionResult> RecuperarFilme(int id)
    {

        var filmeEncontrado = await _context.Filmes
            .Include(f => f.FilmesGenero)
            .ThenInclude(fg => fg.Genero)
            .FirstOrDefaultAsync(f => f.Id == id);
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
    public async Task<IActionResult> EditarFilme(int id, [FromBody] AtualizacaoFilmeDTO filmeDTO)
    {
        Filme filme = _mapper.Map<Filme>(filmeDTO);

        try
        {
            Filme filmeEncontrado = await _context.Filmes
            .Include(f => f.FilmesGenero)
            .ThenInclude(fg => fg.Genero)
            .FirstOrDefaultAsync(f => f.Id == id);

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
                await _context.SaveChangesAsync();
                        var filmeCompleto = await _context.Filmes
                        .Include(f => f.FilmesGenero)
                        .ThenInclude(fg => fg.Genero)
                        .FirstOrDefaultAsync(f => f.Id == id);
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
    public async Task<IActionResult> ExcluirFilme(int id)
    {

        try
        {
            Filme filmeEncontrado = await _context.Filmes.FindAsync(id);
            if (filmeEncontrado != null)
            {
                // Busca e remoção dos vínculos com a tabela relacional FilmesGenero
                var vinculo = await _context.FilmesGenero
                    .Where(fg => fg.IdFilme == id)
                    .ToListAsync();
                _context.FilmesGenero.RemoveRange(vinculo);


                _context.Filmes.Remove(filmeEncontrado);
                await _context.SaveChangesAsync();
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


    // Buscas com filtro






}