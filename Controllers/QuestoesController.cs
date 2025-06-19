using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
public class QuestoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuestoesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Questao>>> GetQuestoes()
    {
        return await _context.Questoes.ToListAsync();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Questao>>> GetByMateriaAndAno([FromQuery] string materia, [FromQuery] int ano)
    {
        if (string.IsNullOrWhiteSpace(materia))
            return BadRequest("Parâmetro 'materia' é obrigatório.");

        var questaos = await _context.Questoes
            .Where(a => a.Materia.ToLower() == materia.ToLower() && a.Ano == ano)
            .ToListAsync();

        return questaos.Any() ? Ok(questaos) : NotFound("Nenhuma questao encontrada com os filtros fornecidos.");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Questao>> GetQuestao(int id)
    {
        var questao = await _context.Questoes.FindAsync(id);

        if (questao == null)
            return NotFound();

        return questao;
    }

    [HttpPost]
    public async Task<ActionResult<Questao>> CreateQuestao(Questao questao)
    {
        questao.CreationDate = DateTime.UtcNow;
        questao.UpdateDate = DateTime.UtcNow;

        _context.Questoes.Add(questao);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetQuestao), new { id = questao.Id }, questao);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestao(int id, Questao questao)
    {
        if (id != questao.Id)
            return BadRequest();

        var existing = await _context.Questoes.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Numero= questao.Numero;
        existing.Topico = questao.Topico;
        existing.Materia = questao.Materia;
        existing.Ano = questao.Ano;
        existing.AlternativaA = questao.AlternativaA;
        existing.AlternativaB = questao.AlternativaB;
        existing.AlternativaC = questao.AlternativaC;
        existing.AlternativaD = questao.AlternativaD;
        existing.AlternativaE = questao.AlternativaE;
        existing.Pergunta = questao.Pergunta;
        existing.Resposta = questao.Resposta;
        existing.UpdateDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestao(int id)
    {
        var questao = await _context.Questoes.FindAsync(id);
        if (questao == null)
            return NotFound();

        var comentarios = await _context.ComentariosXQuestoes
           .Where(c => c.IdQuestao == id)
           .ToListAsync();

        _context.ComentariosXQuestoes.RemoveRange(comentarios);

        _context.Questoes.Remove(questao);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("comentario")]
    public async Task<IActionResult> CriarComentario(ComentarioQuestao comentario)
    {
        comentario.CreationDate = DateTime.UtcNow;
        comentario.UpdateDate = DateTime.UtcNow;

        _context.ComentariosXQuestoes.Add(comentario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CriarComentario), new { idApostila = comentario.IdQuestao }, comentario);
    }


    [HttpGet("{idQuestao}/comentarios")]
    public async Task<ActionResult<IEnumerable<ComentarioQuestaoDto>>> GetComentariosPorQuestao(int idQuestao)
    {
        var comentarios = await _context.ComentariosXQuestoes
            .Where(c => c.IdQuestao == idQuestao)
            .Join(
                _context.Usuarios,
                comentario => comentario.IdUsuario,
                usuario => usuario.Id,
                (comentario, usuario) => new ComentarioQuestaoDto
                {
                    Id = comentario.Id,
                    IdQuestao = comentario.IdQuestao,
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
