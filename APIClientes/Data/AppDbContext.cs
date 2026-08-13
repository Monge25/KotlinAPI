using APIClientes.Models;
using Microsoft.EntityFrameworkCore;

namespace APIClientes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rutina> Rutinas { get; set; }
        public DbSet<Ejercicio> Ejercicios { get; set; }

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

            // ── Rutina ───────────────────────────────────────────
            modelBuilder.Entity<Rutina>(e =>
            {
                e.ToTable("rutinas");
                e.HasKey(r => r.Id);
                e.Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();
                e.Property(r => r.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                e.Property(r => r.Nivel)
                    .HasColumnName("nivel")
                    .HasColumnType("smallint")
                    .HasConversion<short>();
                e.Property(r => r.Objetivo)
                    .HasColumnName("objetivo")
                    .HasColumnType("smallint")
                    .HasConversion<short>();
                e.Property(r => r.FechaCreacion).HasColumnName("fecha_creacion");
                e.Property(r => r.EsActivo).HasColumnName("es_activo");

                // Relación: una rutina tiene muchos ejercicios
                e.HasMany(r => r.Ejercicios)
                    .WithOne(ej => ej.Rutina)
                    .HasForeignKey(ej => ej.RutinaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Ejercicio ─────────────────────────────────────────
            modelBuilder.Entity<Ejercicio>(e =>
            {
                e.ToTable("ejercicios");
                e.HasKey(ej => ej.Id);
                e.Property(ej => ej.Id).HasColumnName("id").ValueGeneratedOnAdd();
                e.Property(ej => ej.RutinaId).HasColumnName("rutina_id");
                e.Property(ej => ej.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                e.Property(ej => ej.Series).HasColumnName("series");
                e.Property(ej => ej.Repeticiones).HasColumnName("repeticiones");
                e.Property(ej => ej.Peso).HasColumnName("peso").HasColumnType("decimal(8,2)");
                e.Property(ej => ej.Descanso).HasColumnName("descanso");
                e.Property(ej => ej.Notas).HasColumnName("notas");
                e.Property(ej => ej.FechaCreacion).HasColumnName("fecha_creacion");
                e.Property(ej => ej.EsActivo).HasColumnName("es_activo");
            });
        }
    }
}
