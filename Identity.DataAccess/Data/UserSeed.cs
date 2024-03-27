using Identity.DataAccess.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Data
{
    public class UserSeed(ModelBuilder modelBuilder)
    {
        private readonly ModelBuilder _modelBuilder = modelBuilder;

        public void Seed()
        {
            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            //Roles
            AddRoles(adminRoleId, userRoleId);


            var adminId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            AddTestData(adminId, userId);

            //Link Test Users with Roles
            GrantRole(adminRoleId, adminId);
            GrantRole(userRoleId, userId);
        }

        private void AddRoles(string adminRoleId, string userRoleId)
        {
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
        }

        private void AddTestData(string adminId, string userId)
        {
            var passwordHasher = new PasswordHasher<User>();

            var adminUser = new User
            {
                Id = adminId,
                Email = "admin@admin.com",
                EmailConfirmed = true,
                UserName = "AdminTest",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                NormalizedUserName = "ADMINTEST",
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "0451");

            var regularUser = new User
            {
                Id = userId,
                Email = "user@example.com",
                EmailConfirmed = false,
                UserName = "JoeDoe",
                NormalizedUserName = "JOEDOE",
                NormalizedEmail = "USER@EXAMPLE.COM"
            };
            regularUser.PasswordHash = passwordHasher.HashPassword(regularUser, "F_p@raD0x?");

            _modelBuilder.Entity<User>().HasData(
                adminUser,
                regularUser);
        }

        private void GrantRole(string roleId, string userId)
        {
            _modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = roleId,
                    UserId = userId
                });
        }
    }
}
