using System;
using HotelUp.Cleaning.Persistence.Const;
using Microsoft.EntityFrameworkCore.Migrations;
using TaskStatus = HotelUp.Cleaning.Persistence.Const.TaskStatus;

#nullable disable

namespace HotelUp.Cleaning.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cleaning");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:cleaning_type", "cyclic,on_demand")
                .Annotation("Npgsql:Enum:task_status", "pending,in_progress,done");

            migrationBuilder.CreateTable(
                name: "Cleaners",
                schema: "cleaning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cleaners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CleaningTasks",
                schema: "cleaning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RealisationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoomNumber = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<TaskStatus>(type: "cleaning.task_status", nullable: false),
                    CleaningType = table.Column<CleaningType>(type: "cleaning.cleaning_type", nullable: false),
                    CleanerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CleaningTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CleaningTasks_Cleaners_CleanerId",
                        column: x => x.CleanerId,
                        principalSchema: "cleaning",
                        principalTable: "Cleaners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTasks_CleanerId",
                schema: "cleaning",
                table: "CleaningTasks",
                column: "CleanerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CleaningTasks",
                schema: "cleaning");

            migrationBuilder.DropTable(
                name: "Cleaners",
                schema: "cleaning");
        }
    }
}
