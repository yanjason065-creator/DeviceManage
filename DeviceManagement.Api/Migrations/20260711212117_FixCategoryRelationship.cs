using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeviceManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixCategoryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Categories_CategoryId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Categories_CategoryId1",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_CategoryId1",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Devices");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Categories_CategoryId",
                table: "Devices",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Categories_CategoryId",
                table: "Devices");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId1",
                table: "Devices",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CategoryId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CategoryId1",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CategoryId1",
                table: "Devices",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Categories_CategoryId",
                table: "Devices",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Categories_CategoryId1",
                table: "Devices",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
