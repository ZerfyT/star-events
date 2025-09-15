using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace star_events.Migrations
{
    /// <inheritdoc />
    public partial class addmultipleeventimagepaths2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath1",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ImagePath2",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ImagePath3",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ImagePath4",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "ImagePath5",
                table: "Events",
                newName: "ImagePaths");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePaths",
                table: "Events",
                newName: "ImagePath5");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath1",
                table: "Events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath2",
                table: "Events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath3",
                table: "Events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath4",
                table: "Events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Events",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
