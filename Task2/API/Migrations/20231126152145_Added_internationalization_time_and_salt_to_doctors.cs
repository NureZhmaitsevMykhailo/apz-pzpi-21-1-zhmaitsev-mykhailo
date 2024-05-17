using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OncoBound.API.Migrations
{
    /// <inheritdoc />
    public partial class Added_internationalization_time_and_salt_to_doctors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DatePrescribed",
                table: "Prescriptions",
                newName: "DatePrescribedUTC");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Medications",
                newName: "StartTimeUTC");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Medications",
                newName: "EndTimeUTC");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "MedicationLogs",
                newName: "TimestampUTC");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Doctors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "DatePrescribedUTC",
                table: "Prescriptions",
                newName: "DatePrescribed");

            migrationBuilder.RenameColumn(
                name: "StartTimeUTC",
                table: "Medications",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "EndTimeUTC",
                table: "Medications",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "TimestampUTC",
                table: "MedicationLogs",
                newName: "Timestamp");
        }
    }
}
