using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace star_events.Migrations
{
    /// <inheritdoc />
    public partial class updatepromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions");

            migrationBuilder.AlterColumn<int>(
                name: "EventID",
                table: "Promotions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions");

            migrationBuilder.AlterColumn<int>(
                name: "EventID",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
