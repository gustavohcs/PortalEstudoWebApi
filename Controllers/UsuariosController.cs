using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return NotFound();

        return usuario;
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
    {
        usuario.CreationDate = DateTime.UtcNow;
        usuario.UpdateDate = DateTime.UtcNow;

        var (senhaHash, salt) = HashingHelper.HashPassword(usuario.Senha);
        usuario.Senha = senhaHash;  // Salvar apenas o hash
        usuario.Salt = salt;  // Salvar o salt

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
    {
        if (id != usuario.Id)
            return BadRequest();

        var existing = await _context.Usuarios.FindAsync(id);
        if (existing == null)
            return NotFound();

        var (senhaHash, salt) = HashingHelper.HashPassword(usuario.Senha);

        existing.UsuarioLogin = usuario.UsuarioLogin;
        existing.Senha = senhaHash;
        existing.Salt = salt;
        existing.Role = usuario.Role;
        existing.UpdateDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
