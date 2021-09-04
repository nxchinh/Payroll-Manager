using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll_Manager.Persistence.Migrations
{
    public partial class updatemodel06072020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventImages_Events_EventId",
                table: "EventImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "SuKien");

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianBatDau",
                table: "SuKien",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuKien",
                table: "SuKien",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventImages_SuKien_EventId",
                table: "EventImages",
                column: "EventId",
                principalTable: "SuKien",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventImages_SuKien_EventId",
                table: "EventImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuKien",
                table: "SuKien");

            migrationBuilder.DropColumn(
                name: "ThoiGianBatDau",
                table: "SuKien");

            migrationBuilder.RenameTable(
                name: "SuKien",
                newName: "Events");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventImages_Events_EventId",
                table: "EventImages",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
