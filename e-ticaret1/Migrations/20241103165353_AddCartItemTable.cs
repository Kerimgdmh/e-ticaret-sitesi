using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_ticaret1.Migrations
{
    public partial class AddCartItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false), // ProductId alanını ekleyin
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    // İstenirse ProductId'ye foreign key kısıtlaması ekleyebilirsiniz
                    // table.ForeignKey(
                    //     name: "FK_CartItems_Products_ProductId",
                    //     column: x => x.ProductId,
                    //     principalTable: "Products",
                    //     principalColumn: "Id",
                    //     onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");
        }
    }
}
