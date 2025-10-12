using LibraryManagementApp.Data;
using LibraryManagementApp.Enums;
using LibraryManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Helper
{
    public static class DataSeeder
    {
        public static async Task SeedUsersAsync(ApplicationDbContext context)
        {
            var existingUsers = await context.Users.ToListAsync();
            if (existingUsers.Any())
            {
                context.Users.RemoveRange(existingUsers);
                await context.SaveChangesAsync();
            }

            var admin = new User
            {
                Username = "admin",
                PasswordHash = PasswordHelper.HashPassword("admin!"),
                Email = "admin@aplibrary.com",
                Role = UserRole.User_Admin,
                IsActive = true
            };

            var normalUser = new User
            {
                Username = "user",
                PasswordHash = PasswordHelper.HashPassword("user!"),
                Email = "user@aplibrary.com",
                Role = UserRole.User_Standard,
                IsActive = true
            };

            var readOnlyUser = new User
            {
                Username = "viewer",
                PasswordHash = PasswordHelper.HashPassword("viewer!"),
                Email = "viewer@aplibrary.com",
                Role = UserRole.User_ReadOnly,
                IsActive = true
            };

            var customerMemeber = new User
            {
                Username = "member",
                PasswordHash = PasswordHelper.HashPassword("member!"),
                Email = "viewer@aplibrary.com",
                Role = UserRole.Member_Client,
                IsActive = true
            };

            

            await context.Users.AddRangeAsync(admin, normalUser, readOnlyUser, customerMemeber);
            await context.SaveChangesAsync();
        }
    }
}