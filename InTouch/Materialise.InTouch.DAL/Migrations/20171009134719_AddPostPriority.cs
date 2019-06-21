using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Materialise.InTouch.DAL.Migrations
{
    public partial class AddPostPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "PostPriority",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Priority", x => x.Id); });

            migrationBuilder.Sql("insert into PostPriority(Id, Name) values" +
                                 " (1, 'Normal')" +
                                 ",(2, 'High');");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Priority_Id",
                table: "Post",
                column: "Priority",
                principalTable: "PostPriority",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetDefault);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Priority_Id",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Post");

            migrationBuilder.DropTable(
                name: "PostPriority");
        }
    }
}
