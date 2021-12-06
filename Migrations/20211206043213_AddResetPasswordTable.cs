using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portofolio_aspnet_core.Migrations
{
    public partial class AddResetPasswordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "reset_password",
                columns: table => new
                {
                    id_reset_password = table.Column<string>(type: "text", nullable: false),
                    id_user = table.Column<string>(type: "text", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    issued_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reset_password", x => x.id_reset_password);
                    table.ForeignKey(
                        name: "FK_reset_password_user_id_user",
                        column: x => x.id_user,
                        principalTable: "user",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reset_password_id_user",
                table: "reset_password",
                column: "id_user");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reset_password");

            migrationBuilder.DropColumn(
                name: "email",
                table: "user");
        }
    }
}
