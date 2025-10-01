using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserOnboardingApi.Controllers;
using UserOnboardingApi.Data;
using UserOnboardingApi.Models;
using Xunit;
using System.Collections.Generic;

namespace UserOnboardingApi.Tests
{
    public class AuthControllerPasswordHashTests
    {
        private AuthController GetController(UserDbContext db)
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string?>> { new KeyValuePair<string, string?>("Jwt:Key", "testkey") }).Build();
            return new AuthController(db, config);
        }

        private UserDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: "UserOnboardingTestDb3")
                .Options;
            return new UserDbContext(options);
        }

        [Fact]
        public async Task Register_ShouldHashPassword()
        {
            var db = GetDbContext();
            var controller = GetController(db);
            var plainPassword = "Password123";
            var user = new User { Email = "hash@example.com", PasswordHash = plainPassword };
            await controller.Register(user);
            var savedUser = await db.Users.FirstOrDefaultAsync(u => u.Email == "hash@example.com");
            Assert.NotNull(savedUser);
            Assert.NotEqual(plainPassword, savedUser.PasswordHash);
            Assert.True(BCrypt.Net.BCrypt.Verify(plainPassword, savedUser.PasswordHash));
        }
    }
}
