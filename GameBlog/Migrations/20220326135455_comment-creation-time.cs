using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBlog.Migrations
{
    public partial class commentcreationtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PostDate",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "Comments");
        }
    }
}
