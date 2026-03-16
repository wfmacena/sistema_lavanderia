using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaLavanderia.Migrations
{
    /// <inheritdoc />
    public partial class FixItensPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "ItensPedido",
            columns: table => new
    {
        Id = table.Column<int>(nullable: false)
            .Annotation("Sqlite:Autoincrement", true),

            PedidoId = table.Column<int>(nullable: false),
            TipoPeca = table.Column<string>(nullable: false),
            Quantidade = table.Column<int>(nullable: false),
            ValorUnitario = table.Column<decimal>(nullable: false),
            Subtotal = table.Column<decimal>(nullable: false)
    },
            constraints: table =>
    {
            table.PrimaryKey("PK_ItensPedido", x => x.Id);

            table.ForeignKey(
            name: "FK_ItensPedido_Pedidos_PedidoId",
            column: x => x.PedidoId,
            principalTable: "Pedidos",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_PedidoId",
                table: "ItensPedido",
                column: "PedidoId");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Pedidos_PedidoId",
                table: "ItemPedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPedido",
                table: "ItemPedido");

            migrationBuilder.RenameTable(
                name: "ItemPedido",
                newName: "ItensPedido");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPedido_PedidoId",
                table: "ItensPedido",
                newName: "IX_ItensPedido_PedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensPedido",
                table: "ItensPedido",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedido_Pedidos_PedidoId",
                table: "ItensPedido",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "ItensPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedido_Pedidos_PedidoId",
                table: "ItensPedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensPedido",
                table: "ItensPedido");

            migrationBuilder.RenameTable(
                name: "ItensPedido",
                newName: "ItemPedido");

            migrationBuilder.RenameIndex(
                name: "IX_ItensPedido_PedidoId",
                table: "ItemPedido",
                newName: "IX_ItemPedido_PedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPedido",
                table: "ItemPedido",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Pedidos_PedidoId",
                table: "ItemPedido",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
