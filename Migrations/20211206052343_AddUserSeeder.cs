using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portofolio_aspnet_core.Migrations
{
    public partial class AddUserSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id_user", "email", "password", "username" },
                values: new object[] { "2058b90b-fcb9-494b-972f-6afaf0173c83", "yanuar.wanda2@gmail.com", "admin123", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "id_user",
                keyValue: "2058b90b-fcb9-494b-972f-6afaf0173c83");
        }
    }
}
