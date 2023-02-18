using Microsoft.EntityFrameworkCore;
using System;

namespace ConsoleAppTask
{
    public class MyDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public MyDbContext()
        {
            Database.EnsureCreated();
        }

        public MyDbContext(bool tmp)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Persons;Username=postgres;Password=admin");
        }

    }
}
