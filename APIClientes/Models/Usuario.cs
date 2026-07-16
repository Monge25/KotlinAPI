using APIClientes.Enums;

namespace APIClientes.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";  // guarda hash en producción
        public RolEnum Rol { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EsActivo { get; set; }
    }
}
