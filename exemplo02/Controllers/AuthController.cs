using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exemplo02.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest login)
    {
        if (login.Email == "admin@brincafacil.com" &&
            login.Senha == "123456")
        {
            var token = GerarToken("Administrador");

            return Ok(new
            {
                token = token,
                usuario = login.Email,
                perfil = "Administrador"
            });
        }

        if (login.Email == "cliente@brincafacil.com" &&
            login.Senha == "123456")
        {
            var token = GerarToken("Cliente");

            return Ok(new
            {
                token = token,
                usuario = login.Email,
                perfil = "Cliente"
            });
        }

        return Unauthorized(new
        {
            mensagem = "E-mail ou senha inválidos."
        });
    }

    private string GerarToken(string perfil)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, perfil),
            new Claim(ClaimTypes.Role, perfil)
        };

        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                "chave-secreta-do-projeto-brinca-facil-2026"
            )
        );

        var credenciais = new SigningCredentials(
            chave,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
}