using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "UpdateReturnDate",
                table: "Loans",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateReturnDate",
                table: "Loans");
        }
    }
}
