using Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess
{
    public class UserSeed(ModelBuilder modelBuilder)
    {
        private readonly ModelBuilder _modelBuilder = modelBuilder;

        public void Seed()
        {
            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            //Roles
            _modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                });

            var adminId = Guid.NewGuid().ToString();
            var adminUser = new User
            {
                Id = adminId,
                Email = "admin@admin.com",
                EmailConfirmed = true,
                UserName = "AdminTest",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                NormalizedUserName = "ADMINTEST"
            };

            var userId = Guid.NewGuid().ToString();
            var regularUser = new User
            {
                Id = userId,
                Email = "user@example.com",
                EmailConfirmed = false,
                UserName = "JoeDoe",
                NormalizedUserName = "JOEDOE",
                NormalizedEmail = "USER@EXAMPLE.COM"
            };

            //Add Hash Password
            var passwordHasher = new PasswordHasher<User>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "0451");
            regularUser.PasswordHash = passwordHasher.HashPassword(regularUser, "F_p@raD0x?");


            _modelBuilder.Entity<User>().HasData(
                adminUser,
                regularUser);

            //Link Test Users with Roles
            _modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = userId
                });
        }
    }
}
