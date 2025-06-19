using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
public class ApostilasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApostilasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Apostila>>> GetApostilas()
    {
        return await _context.Apostilas.ToListAsync();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Apostila>>> GetByMateriaAndAno([FromQuery] string materia, [FromQuery] int ano)
    {
        if (string.IsNullOrWhiteSpace(materia))
            return BadRequest("Parâmetro 'materia' é obrigatório.");

        var apostilas = await _context.Apostilas
            .Where(a => a.Materia.ToLower() == materia.ToLower() && a.Ano == ano)
            .ToListAsync();

        return apostilas.Any() ? Ok(apostilas) : NotFound("Nenhuma apostila encontrada com os filtros fornecidos.");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Apostila>> GetApostila(int id)
    {
        var apostila = await _context.Apostilas.FindAsync(id);

        if (apostila == null)
            return NotFound();

        return apostila;
    }

    [HttpPost]
    public async Task<ActionResult<Apostila>> CreateApostila(Apostila apostila)
    {
        apostila.CreationDate = DateTime.UtcNow;
        apostila.UpdateDate = DateTime.UtcNow;

        _context.Apostilas.Add(apostila);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetApostila), new { id = apostila.Id }, apostila);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApostila(int id, Apostila apostila)
    {
        if (id != apostila.Id)
            return BadRequest();

        var existing = await _context.Apostilas.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Capitulo = apostila.Capitulo;
        existing.Topico = apostila.Topico;
        existing.Materia = apostila.Materia;
        existing.Ano = apostila.Ano;
        existing.Conteudo = apostila.Conteudo;
        existing.UpdateDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApostila(int id)
    {
        var apostila = await _context.Apostilas.FindAsync(id);
        if (apostila == null)
            return NotFound();

        var comentarios = await _context.ComentariosXApostilas
            .Where(c => c.IdApostila == id)
            .ToListAsync();

        _context.ComentariosXApostilas.RemoveRange(comentarios);
        _context.Apostilas.Remove(apostila);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("comentario")]
    public async Task<IActionResult> CriarComentario(ComentarioApostila comentario)
    {
        comentario.CreationDate = DateTime.UtcNow;
        comentario.UpdateDate = DateTime.UtcNow;

        _context.ComentariosXApostilas.Add(comentario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetComentariosPorApostila), new { idApostila = comentario.IdApostila }, comentario);
    }


    [HttpGet("{idApostila}/comentarios")]
    public async Task<ActionResult<IEnumerable<ComentarioApostilaDto>>> GetComentariosPorApostila(int idApostila)
    {
        var comentarios = await _context.ComentariosXApostilas
            .Where(c => c.IdApostila == idApostila)
            .Join(
                _context.Usuarios,
                comentario => comentario.IdUsuario,
                usuario => usuario.Id,
                (comentario, usuario) => new ComentarioApostilaDto
                {
                    Id = comentario.Id,
                    IdApostila = comentario.IdApostila,
                    IdUsuario = comentario.IdUsuario,
                    NomeUsuario = usuario.Nome,
                    RespostaParaIdComentario = comentario.RespostaParaIdComentario,
                    Mensagem = comentario.Mensagem,
                    CreationDate = comentario.CreationDate,
                    UpdateDate = comentario.UpdateDate
                }
            )
            .ToListAsync();

        return Ok(comentarios);
    }
}
