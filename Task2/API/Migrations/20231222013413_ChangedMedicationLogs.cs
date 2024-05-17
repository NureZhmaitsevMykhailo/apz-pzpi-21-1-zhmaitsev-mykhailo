using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OncoBound.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMedicationLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MedicationLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MedicationLogs_UserId",
                table: "MedicationLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationLogs_Users_UserId",
                table: "MedicationLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationLogs_Users_UserId",
                table: "MedicationLogs");

            migrationBuilder.DropIndex(
                name: "IX_MedicationLogs_UserId",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MedicationLogs");
        }
    }
}
