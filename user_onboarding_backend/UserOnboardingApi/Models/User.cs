using System.ComponentModel.DataAnnotations;

namespace UserOnboardingApi.Models
{
    public enum UserStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserStatus Status { get; set; } = UserStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
