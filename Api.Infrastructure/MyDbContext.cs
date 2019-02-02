using Api.Core.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace Api.Infrastructure
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
