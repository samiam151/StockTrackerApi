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
        public DbSet<UserWatchlist> Watchlists { get; set; }

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
            modelBuilder.Entity<UserWatchlist>()
                .HasKey(wl => new { wl.UserId, wl.SymbolId });
            modelBuilder.Entity<UserWatchlist>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.Watchlist)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<UserWatchlist>()
                .HasOne(bc => bc.Symbol)
                .WithMany(c => c.Watchlist)
                .HasForeignKey(bc => bc.SymbolId);
        }

        private void SeedData(ModelBuilder builder)
        {
            builder.Entity<StockSymbol>().HasData(StockSymbol.GetSeedData());
        }

    }
}
