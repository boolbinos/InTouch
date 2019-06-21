using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Materialise.InTouch.DAL.Migrations
{
    public partial class AddPostFile2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostFile_FileId",
                table: "PostFile");

            migrationBuilder.CreateIndex(
                name: "IX_PostFile_FileId",
                table: "PostFile",
                column: "FileId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostFile_FileId",
                table: "PostFile");

            migrationBuilder.CreateIndex(
                name: "IX_PostFile_FileId",
                table: "PostFile",
                column: "FileId");
        }
    }
}
