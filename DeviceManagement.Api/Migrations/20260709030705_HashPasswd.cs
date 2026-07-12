using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeviceManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class HashPasswd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ogfduxgaoWgAXcmymvqCG.Xd3ud9PrXV.0eFiCMDuu2Kx25cnf/pu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$ogfduxgaoWgAXcmymvqCG.Xd3ud9PrXV.0eFiCMDuu2Kx25cnf/pu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "123456");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "123456");
        }
    }
}
