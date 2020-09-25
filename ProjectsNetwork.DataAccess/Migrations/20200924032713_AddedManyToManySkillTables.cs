using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectsNetwork.DataAccess.Migrations
{
    public partial class AddedManyToManySkillTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterestedInProject_Projects_ProjectId",
                table: "InterestedInProject");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestedInProject_AspNetUsers_UserId",
                table: "InterestedInProject");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_AspNetUsers_ApplicationUserId",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Projects_ProjectId",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Skill_ApplicationUserId",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Skill_ProjectId",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterestedInProject",
                table: "InterestedInProject");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Skill");

            migrationBuilder.RenameTable(
                name: "Skill",
                newName: "Skills");

            migrationBuilder.RenameTable(
                name: "InterestedInProject",
                newName: "InterestedInProjects");

            migrationBuilder.RenameIndex(
                name: "IX_InterestedInProject_ProjectId",
                table: "InterestedInProjects",
                newName: "IX_InterestedInProjects_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterestedInProjects",
                table: "InterestedInProjects",
                columns: new[] { "UserId", "ProjectId" });

            migrationBuilder.CreateTable(
                name: "ProjectSkill",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false),
                    SkillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSkill", x => new { x.ProjectId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_ProjectSkill_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkill",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    SkillId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkill", x => new { x.ApplicationUserId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_UserSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkill_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSkill_SkillId",
                table: "ProjectSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_SkillId",
                table: "UserSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserId",
                table: "UserSkill",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedInProjects_Projects_ProjectId",
                table: "InterestedInProjects",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedInProjects_AspNetUsers_UserId",
                table: "InterestedInProjects",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterestedInProjects_Projects_ProjectId",
                table: "InterestedInProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestedInProjects_AspNetUsers_UserId",
                table: "InterestedInProjects");

            migrationBuilder.DropTable(
                name: "ProjectSkill");

            migrationBuilder.DropTable(
                name: "UserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterestedInProjects",
                table: "InterestedInProjects");

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "Skill");

            migrationBuilder.RenameTable(
                name: "InterestedInProjects",
                newName: "InterestedInProject");

            migrationBuilder.RenameIndex(
                name: "IX_InterestedInProjects_ProjectId",
                table: "InterestedInProject",
                newName: "IX_InterestedInProject_ProjectId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Skill",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Skill",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterestedInProject",
                table: "InterestedInProject",
                columns: new[] { "UserId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_Skill_ApplicationUserId",
                table: "Skill",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_ProjectId",
                table: "Skill",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedInProject_Projects_ProjectId",
                table: "InterestedInProject",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedInProject_AspNetUsers_UserId",
                table: "InterestedInProject",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_AspNetUsers_ApplicationUserId",
                table: "Skill",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Projects_ProjectId",
                table: "Skill",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
