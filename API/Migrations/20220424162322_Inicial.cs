using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "GVFlix");

            migrationBuilder.CreateTable(
                name: "Atores",
                schema: "GVFlix",
                columns: table => new
                {
                    CodigoAtor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atores", x => x.CodigoAtor);
                });

            migrationBuilder.CreateTable(
                name: "Filmes",
                schema: "GVFlix",
                columns: table => new
                {
                    CodigoFilme = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DataLancamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UrlImagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Excluido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filmes", x => x.CodigoFilme);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                schema: "GVFlix",
                columns: table => new
                {
                    CodigoUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Excluido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.CodigoUsuario);
                });

            migrationBuilder.CreateTable(
                name: "FilmesAtores",
                schema: "GVFlix",
                columns: table => new
                {
                    CodigoFilmeAtor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoFilme = table.Column<int>(type: "int", nullable: false),
                    CodigoAtor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmesAtores", x => x.CodigoFilmeAtor);
                    table.ForeignKey(
                        name: "FK_FilmesAtores_Atores_CodigoAtor",
                        column: x => x.CodigoAtor,
                        principalSchema: "GVFlix",
                        principalTable: "Atores",
                        principalColumn: "CodigoAtor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FilmesAtores_Filmes_CodigoFilme",
                        column: x => x.CodigoFilme,
                        principalSchema: "GVFlix",
                        principalTable: "Filmes",
                        principalColumn: "CodigoFilme",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Historicos",
                schema: "GVFlix",
                columns: table => new
                {
                    CodigoHistorico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoUsuario = table.Column<int>(type: "int", nullable: false),
                    CodigoFilme = table.Column<int>(type: "int", nullable: false),
                    DataReproducao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historicos", x => x.CodigoHistorico);
                    table.ForeignKey(
                        name: "FK_Historicos_Filmes_CodigoFilme",
                        column: x => x.CodigoFilme,
                        principalSchema: "GVFlix",
                        principalTable: "Filmes",
                        principalColumn: "CodigoFilme",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Historicos_Usuarios_CodigoUsuario",
                        column: x => x.CodigoUsuario,
                        principalSchema: "GVFlix",
                        principalTable: "Usuarios",
                        principalColumn: "CodigoUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "GVFlix",
                table: "Atores",
                columns: new[] { "CodigoAtor", "DataNascimento", "Nome" },
                values: new object[] { 1, new DateTime(2022, 4, 24, 13, 23, 22, 52, DateTimeKind.Local).AddTicks(8281), "Ator1" });

            migrationBuilder.InsertData(
                schema: "GVFlix",
                table: "Filmes",
                columns: new[] { "CodigoFilme", "DataLancamento", "Descricao", "Excluido", "Titulo", "UrlImagem" },
                values: new object[] { 1, new DateTime(2022, 4, 24, 13, 23, 22, 48, DateTimeKind.Local).AddTicks(8306), "DescricaoFilme1", false, "Filme1", "https://media-exp1.licdn.com/dms/image/C4E0BAQFFxN23wILsEw/company-logo_200_200/0/1648731270519?e=2147483647&v=beta&t=_KEZSCAMqGkm_X_09BocNUTs8hQRcr_9Ub2yFl0Epig" });

            migrationBuilder.InsertData(
                schema: "GVFlix",
                table: "Usuarios",
                columns: new[] { "CodigoUsuario", "DataNascimento", "Email", "Excluido", "Nome", "Role", "Senha" },
                values: new object[] { 1, new DateTime(2022, 4, 24, 13, 23, 22, 64, DateTimeKind.Local).AddTicks(5646), "admin@gvflix.com", false, "Admin", 1, "admin123" });

            migrationBuilder.CreateIndex(
                name: "IX_FilmesAtores_CodigoAtor",
                schema: "GVFlix",
                table: "FilmesAtores",
                column: "CodigoAtor");

            migrationBuilder.CreateIndex(
                name: "IX_FilmesAtores_CodigoFilme",
                schema: "GVFlix",
                table: "FilmesAtores",
                column: "CodigoFilme");

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_CodigoFilme",
                schema: "GVFlix",
                table: "Historicos",
                column: "CodigoFilme");

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_CodigoUsuario",
                schema: "GVFlix",
                table: "Historicos",
                column: "CodigoUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmesAtores",
                schema: "GVFlix");

            migrationBuilder.DropTable(
                name: "Historicos",
                schema: "GVFlix");

            migrationBuilder.DropTable(
                name: "Atores",
                schema: "GVFlix");

            migrationBuilder.DropTable(
                name: "Filmes",
                schema: "GVFlix");

            migrationBuilder.DropTable(
                name: "Usuarios",
                schema: "GVFlix");
        }
    }
}
