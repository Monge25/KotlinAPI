using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace APIClientes.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSesiones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sesiones_entrenamiento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    rutina_id = table.Column<int>(type: "integer", nullable: false),
                    fecha_programada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    hora_programada = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    estado = table.Column<short>(type: "smallint", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fecha_fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    porcentaje_completado = table.Column<int>(type: "integer", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sesiones_entrenamiento", x => x.id);
                    table.ForeignKey(
                        name: "FK_sesiones_entrenamiento_rutinas_rutina_id",
                        column: x => x.rutina_id,
                        principalTable: "rutinas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sesiones_entrenamiento_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ejercicios_completados",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sesion_id = table.Column<int>(type: "integer", nullable: false),
                    ejercicio_id = table.Column<int>(type: "integer", nullable: false),
                    completado = table.Column<bool>(type: "boolean", nullable: false),
                    series_completadas = table.Column<int>(type: "integer", nullable: true),
                    repeticiones_completadas = table.Column<int>(type: "integer", nullable: true),
                    peso_usado = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    notas = table.Column<string>(type: "text", nullable: true),
                    fecha_completado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ejercicios_completados", x => x.id);
                    table.ForeignKey(
                        name: "FK_ejercicios_completados_ejercicios_ejercicio_id",
                        column: x => x.ejercicio_id,
                        principalTable: "ejercicios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ejercicios_completados_sesiones_entrenamiento_sesion_id",
                        column: x => x.sesion_id,
                        principalTable: "sesiones_entrenamiento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ejercicios_completados_ejercicio_id",
                table: "ejercicios_completados",
                column: "ejercicio_id");

            migrationBuilder.CreateIndex(
                name: "IX_ejercicios_completados_sesion_id",
                table: "ejercicios_completados",
                column: "sesion_id");

            migrationBuilder.CreateIndex(
                name: "IX_sesiones_entrenamiento_rutina_id",
                table: "sesiones_entrenamiento",
                column: "rutina_id");

            migrationBuilder.CreateIndex(
                name: "IX_sesiones_entrenamiento_usuario_id",
                table: "sesiones_entrenamiento",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ejercicios_completados");

            migrationBuilder.DropTable(
                name: "sesiones_entrenamiento");
        }
    }
}
