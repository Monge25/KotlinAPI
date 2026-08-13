using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace APIClientes.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRutinasYEjercicios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rutinas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    es_activo = table.Column<bool>(type: "boolean", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nivel = table.Column<short>(type: "smallint", nullable: false),
                    objetivo = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rutinas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ejercicios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    es_activo = table.Column<bool>(type: "boolean", nullable: false),
                    rutina_id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    series = table.Column<int>(type: "integer", nullable: false),
                    repeticiones = table.Column<int>(type: "integer", nullable: false),
                    peso = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    descanso = table.Column<int>(type: "integer", nullable: false),
                    notas = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ejercicios", x => x.id);
                    table.ForeignKey(
                        name: "FK_ejercicios_rutinas_rutina_id",
                        column: x => x.rutina_id,
                        principalTable: "rutinas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ejercicios_rutina_id",
                table: "ejercicios",
                column: "rutina_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ejercicios");

            migrationBuilder.DropTable(
                name: "rutinas");
        }
    }
}
