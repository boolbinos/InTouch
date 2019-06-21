using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Materialise.InTouch.DAL.Migrations
{
    public partial class AddDefaultUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlQuery = @"DELETE FROM [dbo].[User] WHERE Id = 1;

                SET IDENTITY_INSERT[dbo].[User] ON;  

                INSERT INTO[dbo].[User]
                        ([Id]
                          ,[Contacts]
                          ,[Email]
                          ,[FirstName]
                          ,[IsDeleted]
                          ,[LastName]
                          ,[Login]
                          ,[Password])
                    VALUES
                   (
	                 1

                   , NULL

                   ,'noreplay@materialise.net'
                    ,'System'
                    , 0
                    ,'User'
                    ,'noreplay@materialise.net'
                    ,'111111')

                SET IDENTITY_INSERT[dbo].[User] OFF;";

            migrationBuilder.Sql(sqlQuery);
        }            

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sqlQuery = @"DELETE FROM [dbo].[User] WHERE Id = 1;";
            
            migrationBuilder.Sql(sqlQuery);
        }
    }
}
