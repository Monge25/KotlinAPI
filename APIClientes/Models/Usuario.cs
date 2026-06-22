namespace APIClientes.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";  // guarda hash en producción
    }
}
