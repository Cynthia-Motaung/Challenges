using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    [Table("challenges")]
    public class Challenge
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("title")]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(250)]
        [Column("description")]
        public string Description { get; set; } = null!;

        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        [Column("end_date")]
        public DateOnly EndDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("challenge_status")]
        public ChallengeStatus ChallengeStatus { get; set; }

        [NotMapped]
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