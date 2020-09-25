using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectsNetwork.DataAccess.Migrations
{
    public partial class FixForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_AspNetUsers_UserId",
                table: "UserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserId",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserSkill");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserSkill",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                columns: new[] { "UserId", "SkillId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_AspNetUsers_UserId",
                table: "UserSkill",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_AspNetUsers_UserId",
                table: "UserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserSkill",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserSkill",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                columns: new[] { "ApplicationUserId", "SkillId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserId",
                table: "UserSkill",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_AspNetUsers_UserId",
                table: "UserSkill",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
