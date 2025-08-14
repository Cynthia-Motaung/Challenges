using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    [Table("progress_updates")]
    public class Progress
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("challenge_id")]
        public int ChallengeId { get; set; }

        [Required]
        [StringLength(250)]
        [Column("progress_details")]
        public string ProgressDetails { get; set; } = null!; 

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Column("progress_percentage")]
        public double ProgressPercentage { get; set; } = 0.0;

        // Navigation Properties
        public User? User { get; set; }
        public Challenge? Challenge { get; set; } 
    }
}