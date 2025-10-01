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
    public class AuthControllerTests
    {
        private AuthController GetController(UserDbContext db)
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>{{"Jwt:Key", "testkey"}}).Build();
            return new AuthController(db, config);
        }

        private UserDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: "UserOnboardingTestDb2")
                .Options;
            return new UserDbContext(options);
        }

        [Fact]
        public async Task Register_InvalidEmail_ShouldReturnBadRequest()
        {
            var db = GetDbContext();
            var controller = GetController(db);
            var user = new User { Email = "invalidemail", PasswordHash = "Password1" };
            var result = await controller.Register(user);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_WeakPassword_ShouldReturnBadRequest()
        {
            var db = GetDbContext();
            var controller = GetController(db);
            var user = new User { Email = "test@example.com", PasswordHash = "short" };
            var result = await controller.Register(user);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_DuplicateEmail_ShouldReturnBadRequest()
        {
            var db = GetDbContext();
            var controller = GetController(db);
            var user1 = new User { Email = "test@example.com", PasswordHash = "Password1" };
            await controller.Register(user1);
            var user2 = new User { Email = "test@example.com", PasswordHash = "Password2" };
            var result = await controller.Register(user2);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
