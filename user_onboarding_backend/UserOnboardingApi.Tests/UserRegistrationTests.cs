using System.Threading.Tasks;
using UserOnboardingApi.Models;
using UserOnboardingApi.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UserOnboardingApi.Tests
{
    public class UserRegistrationTests
    {
        private UserDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: "UserOnboardingTestDb")
                .Options;
            return new UserDbContext(options);
        }

        [Fact]
        public async Task RegisterUser_ShouldSetPendingStatus()
        {
            var db = GetDbContext();
            var user = new User { Email = "test@example.com", PasswordHash = "hashed" };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            var savedUser = await db.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
            Assert.NotNull(savedUser);
            Assert.Equal(UserStatus.Pending, savedUser.Status);
        }

        [Fact]
        public async Task RegisterUser_DuplicateEmail_ShouldFail()
        {
            var db = GetDbContext();
            db.Users.Add(new User { Email = "test@example.com", PasswordHash = "hashed" });
            await db.SaveChangesAsync();

            db.Users.Add(new User { Email = "test@example.com", PasswordHash = "hashed2" });
            await Assert.ThrowsAsync<DbUpdateException>(async () => await db.SaveChangesAsync());
        }
    }
}
