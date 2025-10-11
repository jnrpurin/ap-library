using LibraryManagementApp.Data;
using LibraryManagementApp.Helper;
using LibraryManagementApp.Models;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.HelperTests
{
    public class DataSeederTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task SeedUsersAsync_ShouldCreateThreeUsers_WhenDatabaseIsEmpty()
        {
            // Arrange
            using var context = CreateInMemoryContext();

            // Act
            await DataSeeder.SeedUsersAsync(context);

            // Assert
            var users = await context.Users.ToListAsync();
            Assert.Equal(3, users.Count);
            Assert.Contains(users, u => u.Username == "admin");
            Assert.Contains(users, u => u.Username == "user");
            Assert.Contains(users, u => u.Username == "viewer");
        }

        [Fact]
        public async Task SeedUsersAsync_ShouldReplaceExistingUsers()
        {
            using var context = CreateInMemoryContext();

            context.Users.Add(new User { Username = "olduser", Email = "old@user.com", PasswordHash = "oldhash" });
            await context.SaveChangesAsync();

            // Act
            await DataSeeder.SeedUsersAsync(context);

            // Assert
            var users = await context.Users.ToListAsync();
            Assert.DoesNotContain(users, u => u.Username == "olduser");
            Assert.Equal(3, users.Count);
        }
    }
}
