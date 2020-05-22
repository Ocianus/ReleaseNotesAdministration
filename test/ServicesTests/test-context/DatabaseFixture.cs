﻿using DbUp;
using Microsoft.Extensions.DependencyInjection;
using System;
using test.ServicesTests.DbMigration;

namespace test.ServicesTests.test_context
{
    public class DatabaseFixture : IDisposable
    {
        public string ConnectionString { get; set; }

        public DatabaseFixture()
        {
            //ConnectionString = $"Server=(localdb)\\MSSQLLocalDB;Database=TestDatabase_{Guid.NewGuid()}; Integrated Security=SSPI; Connection Timeout=60";
            ConnectionString = $"Server=(localdb)\\MSSQLLocalDB;Database=ReleaseNotesDb_{Guid.NewGuid()};Integrated Security=SSPI;Connection Timeout=60";
            DbMigrator.Migrate(ConnectionString);
        }

        public void Dispose()
        {
            DropDatabase.For.SqlDatabase(ConnectionString);
        }
    }
}
