

namespace Challenges.Models
{
    public class Profile
    {
        
        public int Id { get; set; }
        public int UserId { get; set; }

        
        public string? FirstName { get; set; }

        
        public string? LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? ProfilePicture { get; set; }

        
        public string FullName => $"{FirstName} {LastName}";

        
        public string? Slug =>
            FullName?.Replace(' ', '-').ToLower();
        public User? User { get; set; }
    }
}
