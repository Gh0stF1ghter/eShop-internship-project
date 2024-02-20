﻿using Catalogs.Domain.Entities.Models;
using Catalogs.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

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

            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VendorConfiguration());
            modelBuilder.ApplyConfiguration(new BrandConfiguration());

            new CatalogSeed(modelBuilder).Seed();
        }
    }
}