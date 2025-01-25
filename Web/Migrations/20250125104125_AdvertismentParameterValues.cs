using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class AdvertismentParameterValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertismentParameterValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdvertismentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryParameterId = table.Column<Guid>(type: "uuid", nullable: false),
                    IntegerValue = table.Column<int>(type: "integer", nullable: true),
                    FloatValue = table.Column<float>(type: "real", nullable: true),
                    BooleanValue = table.Column<bool>(type: "boolean", nullable: true),
                    StringValue = table.Column<string>(type: "text", nullable: true),
                    EnumValue = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertismentParameterValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertismentParameterValues_Advertisments_AdvertismentId",
                        column: x => x.AdvertismentId,
                        principalTable: "Advertisments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertismentParameterValues_CategoryParameters_CategoryPara~",
                        column: x => x.CategoryParameterId,
                        principalTable: "CategoryParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertismentParameterValues_AdvertismentId",
                table: "AdvertismentParameterValues",
                column: "AdvertismentId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertismentParameterValues_CategoryParameterId",
                table: "AdvertismentParameterValues",
                column: "CategoryParameterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertismentParameterValues");
        }
    }
}
