namespace UserOnboardingWorkflow.Models
{
    public enum UserStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
