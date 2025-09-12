using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace star_events.Migrations
{
    /// <inheritdoc />
    public partial class addeventidtopromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventID",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_EventID",
                table: "Promotions",
                column: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_EventID",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "EventID",
                table: "Promotions");
        }
    }
}
