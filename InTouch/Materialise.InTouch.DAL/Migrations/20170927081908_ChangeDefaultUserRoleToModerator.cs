using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Materialise.InTouch.DAL.Migrations
{
    public partial class ChangeDefaultUserRoleToModerator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlQuery = @"UPDATE [dbo].[User]
                             SET [dbo].[User].[RoleId] = 2
                             WHERE [dbo].[User].[Id] = 1";

            migrationBuilder.Sql(sqlQuery);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sqlQuery = @"UPDATE [dbo].[User]
                             SET [dbo].[User].[RoleId] = 1
                             WHERE [dbo].[User].[Id] = 1";

            migrationBuilder.Sql(sqlQuery);
        }
    }
}
