using Catalogs.Domain.Entities.Models;
using Catalogs.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Context
{
    public class CatalogContext(DbContextOptions options) : DbContext(options)
    {
        DbSet<Brand>? Brands { get; set; }
        DbSet<Item>? Items { get; set; }
        DbSet<ItemType>? Types { get; set; }
        DbSet<Vendor>? Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnitOfWork).Assembly);

            new CatalogSeed(modelBuilder).Seed();
        }
    }
}