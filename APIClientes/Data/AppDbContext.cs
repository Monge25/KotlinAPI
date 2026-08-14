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
        public DbSet<SesionEntrenamiento> Sesiones { get; set; }
        public DbSet<EjercicioCompletado> EjerciciosCompletados { get; set; }

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
            // ── SesionEntrenamiento ───────────────────────────────
            modelBuilder.Entity<SesionEntrenamiento>(e =>
            {
                e.ToTable("sesiones_entrenamiento");
                e.HasKey(s => s.Id);
                e.Property(s => s.Id).HasColumnName("id").ValueGeneratedOnAdd();
                e.Property(s => s.UsuarioId).HasColumnName("usuario_id");
                e.Property(s => s.RutinaId).HasColumnName("rutina_id");
                e.Property(s => s.FechaProgramada).HasColumnName("fecha_programada");
                e.Property(s => s.HoraProgramada).HasColumnName("hora_programada").HasMaxLength(5);
                e.Property(s => s.Estado)
                    .HasColumnName("estado")
                    .HasColumnType("smallint")
                    .HasConversion<short>();
                e.Property(s => s.FechaInicio).HasColumnName("fecha_inicio");
                e.Property(s => s.FechaFin).HasColumnName("fecha_fin");
                e.Property(s => s.PorcentajeCompletado).HasColumnName("porcentaje_completado");
                e.Property(s => s.FechaCreacion).HasColumnName("fecha_creacion");

                // Relaciones
                e.HasOne(s => s.Usuario)
                    .WithMany()
                    .HasForeignKey(s => s.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(s => s.Rutina)
                    .WithMany()
                    .HasForeignKey(s => s.RutinaId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(s => s.EjerciciosCompletados)
                    .WithOne(ec => ec.Sesion)
                    .HasForeignKey(ec => ec.SesionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── EjercicioCompletado ───────────────────────────────
            modelBuilder.Entity<EjercicioCompletado>(e =>
            {
                e.ToTable("ejercicios_completados");
                e.HasKey(ec => ec.Id);
                e.Property(ec => ec.Id).HasColumnName("id").ValueGeneratedOnAdd();
                e.Property(ec => ec.SesionId).HasColumnName("sesion_id");
                e.Property(ec => ec.EjercicioId).HasColumnName("ejercicio_id");
                e.Property(ec => ec.Completado).HasColumnName("completado");
                e.Property(ec => ec.SeriesCompletadas).HasColumnName("series_completadas");
                e.Property(ec => ec.RepeticionesCompletadas).HasColumnName("repeticiones_completadas");
                e.Property(ec => ec.PesoUsado).HasColumnName("peso_usado").HasColumnType("decimal(8,2)");
                e.Property(ec => ec.Notas).HasColumnName("notas");
                e.Property(ec => ec.FechaCompletado).HasColumnName("fecha_completado");

                e.HasOne(ec => ec.Ejercicio)
                    .WithMany()
                    .HasForeignKey(ec => ec.EjercicioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
