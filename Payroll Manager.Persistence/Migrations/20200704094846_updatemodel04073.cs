using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll_Manager.Persistence.Migrations
{
    public partial class updatemodel04073 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlaces_Shops_EventId",
                table: "CurrentWorkPlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shops",
                table: "Shops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentWorkPlaces",
                table: "CurrentWorkPlaces");

            migrationBuilder.RenameTable(
                name: "Shops",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "CurrentWorkPlaces",
                newName: "EventImages");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlaces_EventId",
                table: "EventImages",
                newName: "IX_EventImages_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventImages",
                table: "EventImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventImages_Events_EventId",
                table: "EventImages",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventImages_Events_EventId",
                table: "EventImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventImages",
                table: "EventImages");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Shops");

            migrationBuilder.RenameTable(
                name: "EventImages",
                newName: "CurrentWorkPlaces");

            migrationBuilder.RenameIndex(
                name: "IX_EventImages_EventId",
                table: "CurrentWorkPlaces",
                newName: "IX_CurrentWorkPlaces_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shops",
                table: "Shops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentWorkPlaces",
                table: "CurrentWorkPlaces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlaces_Shops_EventId",
                table: "CurrentWorkPlaces",
                column: "EventId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
