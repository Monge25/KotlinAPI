namespace APIClientes.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Clave { get; set; } = "";
        public string Nombre { get; set; } = "";
        public int Edad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
