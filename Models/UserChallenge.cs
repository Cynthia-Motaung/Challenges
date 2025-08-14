using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    [Table("user_challenges")]
    public class UserChallenge
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("challenge_id")]
        public int ChallengeId { get; set; }

        [Column("assigned_date")]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        public User? User { get; set; } 
        public Challenge? Challenge { get; set; } 
    }
}