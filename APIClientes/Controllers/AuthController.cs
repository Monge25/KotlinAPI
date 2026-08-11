using APIClientes.Data;
using APIClientes.Enums;
using APIClientes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public record LoginRequest(string Email, string Password);
        public record LoginResponse(string Token, int UsuarioId, string Nombre, string Email, string Rol);

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { mensaje = "Email y contraseña son requeridos." });

            // Buscar por email
            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Email == req.Email);

            // Verificar existencia, estado activo y contraseña
            if (usuario is null || !usuario.EsActivo)
                return Unauthorized(new { mensaje = "Credenciales inválidas." });

            if (!BCrypt.Net.BCrypt.Verify(req.Password, usuario.Password))
                return Unauthorized(new { mensaje = "Credenciales inválidas." });

            var token = GenerarToken(usuario);

            return Ok(new LoginResponse(
                token,
                usuario.Id,
                usuario.Nombre,
                usuario.Email,
                usuario.Rol.ToString()   // "ADMINISTRADOR" o "OPERADOR"
            ));
        }

        // POST api/auth/register  (opcional, para crear usuarios desde la API)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { mensaje = "Todos los campos son requeridos." });

            bool emailExiste = await _db.Usuarios.AnyAsync(u => u.Email == req.Email);
            if (emailExiste)
                return Conflict(new { mensaje = "Ya existe un usuario con ese email." });

            var nuevo = new Usuario
            {
                Nombre = req.Nombre.Trim(),
                Email = req.Email.Trim().ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Rol = req.Rol,
                FechaCreacion = DateTime.UtcNow,
                EsActivo = true
            };

            _db.Usuarios.Add(nuevo);
            await _db.SaveChangesAsync();

            return Created($"api/auth/{nuevo.Id}", new { mensaje = "Usuario creado.", nuevo.Id });
        }

        private string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString()),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // DTO de registro
    public record RegisterRequest(
        string Nombre,
        string Email,
        string Password,
        RolEnum Rol = RolEnum.OPERADOR   // rol por defecto
    );
}
