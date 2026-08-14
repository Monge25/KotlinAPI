namespace APIClientes.DTOs
{
    public record ProgramarSesionRequest(
        int UsuarioId,
        int RutinaId,
        DateTime FechaProgramada,
        string HoraProgramada
    );

    public record CompletarEjercicioRequest(
        int? SeriesCompletadas,
        int? RepeticionesCompletadas,
        decimal? PesoUsado,
        string? Notas
    );

    public record EjercicioCompletadoResponse(
        int Id,
        int EjercicioId,
        string NombreEjercicio,
        int SeriesProgramadas,
        int RepeticionesProgramadas,
        bool Completado,
        int? SeriesCompletadas,
        int? RepeticionesCompletadas,
        decimal? PesoUsado,
        string? Notas,
        DateTime? FechaCompletado
    );

    public record SesionResponse(
        int Id,
        int UsuarioId,
        int RutinaId,
        string NombreRutina,
        DateTime FechaProgramada,
        string HoraProgramada,
        short Estado,
        string EstadoTexto,
        int PorcentajeCompletado,
        DateTime? FechaInicio,
        DateTime? FechaFin,
        List<EjercicioCompletadoResponse> Ejercicios
    );
}
