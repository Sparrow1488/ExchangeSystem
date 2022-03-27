﻿using Exchange.System.Entities;
using System.Data.Entity;

namespace Exchange.Server.SQLDataBase
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext() : base("DbConnectionString") { }
        public DbSet<UserPassport> Passports { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
