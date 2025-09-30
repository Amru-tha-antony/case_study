using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserOnboardingApi.Data;
using UserOnboardingApi.Models;
using BCrypt.Net;

namespace UserOnboardingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserDbContext _context;

        public AdminController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet("pending-users")]
        public async Task<IActionResult> GetPendingUsers()
        {
            var pending = await _context.Users.Where(u => u.Status == UserStatus.Pending).ToListAsync();
            return Ok(pending);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            user.Status = UserStatus.Approved;
            await _context.SaveChangesAsync();
            return Ok(new { message = "User approved." });
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            user.Status = UserStatus.Rejected;
            await _context.SaveChangesAsync();
            return Ok(new { message = "User rejected." });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> AdminLogin([FromBody] Admin login)
        {
            var admin = await _context.Set<Admin>().FirstOrDefaultAsync(a => a.Email == login.Email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(login.PasswordHash, admin.PasswordHash))
                return Unauthorized("Invalid credentials.");
            // Generate JWT for admin
            // ...existing code...
            return Ok(new { token = "<admin-jwt-token>" });
        }
    }
}
