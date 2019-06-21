using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Materialise.InTouch.DAL.Migrations
{
    public partial class AssMaxlengthToAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Avatar",
                table: "User",
                type: "varbinary(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Avatar",
                table: "User",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(2000)",
                oldMaxLength: 2000,
                oldNullable: true);
        }
    }
}
