using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using MyApi.Models;

namespace MyApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        
        public DbSet<Producto> Products { get; set; }
        public DbSet<Categoria> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}