namespace Challenges.Models
{
    public class UserChallenge
    {
        public int UserId { get; set; }
        public int ChallengeId { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.Now;
        public User? User { get; set; } 
        public Challenge? Challenge { get; set; } 
    }
}
