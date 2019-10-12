using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MincePieRateV2.Web.Data.Migrations
{
    public partial class AddImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "MincePies",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "MincePies");
        }
    }
}
