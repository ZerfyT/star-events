using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace star_events.Migrations
{
    /// <inheritdoc />
    public partial class updatetickettyperelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_EventID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Promotions_PromotionID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Categories_CategoryID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Locations_LocationID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketTypes_TicketTypeID",
                table: "Tickets");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "TicketTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountValue",
                table: "Promotions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IsScanned",
                table: "Tickets",
                column: "IsScanned");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_QRCodeValue",
                table: "Tickets",
                column: "QRCodeValue");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_IsActive",
                table: "Promotions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_PromoCode",
                table: "Promotions",
                column: "PromoCode");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDateTime",
                table: "Events",
                column: "StartDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Status",
                table: "Events",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingDateTime",
                table: "Bookings",
                column: "BookingDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Status",
                table: "Bookings",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_EventID",
                table: "Bookings",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Promotions_PromotionID",
                table: "Bookings",
                column: "PromotionID",
                principalTable: "Promotions",
                principalColumn: "PromotionID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerID",
                table: "Events",
                column: "OrganizerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Categories_CategoryID",
                table: "Events",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Locations_LocationID",
                table: "Events",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketTypes_TicketTypeID",
                table: "Tickets",
                column: "TicketTypeID",
                principalTable: "TicketTypes",
                principalColumn: "TicketTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_EventID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Promotions_PromotionID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Categories_CategoryID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Locations_LocationID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketTypes_TicketTypeID",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_IsScanned",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_QRCodeValue",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_IsActive",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_PromoCode",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Events_StartDateTime",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_Status",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BookingDateTime",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_Status",
                table: "Bookings");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "TicketTypes",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountValue",
                table: "Promotions",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_EventID",
                table: "Bookings",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Promotions_PromotionID",
                table: "Bookings",
                column: "PromotionID",
                principalTable: "Promotions",
                principalColumn: "PromotionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerID",
                table: "Events",
                column: "OrganizerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Categories_CategoryID",
                table: "Events",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Locations_LocationID",
                table: "Events",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Events_EventID",
                table: "Promotions",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketTypes_TicketTypeID",
                table: "Tickets",
                column: "TicketTypeID",
                principalTable: "TicketTypes",
                principalColumn: "TicketTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
