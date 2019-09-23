using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MincePieRateV2.Web.Data.Migrations
{
    public partial class AddReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: false),
                    MincePieId = table.Column<int>(nullable: false),
                    PastryRating = table.Column<int>(nullable: false),
                    FillingRating = table.Column<int>(nullable: false),
                    AppearanceRating = table.Column<int>(nullable: false),
                    AromaRating = table.Column<int>(nullable: false),
                    ValueForMoney = table.Column<int>(nullable: false),
                    FreeTextReview = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_MincePies_MincePieId",
                        column: x => x.MincePieId,
                        principalTable: "MincePies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MincePieId",
                table: "Reviews",
                column: "MincePieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
