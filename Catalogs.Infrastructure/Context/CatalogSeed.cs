using Catalogs.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Context
{
    internal class CatalogSeed(ModelBuilder builder)
    {
        private readonly ModelBuilder _builder = builder;

        public void Seed()
        {
            _builder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Starbucks" },
                new Brand { Id = 2, Name = "Apple" },
                new Brand { Id = 3, Name = "Nike" },
                new Brand { Id = 4, Name = "CodeMaze" },
                new Brand { Id = 5, Name = "Lego" }
                );

            _builder.Entity<ItemType>().HasData(
                new ItemType { Id = 1, Name = "Learning" },
                new ItemType { Id = 2, Name = "Electronics" },
                new ItemType { Id = 3, Name = "Clothes" },
                new ItemType { Id = 4, Name = "Food" },
                new ItemType { Id = 5, Name = "Software" }
                );

            _builder.Entity<Vendor>().HasData(
                new Vendor { Id = 1, Name = "Amazon" },
                new Vendor { Id = 2, Name = "Walmart" },
                new Vendor { Id = 3, Name = "Starbucks" },
                new Vendor { Id = 4, Name = "Netflix" },
                new Vendor { Id = 5, Name = "MediaMarkt" }
                );

            _builder.Entity<Item>().HasData(
                new Item
                {
                    Id = 1,
                    Name = "Headphones",
                    Stock = 15,
                    Price = 49.99,
                    ImageUrl = "headphones.jpg",
                    BrandId = 2,
                    TypeId = 2,
                    VendorId = 5
                },
                new Item
                {
                    Id = 2,
                    Name = "Book",
                    Stock = 50,
                    Price = 19.99,
                    ImageUrl = "book.jpg",
                    BrandId = 4,
                    TypeId = 1,
                    VendorId = 1
                },
                new Item
                {
                    Id = 3,
                    Name = "Coffee Mug",
                    Stock = 20,
                    Price = 9.99,
                    ImageUrl = "mug.jpg",
                    BrandId = 1,
                    TypeId = 4,
                    VendorId = 3
                },
                new Item
                {
                    Id = 4,
                    Name = "T-Shirt",
                    Stock = 30,
                    Price = 14.99,
                    ImageUrl = "tshirt.jpg",
                    BrandId = 3,
                    TypeId = 3,
                    VendorId = 1
                },
                new Item
                {
                    Id = 5,
                    Name = "Headphones",
                    Stock = 15,
                    Price = 49.99,
                    ImageUrl = "headphones.jpg",
                    BrandId = 2,
                    TypeId = 2,
                    VendorId = 5
                }
                );
        }
    }
}
