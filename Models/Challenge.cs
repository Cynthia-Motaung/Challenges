using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    public class Challenge
    {
        [Key]
        public int ChallengeId {get;set;}
 
        //public int UserId {get;set;}

        public int CategoryId {get;set;}

        public int RewardId {get;set;}

        [Required]
        [StringLength(50)]
        public string Title {get;set;}

        [Required]
        [StringLength(250)]
        public string Description {get;set;}

        public DateOnly StartDate {get;set;}

        public DateOnly EndDate {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string? Status {get;set;} // "Not Started", "In Progress", "Completed, "Pending"

        [NotMapped]
        public string Slug =>
            Title?.Replace(' ','-').ToLower();

        /**[StringLength(20)]
        public string? Reward {get;set;} // "Optional, e.g, "Buy new headphones", "Go for a trip"**/

        //Navigation Properties
       // public User Users {get;set;} = null!;
        public Reward Rewards {get;set;}= null!;
        public Category Categories {get;set;} = null!;
        public ICollection<Milestone> Milestones {get;set;} = null!;
        public ICollection<Log> Logs {get;set;} = null!;
         
    }
}