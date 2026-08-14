using APIClientes.Data;
using APIClientes.DTOs;
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
    public class SesionesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SesionesController(AppDbContext db)
        {
            _db = db;
        }

        // ── POST /api/sesiones ────────────────────────────────────
        // Programa una sesión (desde ProgramarEntrenamientoActivity)
        [HttpPost]
        public async Task<IActionResult> Programar([FromBody] ProgramarSesionRequest req)
        {
            var rutinaExiste = await _db.Rutinas.AnyAsync(r => r.Id == req.RutinaId);
            if (!rutinaExiste)
                return NotFound(new { mensaje = "Rutina no encontrada." });

            var usuarioExiste = await _db.Usuarios.AnyAsync(u => u.Id == req.UsuarioId);
            if (!usuarioExiste)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            var sesion = new SesionEntrenamiento
            {
                UsuarioId = req.UsuarioId,
                RutinaId = req.RutinaId,
                FechaProgramada = req.FechaProgramada.ToUniversalTime(),
                HoraProgramada = req.HoraProgramada,
                Estado = EstadoSesionEnum.PROGRAMADA,
                FechaCreacion = DateTime.UtcNow
            };

            _db.Sesiones.Add(sesion);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId),
                new { id = sesion.Id }, new { sesion.Id, mensaje = "Sesión programada." });
        }

        // ── GET /api/sesiones/usuario/{usuarioId} ─────────────────
        // Lista todas las sesiones de un usuario (para el calendario)
        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<IActionResult> GetPorUsuario(int usuarioId)
        {
            var sesiones = await _db.Sesiones
                .Where(s => s.UsuarioId == usuarioId)
                .Include(s => s.Rutina)
                .Include(s => s.EjerciciosCompletados)
                    .ThenInclude(ec => ec.Ejercicio)
                .OrderByDescending(s => s.FechaProgramada)
                .Select(s => MapearSesion(s))
                .ToListAsync();

            return Ok(sesiones);
        }

        // ── GET /api/sesiones/{id} ────────────────────────────────
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var sesion = await _db.Sesiones
                .Include(s => s.Rutina)
                .Include(s => s.EjerciciosCompletados)
                    .ThenInclude(ec => ec.Ejercicio)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sesion == null)
                return NotFound(new { mensaje = "Sesión no encontrada." });

            return Ok(MapearSesion(sesion));
        }

        // ── PATCH /api/sesiones/{id}/iniciar ──────────────────────
        // Inicia la sesión y genera los EjercicioCompletado
        [HttpPatch("{id:int}/iniciar")]
        public async Task<IActionResult> Iniciar(int id)
        {
            var sesion = await _db.Sesiones
                .Include(s => s.EjerciciosCompletados)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sesion == null)
                return NotFound(new { mensaje = "Sesión no encontrada." });

            if (sesion.Estado != EstadoSesionEnum.PROGRAMADA)
                return BadRequest(new { mensaje = "La sesión ya fue iniciada." });

            // Cargar los ejercicios activos de la rutina
            var ejercicios = await _db.Ejercicios
                .Where(ej => ej.RutinaId == sesion.RutinaId && ej.EsActivo)
                .ToListAsync();

            // Crear un EjercicioCompletado por cada ejercicio
            foreach (var ej in ejercicios)
            {
                sesion.EjerciciosCompletados.Add(new EjercicioCompletado
                {
                    SesionId = sesion.Id,
                    EjercicioId = ej.Id,
                    Completado = false
                });
            }

            sesion.Estado = EstadoSesionEnum.EN_PROGRESO;
            sesion.FechaInicio = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new { mensaje = "Sesión iniciada.", sesionId = sesion.Id });
        }

        // ── PATCH /api/sesiones/{id}/ejercicios/{ejCompletadoId}/completar
        // Marca un ejercicio como completado
        [HttpPatch("{id:int}/ejercicios/{ejCompletadoId:int}/completar")]
        public async Task<IActionResult> CompletarEjercicio(
            int id, int ejCompletadoId,
            [FromBody] CompletarEjercicioRequest req)
        {
            var ec = await _db.EjerciciosCompletados
                .FirstOrDefaultAsync(e => e.Id == ejCompletadoId && e.SesionId == id);

            if (ec == null)
                return NotFound(new { mensaje = "Ejercicio no encontrado en esta sesión." });

            ec.Completado = true;
            ec.SeriesCompletadas = req.SeriesCompletadas;
            ec.RepeticionesCompletadas = req.RepeticionesCompletadas;
            ec.PesoUsado = req.PesoUsado;
            ec.Notas = req.Notas;
            ec.FechaCompletado = DateTime.UtcNow;

            // Recalcular porcentaje de la sesión
            var sesion = await _db.Sesiones
                .Include(s => s.EjerciciosCompletados)
                .FirstAsync(s => s.Id == id);

            var total = sesion.EjerciciosCompletados.Count;
            var completados = sesion.EjerciciosCompletados.Count(e => e.Completado);
            sesion.PorcentajeCompletado = total > 0
                ? (int)Math.Round((double)completados / total * 100)
                : 0;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Ejercicio completado.",
                porcentajeCompletado = sesion.PorcentajeCompletado
            });
        }

        // ── PATCH /api/sesiones/{id}/finalizar ────────────────────
        // Cierra la sesión y determina el estado final
        [HttpPatch("{id:int}/finalizar")]
        public async Task<IActionResult> Finalizar(int id)
        {
            var sesion = await _db.Sesiones
                .Include(s => s.EjerciciosCompletados)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sesion == null)
                return NotFound(new { mensaje = "Sesión no encontrada." });

            if (sesion.Estado != EstadoSesionEnum.EN_PROGRESO)
                return BadRequest(new { mensaje = "La sesión no está en progreso." });

            var total = sesion.EjerciciosCompletados.Count;
            var completados = sesion.EjerciciosCompletados.Count(e => e.Completado);

            sesion.Estado = completados == 0 ? EstadoSesionEnum.ABANDONADA
                          : completados == total ? EstadoSesionEnum.COMPLETADA
                          : EstadoSesionEnum.PARCIAL;

            sesion.PorcentajeCompletado = total > 0
                ? (int)Math.Round((double)completados / total * 100)
                : 0;

            sesion.FechaFin = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Sesión finalizada.",
                estado = (short)sesion.Estado,
                porcentajeCompletado = sesion.PorcentajeCompletado
            });
        }

        // ── Mapper ────────────────────────────────────────────────
        private static SesionResponse MapearSesion(SesionEntrenamiento s) => new(
            s.Id,
            s.UsuarioId,
            s.RutinaId,
            s.Rutina?.Nombre ?? "",
            s.FechaProgramada,
            s.HoraProgramada,
            (short)s.Estado,
            s.Estado.ToString(),
            s.PorcentajeCompletado,
            s.FechaInicio,
            s.FechaFin,
            s.EjerciciosCompletados.Select(ec => new EjercicioCompletadoResponse(
                ec.Id,
                ec.EjercicioId,
                ec.Ejercicio?.Nombre ?? "",
                ec.Ejercicio?.Series ?? 0,
                ec.Ejercicio?.Repeticiones ?? 0,
                ec.Completado,
                ec.SeriesCompletadas,
                ec.RepeticionesCompletadas,
                ec.PesoUsado,
                ec.Notas,
                ec.FechaCompletado
            )).ToList()
        );
    }
}