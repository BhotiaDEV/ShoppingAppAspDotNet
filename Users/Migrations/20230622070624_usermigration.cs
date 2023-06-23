using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Users.Migrations
{
    /// <inheritdoc />
    public partial class usermigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3bfaa88-9c47-4b65-a0db-fe719c7d62b8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd0294aa-26a0-4d7e-b6b5-d6308128e441");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2239eb99-31d8-422d-9dfd-d7a93cb39ddb", "1", "Admin", "ADMIN" },
                    { "9be06aeb-4e34-4adf-8aae-e7c2e3721e09", "2", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2239eb99-31d8-422d-9dfd-d7a93cb39ddb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9be06aeb-4e34-4adf-8aae-e7c2e3721e09");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c3bfaa88-9c47-4b65-a0db-fe719c7d62b8", "1", "Admin", "ADMIN" },
                    { "cd0294aa-26a0-4d7e-b6b5-d6308128e441", "2", "User", "USER" }
                });
        }
    }
}
