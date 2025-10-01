using Microsoft.EntityFrameworkCore;
using UserOnboardingWorkflow.Models;

namespace UserOnboardingWorkflow.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
