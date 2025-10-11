using LibraryManagementApp.Helper;

namespace UnitTests.HelperTests
{
    public class PasswordHelperTests
    {
        [Fact]
        public void HashPassword_ShouldGenerateDifferentHashesForSamePassword()
        {
            // Arrange
            var password = "MySecurePassword123";

            // Act
            var hash1 = PasswordHelper.HashPassword(password);
            var hash2 = PasswordHelper.HashPassword(password);

            // Assert
            Assert.NotEqual(hash1, hash2); 
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            var password = "MySecurePassword123";
            var hash = PasswordHelper.HashPassword(password);

            var result = PasswordHelper.VerifyPassword(password, hash);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForWrongPassword()
        {
            var password = "CorrectPassword";
            var hash = PasswordHelper.HashPassword(password);

            var result = PasswordHelper.VerifyPassword("WrongPassword", hash);

            Assert.False(result);
        }
    }
}
