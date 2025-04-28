using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevXpert.Academy.Financeiro.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatriculaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Vencimento = table.Column<string>(type: "TEXT", nullable: true),
                    CcvCvc = table.Column<string>(type: "TEXT", nullable: true),
                    Situacao = table.Column<int>(type: "INTEGER", nullable: true),
                    DataHoraUltimoProcessamento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Mensagem = table.Column<string>(type: "TEXT", nullable: true),
                    DataHoraCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataHoraAlteracao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PagamentosTransacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "TEXT", nullable: false),
                    Situacao = table.Column<int>(type: "INTEGER", nullable: false),
                    DataHoraProcessamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Mensagem = table.Column<string>(type: "TEXT", nullable: true),
                    PagamentoId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentosTransacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentosTransacoes_Pagamentos_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PagamentosTransacoes_PagamentoId",
                table: "PagamentosTransacoes",
                column: "PagamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagamentosTransacoes");

            migrationBuilder.DropTable(
                name: "Pagamentos");
        }
    }
}
