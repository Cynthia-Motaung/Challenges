using System.ComponentModel.DataAnnotations;

namespace Challenges.Models
{
    public class Progress
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChallengeId { get; set; }

        [Required]
        [StringLength(250)]
        public string ProgressDetails { get; set; } = null!; // Details about the progress made by the user
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        // Navigation Properties
        public User User { get; set; } = null!;
        public Challenge Challenge { get; set; } = null!;
        // Additional properties can be added here, such as progress percentage, status, etc.
        public double ProgressPercentage { get; set; } = 0.0;
    }
}
