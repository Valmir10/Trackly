using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackly.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingsAndDecisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OriginMeetingId",
                table: "Tickets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meetings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Decisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decisions_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Decisions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_OriginMeetingId",
                table: "Tickets",
                column: "OriginMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_MeetingId",
                table: "Decisions",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_ProjectId",
                table: "Decisions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_TenantId",
                table: "Decisions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ProjectId_ScheduledAt",
                table: "Meetings",
                columns: new[] { "ProjectId", "ScheduledAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_TenantId",
                table: "Meetings",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Meetings_OriginMeetingId",
                table: "Tickets",
                column: "OriginMeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Meetings_OriginMeetingId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Decisions");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_OriginMeetingId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "OriginMeetingId",
                table: "Tickets");
        }
    }
}
