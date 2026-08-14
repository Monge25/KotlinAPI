using APIClientes.Enums;

namespace APIClientes.Models
{
    public class SesionEntrenamiento
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int RutinaId { get; set; }
        public DateTime FechaProgramada { get; set; }
        public string HoraProgramada { get; set; } = "";
        public EstadoSesionEnum Estado { get; set; } = EstadoSesionEnum.PROGRAMADA;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int PorcentajeCompletado { get; set; } = 0;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public Usuario Usuario { get; set; } = null!;
        public Rutina Rutina { get; set; } = null!;
        public ICollection<EjercicioCompletado> EjerciciosCompletados { get; set; }
            = new List<EjercicioCompletado>();
    }
}
