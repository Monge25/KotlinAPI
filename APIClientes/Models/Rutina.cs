using APIClientes.Enums;

namespace APIClientes.Models
{
    public class Rutina
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EsActivo { get; set; }
        public string Nombre { get; set; }
        public NivelEnum Nivel { get; set; }
        public ObjetivoEnum Objetivo { get; set; }

        // Navegación
        public ICollection<Ejercicio> Ejercicios { get; set; } = new List<Ejercicio>();
    }
}
