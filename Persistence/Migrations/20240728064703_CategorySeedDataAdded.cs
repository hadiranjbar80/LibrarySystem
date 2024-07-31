using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CategorySeedDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("232330be-86a1-45a1-a714-e71d2f1a0b73"), "11", "ریاضیات" },
                    { new Guid("bcc429fa-5883-4c2e-95a3-138720f06048"), "12", "عمومی" },
                    { new Guid("c11ea2a9-0ab9-474f-b7f2-b069fb4d2164"), "10", "برنامه نویسی" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("232330be-86a1-45a1-a714-e71d2f1a0b73"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bcc429fa-5883-4c2e-95a3-138720f06048"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c11ea2a9-0ab9-474f-b7f2-b069fb4d2164"));
        }
    }
}
