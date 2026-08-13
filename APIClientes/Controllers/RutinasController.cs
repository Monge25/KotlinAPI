using APIClientes.Data;
using APIClientes.Dtos;
using APIClientes.Enums;
using APIClientes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RutinasController : ControllerBase
    {
        private readonly AppDbContext _db;

        public RutinasController(AppDbContext db)
        {
            _db = db;
        }

        // ── GET /api/rutinas ──────────────────────────────────────
        // Lista todas las rutinas activas con sus ejercicios
        [HttpGet]
        public async Task<IActionResult> GetTodas()
        {
            var rutinas = await _db.Rutinas
                .Where(r => r.EsActivo)
                .Include(r => r.Ejercicios.Where(e => e.EsActivo))
                .OrderByDescending(r => r.FechaCreacion)
                .Select(r => MapearRutina(r))
                .ToListAsync();

            return Ok(rutinas);
        }

        // ── GET /api/rutinas/{id} ─────────────────────────────────
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var rutina = await _db.Rutinas
                .Include(r => r.Ejercicios.Where(e => e.EsActivo))
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rutina == null)
                return NotFound(new { mensaje = $"Rutina {id} no encontrada." });

            return Ok(MapearRutina(rutina));
        }

        // ── POST /api/rutinas ─────────────────────────────────────
        // Crea rutina y sus ejercicios en una sola petición
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearRutinaRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Nombre))
                return BadRequest(new { mensaje = "El nombre es requerido." });

            var rutina = new Rutina
            {
                Nombre = req.Nombre.Trim(),
                Nivel = (NivelEnum)req.Nivel,
                Objetivo = (ObjetivoEnum)req.Objetivo,
                FechaCreacion = DateTime.UtcNow,
                EsActivo = true,
                Ejercicios = req.Ejercicios.Select(e => new Ejercicio
                {
                    Nombre = e.Nombre.Trim(),
                    Series = e.Series,
                    Repeticiones = e.Repeticiones,
                    Peso = e.Peso,
                    Descanso = e.Descanso,
                    Notas = e.Notas,
                    FechaCreacion = DateTime.UtcNow,
                    EsActivo = true
                }).ToList()
            };

            _db.Rutinas.Add(rutina);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId),
                new { id = rutina.Id }, MapearRutina(rutina));
        }

        // ── PUT /api/rutinas/{id} ─────────────────────────────────
        // Actualiza solo los datos de la rutina (sin tocar ejercicios)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarRutinaRequest req)
        {
            var rutina = await _db.Rutinas
                .Include(r => r.Ejercicios)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rutina == null)
                return NotFound(new { mensaje = $"Rutina {id} no encontrada." });

            rutina.Nombre = req.Nombre.Trim();
            rutina.Nivel = (NivelEnum)req.Nivel;
            rutina.Objetivo = (ObjetivoEnum)req.Objetivo;

            await _db.SaveChangesAsync();
            return Ok(MapearRutina(rutina));
        }

        // ── PATCH /api/rutinas/{id}/desactivar ────────────────────
        // Desactiva la rutina Y todos sus ejercicios (soft delete)
        [HttpPatch("{id:int}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var rutina = await _db.Rutinas
                .Include(r => r.Ejercicios)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rutina == null)
                return NotFound(new { mensaje = $"Rutina {id} no encontrada." });

            rutina.EsActivo = false;
            foreach (var e in rutina.Ejercicios)
                e.EsActivo = false;

            await _db.SaveChangesAsync();
            return Ok(new { mensaje = "Rutina desactivada correctamente." });
        }

        // ── POST /api/rutinas/{id}/ejercicios ─────────────────────
        // Agrega un ejercicio a una rutina existente
        [HttpPost("{id:int}/ejercicios")]
        public async Task<IActionResult> AgregarEjercicio(
            int id, [FromBody] CrearEjercicioRequest req)
        {
            var rutina = await _db.Rutinas.FindAsync(id);
            if (rutina == null)
                return NotFound(new { mensaje = $"Rutina {id} no encontrada." });

            var ejercicio = new Ejercicio
            {
                RutinaId = id,
                Nombre = req.Nombre.Trim(),
                Series = req.Series,
                Repeticiones = req.Repeticiones,
                Peso = req.Peso,
                Descanso = req.Descanso,
                Notas = req.Notas,
                FechaCreacion = DateTime.UtcNow,
                EsActivo = true
            };

            _db.Ejercicios.Add(ejercicio);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId),
                new { id = rutina.Id }, MapearEjercicio(ejercicio));
        }

        // ── PUT /api/rutinas/{id}/ejercicios/{ejercicioId} ────────
        // Actualiza un ejercicio específico
        [HttpPut("{id:int}/ejercicios/{ejercicioId:int}")]
        public async Task<IActionResult> ActualizarEjercicio(
            int id, int ejercicioId, [FromBody] ActualizarEjercicioRequest req)
        {
            var ejercicio = await _db.Ejercicios
                .FirstOrDefaultAsync(e => e.Id == ejercicioId && e.RutinaId == id);

            if (ejercicio == null)
                return NotFound(new { mensaje = "Ejercicio no encontrado." });

            ejercicio.Nombre = req.Nombre.Trim();
            ejercicio.Series = req.Series;
            ejercicio.Repeticiones = req.Repeticiones;
            ejercicio.Peso = req.Peso;
            ejercicio.Descanso = req.Descanso;
            ejercicio.Notas = req.Notas;

            await _db.SaveChangesAsync();
            return Ok(MapearEjercicio(ejercicio));
        }

        // ── PATCH /api/rutinas/{id}/ejercicios/{ejercicioId}/desactivar
        // Desactiva solo un ejercicio
        [HttpPatch("{id:int}/ejercicios/{ejercicioId:int}/desactivar")]
        public async Task<IActionResult> DesactivarEjercicio(int id, int ejercicioId)
        {
            var ejercicio = await _db.Ejercicios
                .FirstOrDefaultAsync(e => e.Id == ejercicioId && e.RutinaId == id);

            if (ejercicio == null)
                return NotFound(new { mensaje = "Ejercicio no encontrado." });

            ejercicio.EsActivo = false;
            await _db.SaveChangesAsync();

            return Ok(new { mensaje = "Ejercicio desactivado correctamente." });
        }

        // ── Mappers privados ──────────────────────────────────────
        private static RutinaResponse MapearRutina(Rutina r) => new(
            r.Id,
            r.Nombre,
            (short)r.Nivel,
            (short)r.Objetivo,
            r.EsActivo,
            r.FechaCreacion,
            r.Ejercicios.Select(MapearEjercicio).ToList()
        );

        private static EjercicioResponse MapearEjercicio(Ejercicio e) => new(
            e.Id,
            e.RutinaId,
            e.Nombre,
            e.Series,
            e.Repeticiones,
            e.Peso,
            e.Descanso,
            e.Notas,
            e.EsActivo,
            e.FechaCreacion
        );
    }
}
