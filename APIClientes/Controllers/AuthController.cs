using APIClientes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AuthController(AppDbContext db) => _db = db;

        public record LoginRequest(string Username, string Password);

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var user = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Username == req.Username && u.Password == req.Password);
            return user is null ? Unauthorized("Credenciales inválidas") : Ok(new { mensaje = "OK", userId = user.Id });
        }
    }
}
