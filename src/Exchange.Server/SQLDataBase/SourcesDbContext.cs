﻿using Exchange.System.Entities;
using System.Data.Entity;

namespace Exchange.Server.SQLDataBase
{
    public class SourcesDbContext : DbContext
    {
        public SourcesDbContext() : base("DbConnectionString") { }
        public DbSet<Source> Sources { get; set; }
    }
}
