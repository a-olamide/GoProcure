using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoProcure.IAM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("13829f1c-043b-4143-912c-b94e4741e9b8"), true, "Procurement" },
                    { new Guid("63b03900-0481-4196-b57e-fe01daea2b56"), true, "HR" },
                    { new Guid("9e7de21f-26ad-43ae-8c36-4c6b9b7c1d59"), true, "Admin" },
                    { new Guid("ad21d88e-66f0-4e10-b752-68a96f554497"), true, "IT" },
                    { new Guid("bf3da4a6-314c-4b73-a0c8-7e8bb758b570"), true, "Finance" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
