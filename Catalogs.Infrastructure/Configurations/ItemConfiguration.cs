using Catalogs.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Infrastructure.Configurations
{
    internal class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(20);
            builder
                .Property(x => x.Stock)
                .IsRequired();
            builder
                .Property(x => x.Price)
                .IsRequired();
            builder
                .HasOne(x => x.Brand)
                .WithMany(b => b.Items)
                .HasForeignKey(b => b.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne(x => x.Type)
                .WithMany(t => t.Items)
                .HasForeignKey(x=> x.TypeId);
            builder
                .HasOne(x => x.Vendor)
                .WithMany(v => v.Items)
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .ToTable("item");
        }
    }
}
