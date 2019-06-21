using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Materialise.InTouch.DAL.Migrations
{
    public partial class AddModifiedPropertiesForPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Post",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_ModifiedByUserId",
                table: "Post",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_ModifiedByUserId",
                table: "Post",
                column: "ModifiedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_ModifiedByUserId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_ModifiedByUserId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Post");
        }
    }
}
