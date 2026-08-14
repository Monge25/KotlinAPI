namespace APIClientes.Models
{
    public class EjercicioCompletado
    {
        public int Id { get; set; }
        public int SesionId { get; set; }
        public int EjercicioId { get; set; }
        public bool Completado { get; set; } = false;
        public int? SeriesCompletadas { get; set; }
        public int? RepeticionesCompletadas { get; set; }
        public decimal? PesoUsado { get; set; }
        public string? Notas { get; set; }
        public DateTime? FechaCompletado { get; set; }

        // Navegación
        public SesionEntrenamiento Sesion { get; set; } = null!;
        public Ejercicio Ejercicio { get; set; } = null!;
    }
}
