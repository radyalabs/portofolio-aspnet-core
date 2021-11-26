using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portofolio_aspnet_core.Migrations
{
    public partial class CreateInitialTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id_category = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id_category);
                });

            migrationBuilder.CreateTable(
                name: "member",
                columns: table => new
                {
                    id_member = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    profile_picture = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_member", x => x.id_member);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    id_project = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.id_project);
                });

            migrationBuilder.CreateTable(
                name: "project_category",
                columns: table => new
                {
                    id_project_category = table.Column<string>(type: "text", nullable: false),
                    id_project = table.Column<string>(type: "text", nullable: false),
                    id_category = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_category", x => x.id_project_category);
                    table.ForeignKey(
                        name: "FK_project_category_category_id_category",
                        column: x => x.id_category,
                        principalTable: "category",
                        principalColumn: "id_category",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_category_project_id_project",
                        column: x => x.id_project,
                        principalTable: "project",
                        principalColumn: "id_project",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_image",
                columns: table => new
                {
                    id_project_image = table.Column<string>(type: "text", nullable: false),
                    id_project = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_image", x => x.id_project_image);
                    table.ForeignKey(
                        name: "FK_project_image_project_id_project",
                        column: x => x.id_project,
                        principalTable: "project",
                        principalColumn: "id_project",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_member",
                columns: table => new
                {
                    id_project_member = table.Column<string>(type: "text", nullable: false),
                    id_project = table.Column<string>(type: "text", nullable: false),
                    id_member = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_member", x => x.id_project_member);
                    table.ForeignKey(
                        name: "FK_project_member_member_id_member",
                        column: x => x.id_member,
                        principalTable: "member",
                        principalColumn: "id_member",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_member_project_id_project",
                        column: x => x.id_project,
                        principalTable: "project",
                        principalColumn: "id_project",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_project_category_id_category",
                table: "project_category",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_project_category_id_project",
                table: "project_category",
                column: "id_project");

            migrationBuilder.CreateIndex(
                name: "IX_project_image_id_project",
                table: "project_image",
                column: "id_project");

            migrationBuilder.CreateIndex(
                name: "IX_project_member_id_member",
                table: "project_member",
                column: "id_member");

            migrationBuilder.CreateIndex(
                name: "IX_project_member_id_project",
                table: "project_member",
                column: "id_project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project_category");

            migrationBuilder.DropTable(
                name: "project_image");

            migrationBuilder.DropTable(
                name: "project_member");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "member");

            migrationBuilder.DropTable(
                name: "project");
        }
    }
}
