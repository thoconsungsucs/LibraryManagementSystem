using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changNaem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d3648d7-11bc-42c4-a948-d5db590368a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "182f20de-80c8-493d-b902-1a09ea1ec4b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "560a5558-47a1-43c8-8e85-661f4afce256");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Books",
                newName: "Category");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1de01430-16f8-4140-a0b9-1e0cc3cd7383", null, "Member", "MEMBER" },
                    { "71799274-461b-40f7-8580-fd871fe803a8", null, "Admin", "ADMIN" },
                    { "798f03b3-93b8-451c-8cb6-1516bd453273", null, "Librarian", "LIBRARIAN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1de01430-16f8-4140-a0b9-1e0cc3cd7383");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71799274-461b-40f7-8580-fd871fe803a8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "798f03b3-93b8-451c-8cb6-1516bd453273");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Books",
                newName: "CategoryName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d3648d7-11bc-42c4-a948-d5db590368a6", null, "Librarian", "LIBRARIAN" },
                    { "182f20de-80c8-493d-b902-1a09ea1ec4b9", null, "Member", "MEMBER" },
                    { "560a5558-47a1-43c8-8e85-661f4afce256", null, "Admin", "ADMIN" }
                });
        }
    }
}
