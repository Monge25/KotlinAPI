using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIClientes.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "usuarios");

            migrationBuilder.RenameTable(
                name: "Clientes",
                newName: "clientes");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "usuarios",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "usuarios",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "clientes",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Edad",
                table: "clientes",
                newName: "edad");

            migrationBuilder.RenameColumn(
                name: "Clave",
                table: "clientes",
                newName: "clave");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "clientes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "FechaNacimiento",
                table: "clientes",
                newName: "fecha_nacimiento");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "clientes",
                newName: "fecha_creacion");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "usuarios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "es_activo",
                table: "usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_creacion",
                table: "usuarios",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "nombre",
                table: "usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "rol",
                table: "usuarios",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "clientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "clave",
                table: "clientes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usuarios",
                table: "usuarios",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_clientes",
                table: "clientes",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clientes_clave",
                table: "clientes",
                column: "clave",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_usuarios",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_email",
                table: "usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_clientes",
                table: "clientes");

            migrationBuilder.DropIndex(
                name: "IX_clientes_clave",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "email",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "es_activo",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "fecha_creacion",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "nombre",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "rol",
                table: "usuarios");

            migrationBuilder.RenameTable(
                name: "usuarios",
                newName: "Usuarios");

            migrationBuilder.RenameTable(
                name: "clientes",
                newName: "Clientes");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Usuarios",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Usuarios",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Clientes",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "edad",
                table: "Clientes",
                newName: "Edad");

            migrationBuilder.RenameColumn(
                name: "clave",
                table: "Clientes",
                newName: "Clave");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Clientes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "fecha_nacimiento",
                table: "Clientes",
                newName: "FechaNacimiento");

            migrationBuilder.RenameColumn(
                name: "fecha_creacion",
                table: "Clientes",
                newName: "FechaCreacion");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Clientes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Clave",
                table: "Clientes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Id");
        }
    }
}
