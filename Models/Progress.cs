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
        public string ProgressDetails { get; set; } = null!; 

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public double ProgressPercentage { get; set; } = 0.0;

        // Navigation Properties
        public User? User { get; set; }
        public Challenge? Challenge { get; set; } 
       
        
    }
}
