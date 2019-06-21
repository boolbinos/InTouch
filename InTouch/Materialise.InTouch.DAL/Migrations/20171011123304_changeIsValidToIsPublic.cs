using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Materialise.InTouch.DAL.Migrations
{
    public partial class changeIsValidToIsPublic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Post");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Post",
                type: "bit",
                nullable: true);

            migrationBuilder.Sql("update Post " +
                "set IsPublic= 1" +
                "where IsPublic IS NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Post");

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Post",
                nullable: true);
        }
    }
}
