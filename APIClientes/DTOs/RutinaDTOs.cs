namespace APIClientes.Dtos
{
    // ── Request para crear/actualizar rutina con sus ejercicios ───
    public record CrearRutinaRequest(
        string Nombre,
        short Nivel,
        short Objetivo,
        List<CrearEjercicioRequest> Ejercicios
    );

    public record ActualizarRutinaRequest(
        string Nombre,
        short Nivel,
        short Objetivo
    );

    // ── Request para crear/actualizar ejercicio individual ────────
    public record CrearEjercicioRequest(
        string Nombre,
        int Series,
        int Repeticiones,
        decimal? Peso,
        int Descanso,
        string? Notas
    );

    public record ActualizarEjercicioRequest(
        string Nombre,
        int Series,
        int Repeticiones,
        decimal? Peso,
        int Descanso,
        string? Notas
    );

    // ── Responses ─────────────────────────────────────────────────
    public record EjercicioResponse(
        int Id,
        int RutinaId,
        string Nombre,
        int Series,
        int Repeticiones,
        decimal? Peso,
        int Descanso,
        string? Notas,
        bool EsActivo,
        DateTime FechaCreacion
    );

    public record RutinaResponse(
        int Id,
        string Nombre,
        short Nivel,
        short Objetivo,
        bool EsActivo,
        DateTime FechaCreacion,
        List<EjercicioResponse> Ejercicios
    );
}
