using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cine_NT1_Grupo2.Migrations
{
    public partial class CineBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pelicula",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 30, nullable: false),
                    Calificacion = table.Column<int>(nullable: false),
                    Genero = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pelicula", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entrada",
                columns: table => new
                {
                    EntradaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuncionId = table.Column<int>(nullable: true),
                    ClienteId = table.Column<int>(nullable: false),
                    AsientoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrada", x => x.EntradaId);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 20, nullable: false),
                    Apellido = table.Column<string>(maxLength: 20, nullable: false),
                    Mail = table.Column<string>(maxLength: 20, nullable: false),
                    pass = table.Column<string>(nullable: true),
                    Rol = table.Column<int>(nullable: false),
                    CineId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false),
                    ClienteActualid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cine_Cliente_ClienteActualid",
                        column: x => x.ClienteActualid,
                        principalTable: "Cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tarjeta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<long>(nullable: false),
                    FechaDeVencimiento = table.Column<DateTime>(nullable: false),
                    CodigoSeguridad = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 30, nullable: false),
                    ClienteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarjeta_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Funcion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPelicula = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Sala = table.Column<int>(nullable: false),
                    CineId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcion_Cine_CineId",
                        column: x => x.CineId,
                        principalTable: "Cine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Funcion_Pelicula_IdPelicula",
                        column: x => x.IdPelicula,
                        principalTable: "Pelicula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asiento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fila = table.Column<string>(maxLength: 1, nullable: false),
                    Numero = table.Column<int>(maxLength: 2, nullable: false),
                    ClienteId = table.Column<int>(nullable: false),
                    FuncionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asiento_Funcion_FuncionId",
                        column: x => x.FuncionId,
                        principalTable: "Funcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asiento_FuncionId",
                table: "Asiento",
                column: "FuncionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cine_ClienteActualid",
                table: "Cine",
                column: "ClienteActualid");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_CineId",
                table: "Cliente",
                column: "CineId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_AsientoId",
                table: "Entrada",
                column: "AsientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_ClienteId",
                table: "Entrada",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_FuncionId",
                table: "Entrada",
                column: "FuncionId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcion_CineId",
                table: "Funcion",
                column: "CineId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcion_IdPelicula",
                table: "Funcion",
                column: "IdPelicula");

            migrationBuilder.CreateIndex(
                name: "IX_Tarjeta_ClienteId",
                table: "Tarjeta",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrada_Funcion_FuncionId",
                table: "Entrada",
                column: "FuncionId",
                principalTable: "Funcion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrada_Cliente_ClienteId",
                table: "Entrada",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrada_Asiento_AsientoId",
                table: "Entrada",
                column: "AsientoId",
                principalTable: "Asiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Cine_CineId",
                table: "Cliente",
                column: "CineId",
                principalTable: "Cine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cine_Cliente_ClienteActualid",
                table: "Cine");

            migrationBuilder.DropTable(
                name: "Entrada");

            migrationBuilder.DropTable(
                name: "Tarjeta");

            migrationBuilder.DropTable(
                name: "Asiento");

            migrationBuilder.DropTable(
                name: "Funcion");

            migrationBuilder.DropTable(
                name: "Pelicula");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Cine");
        }
    }
}
