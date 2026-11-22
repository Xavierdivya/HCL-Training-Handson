using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enrollora.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFilteredUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Enrollments_StudentId_CourseId_Status",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_CancellationRequests_EnrollmentId_Status",
                table: "CancellationRequests");

            migrationBuilder.CreateIndex(
                name: "UX_Enrollments_Student_Course_Active",
                table: "Enrollments",
                columns: new[] { "StudentId", "CourseId" },
                unique: true,
                filter: "[Status] = 0");

            migrationBuilder.CreateIndex(
                name: "UX_CancellationRequests_Enrollment_Pending",
                table: "CancellationRequests",
                column: "EnrollmentId",
                unique: true,
                filter: "[Status] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Enrollments_Student_Course_Active",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "UX_CancellationRequests_Enrollment_Pending",
                table: "CancellationRequests");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_CourseId_Status",
                table: "Enrollments",
                columns: new[] { "StudentId", "CourseId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRequests_EnrollmentId_Status",
                table: "CancellationRequests",
                columns: new[] { "EnrollmentId", "Status" });
        }
    }
}
