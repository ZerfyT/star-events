using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace star_events.Migrations
{
    /// <inheritdoc />
    public partial class updateeventforeignkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "OrganizerId",
                table: "Events",
                newName: "OrganizerID");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizerId",
                table: "Events",
                newName: "IX_Events_OrganizerID");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "OrganizerID",
                keyValue: null,
                column: "OrganizerID",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerID",
                table: "Events",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerID",
                table: "Events",
                column: "OrganizerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerID",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "OrganizerID",
                table: "Events",
                newName: "OrganizerId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizerID",
                table: "Events",
                newName: "IX_Events_OrganizerId");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "Events",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
