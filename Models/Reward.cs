using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    public class Reward
    {
        [Key]
        public int RewardId {get;set;}

        public int ChallengeId {get;set;}

        [Required]
        [StringLength(100)]
        public string Title {get;set;}

        [Required]
        [StringLength(250)]
        public string Description {get;set;}

        public bool IsUnlocked {get;set;}


        [NotMapped]
        public string Slug =>
            Title?.Replace(' ','-').ToLower();
            
        public Challenge Challenges {get;set;} = null!;
    }
}