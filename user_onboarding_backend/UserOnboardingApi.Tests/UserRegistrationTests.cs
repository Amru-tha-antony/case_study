using System.Threading.Tasks;
using UserOnboardingApi.Models;
using UserOnboardingApi.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
            var config = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string?>> { new KeyValuePair<string, string?>("Jwt:Key", "testkey") }).Build();
            var controller = new UserOnboardingApi.Controllers.AuthController(db, config);
            var user1 = new User { Email = "test@example.com", PasswordHash = "Password1" };
            await controller.Register(user1);
            var user2 = new User { Email = "test@example.com", PasswordHash = "Password2" };
            var result = await controller.Register(user2);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
