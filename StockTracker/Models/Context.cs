using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracker.Models
{
    public class Context: DbContext
    {
        public DbSet<StockSymbol> StockSymbols { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Database=stock-tracker;Username=postgres;Password=root");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureRelationships(modelBuilder);
            SeedData(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
        }

        private void SeedData(ModelBuilder builder)
        {
            builder.Entity<StockSymbol>().HasData(StockSymbol.GetSeedData());
        }

    }
}
