using APIClientes.Models;
using Microsoft.EntityFrameworkCore;

namespace APIClientes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── Cliente ───────────────────────────────────────────
            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("clientes");
                e.HasKey(c => c.Id);
                e.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
                e.Property(c => c.Clave).HasColumnName("clave").HasMaxLength(20).IsRequired();
                e.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                e.Property(c => c.Edad).HasColumnName("edad");
                e.Property(c => c.FechaNacimiento).HasColumnName("fecha_nacimiento");
                e.Property(c => c.FechaCreacion).HasColumnName("fecha_creacion");
                e.HasIndex(c => c.Clave).IsUnique();
            });

            // ── Usuario ───────────────────────────────────────────
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("usuarios");
                e.HasKey(u => u.Id);
                e.Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
                e.Property(u => u.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                e.Property(u => u.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
                e.Property(u => u.Password).HasColumnName("password").IsRequired();
                e.Property(u => u.Rol)
                    .HasColumnName("rol")
                    .HasColumnType("smallint")
                    .HasConversion<short>();
                e.Property(u => u.FechaCreacion).HasColumnName("fecha_creacion");
                e.Property(u => u.EsActivo).HasColumnName("es_activo");
                e.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
