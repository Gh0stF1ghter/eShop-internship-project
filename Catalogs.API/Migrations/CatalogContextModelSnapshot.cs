﻿// <auto-generated />
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Catalogs.API.Migrations
{
    [DbContext(typeof(CatalogContext))]
    partial class CatalogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Catalogs.Domain.Entities.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("brand", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Starbucks"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Apple"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Nike"
                        },
                        new
                        {
                            Id = 4,
                            Name = "CodeMaze"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Lego"
                        });
                });

            modelBuilder.Entity("Catalogs.Domain.Entities.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<int>("VendorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("TypeId");

                    b.HasIndex("VendorId");

                    b.ToTable("item", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BrandId = 2,
                            ImageUrl = "headphones.jpg",
                            Name = "Headphones",
                            Price = 49.990000000000002,
                            Stock = 15,
                            TypeId = 2,
                            VendorId = 5
                        },
                        new
                        {
                            Id = 2,
                            BrandId = 4,
                            ImageUrl = "book.jpg",
                            Name = "Book",
                            Price = 19.989999999999998,
                            Stock = 50,
                            TypeId = 1,
                            VendorId = 1
                        },
                        new
                        {
                            Id = 3,
                            BrandId = 1,
                            ImageUrl = "mug.jpg",
                            Name = "Coffee Mug",
                            Price = 9.9900000000000002,
                            Stock = 20,
                            TypeId = 4,
                            VendorId = 3
                        },
                        new
                        {
                            Id = 4,
                            BrandId = 3,
                            ImageUrl = "tshirt.jpg",
                            Name = "T-Shirt",
                            Price = 14.99,
                            Stock = 30,
                            TypeId = 3,
                            VendorId = 1
                        },
                        new
                        {
                            Id = 5,
                            BrandId = 2,
                            ImageUrl = "headphones.jpg",
                            Name = "Headphones",
                            Price = 49.990000000000002,
                            Stock = 15,
                            TypeId = 2,
                            VendorId = 5
                        });
                });

            modelBuilder.Entity("Catalogs.Domain.Entities.Models.ItemType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("type", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Learning"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Clothes"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Food"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Software"
                        });
                });

            modelBuilder.Entity("Catalogs.Domain.Entities.Models.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("vendor", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Amazon"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Walmart"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Starbucks"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Netflix"
                        },
                        new
                        {
                            Id = 5,
                            Name = "MediaMarkt"
                        });
                });

            modelBuilder.Entity("Catalogs.Domain.Entities.Models.Item", b =>
                {
                    b.HasOne("Catalogs.Domain.Entities.Models.Brand", "Brand")
                        .WithMany("Items")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalogs.Domain.Entities.Models.ItemType", "Type")
                        .WithMany("Items")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalogs.Domain.Entities.Models.Vendor", "Vendor")
                        .WithMany("Items")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Type");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("Catalogs.Domain.Entities.Models.Brand", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Catalogs.Domain.Entities.Models.ItemType", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Catalogs.Domain.Entities.Models.Vendor", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
