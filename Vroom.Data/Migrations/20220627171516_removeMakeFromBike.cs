using Microsoft.EntityFrameworkCore.Migrations;

namespace Vroom.Data.Migrations
{
    public partial class removeMakeFromBike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bikes_Makes_MakeId",
                table: "Bikes");

            migrationBuilder.DropIndex(
                name: "IX_Bikes_MakeId",
                table: "Bikes");

            migrationBuilder.DropColumn(
                name: "MakeId",
                table: "Bikes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MakeId",
                table: "Bikes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_MakeId",
                table: "Bikes",
                column: "MakeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bikes_Makes_MakeId",
                table: "Bikes",
                column: "MakeId",
                principalTable: "Makes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
