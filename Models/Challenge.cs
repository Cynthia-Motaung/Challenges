using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    public class Challenge
    {
        [Key]
        public int Id { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(250)]
        public string Description { get; set; } = null!;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public ChallengeStatus ChallengeStatus { get; set; }

        [NotMapped]
        public string? Slug =>
            Title?.Replace(' ', '-').ToLower();


        // Change this property to be nullable
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