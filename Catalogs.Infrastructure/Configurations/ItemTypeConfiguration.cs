using Catalogs.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogs.Infrastructure.Configurations
{
    internal class ItemTypeConfiguration : IEntityTypeConfiguration<ItemType>
    {
        public void Configure(EntityTypeBuilder<ItemType> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .ToTable("item_type");
        }
    }
}
