using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobJump.Migrations
{
    /// <inheritdoc />
    public partial class ResumeAi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "JobApplications",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "MatchScore",
                table: "JobApplications",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MissingSkills",
                table: "JobApplications",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchScore",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "MissingSkills",
                table: "JobApplications");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "JobApplications",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
