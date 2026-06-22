using APIClientes.Data;
using APIClientes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ClientesController(AppDbContext db) => _db = db;

        // GET api/clientes
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Clientes.ToListAsync());

        // GET api/clientes/ByClave/C001
        [HttpGet("ByClave/{clave}")]
        public async Task<IActionResult> GetByClave(string clave)
        {
            var c = await _db.Clientes.FirstOrDefaultAsync(x => x.Clave == clave);
            return c is null ? NotFound() : Ok(c);
        }

        // POST api/clientes
        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            _db.Clientes.Add(cliente);
            await _db.SaveChangesAsync();
            return Ok(cliente);
        }

        // PUT api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cliente cliente)
        {
            var existing = await _db.Clientes.FindAsync(id);
            if (existing is null) return NotFound();
            existing.Clave = cliente.Clave;
            existing.Nombre = cliente.Nombre;
            existing.Email = cliente.Email;
            existing.Telefono = cliente.Telefono;
            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        // DELETE api/clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Clientes.FindAsync(id);
            if (c is null) return NotFound();
            _db.Clientes.Remove(c);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
