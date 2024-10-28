using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiServiceVetBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorWorkshift_DoctorSchedules_schedule_idScheduleId",
                table: "DoctorWorkshift");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorWorkshift_Users_doctor_idUserId",
                table: "DoctorWorkshift");

            migrationBuilder.DropIndex(
                name: "IX_DoctorWorkshift_doctor_idUserId",
                table: "DoctorWorkshift");

            migrationBuilder.DropIndex(
                name: "IX_DoctorWorkshift_schedule_idScheduleId",
                table: "DoctorWorkshift");

            migrationBuilder.DropColumn(
                name: "doctor_idUserId",
                table: "DoctorWorkshift");

            migrationBuilder.DropColumn(
                name: "schedule_idScheduleId",
                table: "DoctorWorkshift");

            migrationBuilder.AddColumn<bool>(
                name: "IsHomeVisit",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DoctorsServices",
                columns: table => new
                {
                    doctor_id = table.Column<int>(type: "int", nullable: false),
                    service_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorsServices", x => x.doctor_id);
                    table.ForeignKey(
                        name: "FK_DoctorsServices_Services_service_id",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "service_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorsServices_Users_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorWorkshift_doctor_id",
                table: "DoctorWorkshift",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorWorkshift_schedule_id",
                table: "DoctorWorkshift",
                column: "schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorsServices_service_id",
                table: "DoctorsServices",
                column: "service_id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorWorkshift_DoctorSchedules_schedule_id",
                table: "DoctorWorkshift",
                column: "schedule_id",
                principalTable: "DoctorSchedules",
                principalColumn: "schedule_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorWorkshift_Users_doctor_id",
                table: "DoctorWorkshift",
                column: "doctor_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorWorkshift_DoctorSchedules_schedule_id",
                table: "DoctorWorkshift");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorWorkshift_Users_doctor_id",
                table: "DoctorWorkshift");

            migrationBuilder.DropTable(
                name: "DoctorsServices");

            migrationBuilder.DropIndex(
                name: "IX_DoctorWorkshift_doctor_id",
                table: "DoctorWorkshift");

            migrationBuilder.DropIndex(
                name: "IX_DoctorWorkshift_schedule_id",
                table: "DoctorWorkshift");

            migrationBuilder.DropColumn(
                name: "IsHomeVisit",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "doctor_idUserId",
                table: "DoctorWorkshift",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "schedule_idScheduleId",
                table: "DoctorWorkshift",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorWorkshift_doctor_idUserId",
                table: "DoctorWorkshift",
                column: "doctor_idUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorWorkshift_schedule_idScheduleId",
                table: "DoctorWorkshift",
                column: "schedule_idScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorWorkshift_DoctorSchedules_schedule_idScheduleId",
                table: "DoctorWorkshift",
                column: "schedule_idScheduleId",
                principalTable: "DoctorSchedules",
                principalColumn: "schedule_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorWorkshift_Users_doctor_idUserId",
                table: "DoctorWorkshift",
                column: "doctor_idUserId",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
