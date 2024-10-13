using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPersonal.Migrations
{
    /// <inheritdoc />
    public partial class SyncWithDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadoBlog",
                columns: table => new
                {
                    CodigoEstadoBlog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EstadoBl__D825BC86D53230BF", x => x.CodigoEstadoBlog);
                });

            migrationBuilder.CreateTable(
                name: "EstadoUsuario",
                columns: table => new
                {
                    CodigoEstadoUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EstadoUs__293148A6FBAF246C", x => x.CodigoEstadoUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    CodigoUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Correo = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CodigoEstadoUsuario = table.Column<int>(type: "int", nullable: false),
                    FechaUltimoAcceso = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    TokenRecuperacion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__F0C18F582484D445", x => x.CodigoUsuario);
                    table.ForeignKey(
                        name: "FK__Usuario__CodigoE__2B3F6F97",
                        column: x => x.CodigoEstadoUsuario,
                        principalTable: "EstadoUsuario",
                        principalColumn: "CodigoEstadoUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    CodigoBlog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "smalldatetime", nullable: false, defaultValueSql: "(getdate())"),
                    FechaModificacion = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CodigoUsuario = table.Column<int>(type: "int", nullable: false),
                    CodigoEstadoBlog = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Blog__87433BD2B95583CB", x => x.CodigoBlog);
                    table.ForeignKey(
                        name: "FK__Blog__CodigoEsta__300424B4",
                        column: x => x.CodigoEstadoBlog,
                        principalTable: "EstadoBlog",
                        principalColumn: "CodigoEstadoBlog");
                    table.ForeignKey(
                        name: "FK__Blog__CodigoUsua__2F10007B",
                        column: x => x.CodigoUsuario,
                        principalTable: "Usuario",
                        principalColumn: "CodigoUsuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_CodigoEstadoBlog",
                table: "Blog",
                column: "CodigoEstadoBlog");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_CodigoUsuario",
                table: "Blog",
                column: "CodigoUsuario");

            migrationBuilder.CreateIndex(
                name: "UQ__EstadoBl__36DF552FB3F3FB09",
                table: "EstadoBlog",
                column: "Estado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__EstadoUs__36DF552F959EB7B1",
                table: "EstadoUsuario",
                column: "Estado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_CodigoEstadoUsuario",
                table: "Usuario",
                column: "CodigoEstadoUsuario");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuario__60695A19168961C8",
                table: "Usuario",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "EstadoBlog");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "EstadoUsuario");
        }
    }
}
