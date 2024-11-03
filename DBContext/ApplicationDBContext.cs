using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using UrlShortenerAPI.Models;

namespace UrlShortenerAPI.Data
{
    public class URLDbContext : DbContext
    {
        public URLDbContext(DbContextOptions<URLDbContext> options) : base(options) { }
        public DbSet<URL> URLs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<URL>()
                .HasKey(u => u.OriginalUrl); 

            modelBuilder.Entity<URL>()
                .HasIndex(u => u.OriginalUrl) 
                .IsUnique(false); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
