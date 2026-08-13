namespace APIClientes.Models
{
    public class Ejercicio
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EsActivo { get; set; }
        public int RutinaId { get; set; }
        public string Nombre { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public decimal? Peso { get; set; }
        public int Descanso { get; set; }
        public string? Notas { get; set; }

        // Navegación
        public Rutina Rutina { get; set; } = null!;
    }
}
