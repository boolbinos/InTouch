using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Materialise.InTouch.DAL.Migrations
{
    public partial class AddPostType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostType",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 1
                );

            migrationBuilder.CreateTable(
                name: "PostType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Course", x => x.Id); });
            migrationBuilder.Sql("insert into PostType(Id, Name) values" +
                                 " (0, 'None')" +
                                 ",(1, 'InTouch')" +
                                 ",(2, 'Facebook')" +
                                 ",(3, 'Twitter')" +
                                 ",(4, 'SharePoint');");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_PostType_Id",
                table: "Post",
                column: "PostType",
                principalTable: "PostType",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetDefault);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostType",
                table: "Post");
        }
    }
}