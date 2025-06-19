using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _context.Usuarios
            .Where(u => u.UsuarioLogin == request.Username)
            .FirstOrDefaultAsync();

        if (usuario == null)
            return Unauthorized("Usuário ou senha inválidos");

        if (!VerifyPassword(request.Password, usuario.Senha, usuario.Salt))
            return Unauthorized("Usuário ou senha inválidos");

        var token = GenerateJwtToken(usuario);

        return Ok(new { token, usuario.Role, usuario.AnoLetivo, usuario.Id });
    }

    private bool VerifyPassword(string inputPassword, string storedHash, string storedSalt)
    {
        return HashingHelper.VerifyPassword(inputPassword, storedHash, storedSalt);
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioLogin),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, usuario.Role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AtividadeExtensionistaIIIRA4641284@2025"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
