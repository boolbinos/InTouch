using Materialise.InTouch.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace Materialise.InTouch.IntegrationTests.Common
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            using (var context = CreateContext())
            {
                Debug.WriteLine("Database deleting...");
                context.Database.EnsureDeleted();
                Debug.WriteLine("Database deleted");

                Debug.WriteLine("Database creating...");
                context.Database.Migrate();
                Debug.WriteLine("Database created");
            }
        }

        public InTouchContext CreateContext()
        {
            var connectionString = 
                "Server=.;Database=InTouch.Integration.Tests;Trusted_Connection=True;MultipleActiveResultSets=true";
            var builder = new DbContextOptionsBuilder<InTouchContext>();
            builder.UseSqlServer(connectionString);

            return new InTouchContext(builder.Options);
        }

        public void Dispose()
        {
            using (var context = CreateContext())
            {
                Debug.WriteLine("Database deleting...");
                context.Database.EnsureDeleted();
                Debug.WriteLine("Database deleted");
            }
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }    
}
