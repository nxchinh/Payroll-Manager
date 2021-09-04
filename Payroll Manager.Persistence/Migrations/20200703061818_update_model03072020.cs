using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll_Manager.Persistence.Migrations
{
    public partial class update_model03072020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlaces_Companies_CompanyId",
                table: "CurrentWorkPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlaces_Departments_DepartmentId",
                table: "CurrentWorkPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlaces_Employees_EmployeeId",
                table: "CurrentWorkPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlaces_Positions_PositionId",
                table: "CurrentWorkPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlaces_Shops_ShopId",
                table: "CurrentWorkPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlaces_Worker_WorkerId",
                table: "CurrentWorkPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Companies_CompanyId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Departments_DepartmentId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousWorkPlaces_Employees_EmployeeId",
                table: "PreviousWorkPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousWorkPlaces_Worker_WorkerId",
                table: "PreviousWorkPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopPromotions_Shops_ShopId",
                table: "ShopPromotions");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Companies_CompanyId",
                table: "Shops");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Departments_DepartmentId",
                table: "Shops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shops",
                table: "Shops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopPromotions",
                table: "ShopPromotions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreviousWorkPlaces",
                table: "PreviousWorkPlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Positions",
                table: "Positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentWorkPlaces",
                table: "CurrentWorkPlaces");

            migrationBuilder.RenameTable(
                name: "Shops",
                newName: "Shop");

            migrationBuilder.RenameTable(
                name: "ShopPromotions",
                newName: "ShopPromotion");

            migrationBuilder.RenameTable(
                name: "PreviousWorkPlaces",
                newName: "PreviousWorkPlace");

            migrationBuilder.RenameTable(
                name: "Positions",
                newName: "Position");

            migrationBuilder.RenameTable(
                name: "CurrentWorkPlaces",
                newName: "CurrentWorkPlace");

            migrationBuilder.RenameIndex(
                name: "IX_Shops_DepartmentId",
                table: "Shop",
                newName: "IX_Shop_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Shops_CompanyId",
                table: "Shop",
                newName: "IX_Shop_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopPromotions_ShopId",
                table: "ShopPromotion",
                newName: "IX_ShopPromotion_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousWorkPlaces_WorkerId",
                table: "PreviousWorkPlace",
                newName: "IX_PreviousWorkPlace_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousWorkPlaces_EmployeeId",
                table: "PreviousWorkPlace",
                newName: "IX_PreviousWorkPlace_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_DepartmentId",
                table: "Position",
                newName: "IX_Position_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_CompanyId",
                table: "Position",
                newName: "IX_Position_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlaces_WorkerId",
                table: "CurrentWorkPlace",
                newName: "IX_CurrentWorkPlace_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlaces_ShopId",
                table: "CurrentWorkPlace",
                newName: "IX_CurrentWorkPlace_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlaces_PositionId",
                table: "CurrentWorkPlace",
                newName: "IX_CurrentWorkPlace_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlaces_EmployeeId",
                table: "CurrentWorkPlace",
                newName: "IX_CurrentWorkPlace_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlaces_DepartmentId",
                table: "CurrentWorkPlace",
                newName: "IX_CurrentWorkPlace_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlaces_CompanyId",
                table: "CurrentWorkPlace",
                newName: "IX_CurrentWorkPlace_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shop",
                table: "Shop",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopPromotion",
                table: "ShopPromotion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreviousWorkPlace",
                table: "PreviousWorkPlace",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Position",
                table: "Position",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentWorkPlace",
                table: "CurrentWorkPlace",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlace_Companies_CompanyId",
                table: "CurrentWorkPlace",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlace_Departments_DepartmentId",
                table: "CurrentWorkPlace",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlace_Employees_EmployeeId",
                table: "CurrentWorkPlace",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlace_Position_PositionId",
                table: "CurrentWorkPlace",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlace_Shop_ShopId",
                table: "CurrentWorkPlace",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlace_Worker_WorkerId",
                table: "CurrentWorkPlace",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Companies_CompanyId",
                table: "Position",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Departments_DepartmentId",
                table: "Position",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousWorkPlace_Employees_EmployeeId",
                table: "PreviousWorkPlace",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousWorkPlace_Worker_WorkerId",
                table: "PreviousWorkPlace",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shop_Companies_CompanyId",
                table: "Shop",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shop_Departments_DepartmentId",
                table: "Shop",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopPromotion_Shop_ShopId",
                table: "ShopPromotion",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlace_Companies_CompanyId",
                table: "CurrentWorkPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlace_Departments_DepartmentId",
                table: "CurrentWorkPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlace_Employees_EmployeeId",
                table: "CurrentWorkPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlace_Position_PositionId",
                table: "CurrentWorkPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlace_Shop_ShopId",
                table: "CurrentWorkPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentWorkPlace_Worker_WorkerId",
                table: "CurrentWorkPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_Position_Companies_CompanyId",
                table: "Position");

            migrationBuilder.DropForeignKey(
                name: "FK_Position_Departments_DepartmentId",
                table: "Position");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousWorkPlace_Employees_EmployeeId",
                table: "PreviousWorkPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousWorkPlace_Worker_WorkerId",
                table: "PreviousWorkPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_Shop_Companies_CompanyId",
                table: "Shop");

            migrationBuilder.DropForeignKey(
                name: "FK_Shop_Departments_DepartmentId",
                table: "Shop");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopPromotion_Shop_ShopId",
                table: "ShopPromotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopPromotion",
                table: "ShopPromotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shop",
                table: "Shop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreviousWorkPlace",
                table: "PreviousWorkPlace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Position",
                table: "Position");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentWorkPlace",
                table: "CurrentWorkPlace");

            migrationBuilder.RenameTable(
                name: "ShopPromotion",
                newName: "ShopPromotions");

            migrationBuilder.RenameTable(
                name: "Shop",
                newName: "Shops");

            migrationBuilder.RenameTable(
                name: "PreviousWorkPlace",
                newName: "PreviousWorkPlaces");

            migrationBuilder.RenameTable(
                name: "Position",
                newName: "Positions");

            migrationBuilder.RenameTable(
                name: "CurrentWorkPlace",
                newName: "CurrentWorkPlaces");

            migrationBuilder.RenameIndex(
                name: "IX_ShopPromotion_ShopId",
                table: "ShopPromotions",
                newName: "IX_ShopPromotions_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Shop_DepartmentId",
                table: "Shops",
                newName: "IX_Shops_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Shop_CompanyId",
                table: "Shops",
                newName: "IX_Shops_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousWorkPlace_WorkerId",
                table: "PreviousWorkPlaces",
                newName: "IX_PreviousWorkPlaces_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousWorkPlace_EmployeeId",
                table: "PreviousWorkPlaces",
                newName: "IX_PreviousWorkPlaces_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Position_DepartmentId",
                table: "Positions",
                newName: "IX_Positions_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Position_CompanyId",
                table: "Positions",
                newName: "IX_Positions_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlace_WorkerId",
                table: "CurrentWorkPlaces",
                newName: "IX_CurrentWorkPlaces_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlace_ShopId",
                table: "CurrentWorkPlaces",
                newName: "IX_CurrentWorkPlaces_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlace_PositionId",
                table: "CurrentWorkPlaces",
                newName: "IX_CurrentWorkPlaces_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlace_EmployeeId",
                table: "CurrentWorkPlaces",
                newName: "IX_CurrentWorkPlaces_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlace_DepartmentId",
                table: "CurrentWorkPlaces",
                newName: "IX_CurrentWorkPlaces_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentWorkPlace_CompanyId",
                table: "CurrentWorkPlaces",
                newName: "IX_CurrentWorkPlaces_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopPromotions",
                table: "ShopPromotions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shops",
                table: "Shops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreviousWorkPlaces",
                table: "PreviousWorkPlaces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Positions",
                table: "Positions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentWorkPlaces",
                table: "CurrentWorkPlaces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlaces_Companies_CompanyId",
                table: "CurrentWorkPlaces",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlaces_Departments_DepartmentId",
                table: "CurrentWorkPlaces",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlaces_Employees_EmployeeId",
                table: "CurrentWorkPlaces",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlaces_Positions_PositionId",
                table: "CurrentWorkPlaces",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlaces_Shops_ShopId",
                table: "CurrentWorkPlaces",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentWorkPlaces_Worker_WorkerId",
                table: "CurrentWorkPlaces",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Companies_CompanyId",
                table: "Positions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Departments_DepartmentId",
                table: "Positions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousWorkPlaces_Employees_EmployeeId",
                table: "PreviousWorkPlaces",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousWorkPlaces_Worker_WorkerId",
                table: "PreviousWorkPlaces",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopPromotions_Shops_ShopId",
                table: "ShopPromotions",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Companies_CompanyId",
                table: "Shops",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Departments_DepartmentId",
                table: "Shops",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
