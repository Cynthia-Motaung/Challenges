namespace Challenges.Models
{
    public class Challenge
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

       
        public string Title { get; set; } = null!;

        
        public string Description { get; set; } = null!;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public ChallengeStatus ChallengeStatus { get; set; }

        public string? Slug =>
            Title?.Replace(' ', '-').ToLower();

        public Category? Category { get; set; }
        public ICollection<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();
        public ICollection<Progress> Progresses { get; set; } = new List<Progress>();
    }
    public enum ChallengeStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Pending
    }
}