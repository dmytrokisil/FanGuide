using FanGuide.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Sportsman> Sportsmen { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Trainer> Trainers { get; set; }

        public DataBaseContext()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FanGuide;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasMany(c => c.HomeMatches).WithOne(c => c.HomeTeam)
                .HasForeignKey(c => c.HomeTeamId).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Team>().HasMany(c => c.VisitorMatches).WithOne(c => c.VisitorTeam)
                .HasForeignKey(c => c.VisitorTeamId).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Team>().HasMany(c => c.HomeMatches).WithOne(c => c.HomeTeam)
                .HasForeignKey(c => c.HomeTeamId).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Team>().HasMany(c => c.VisitorMatches).WithOne(c => c.VisitorTeam)
                .HasForeignKey(c => c.VisitorTeamId).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
