using System.ComponentModel.DataAnnotations;

namespace Challenges.Models
{
    public class Log
    {
        [Key]
        public int LogId {get;set;}

        public int ChallengeId {get;set;}

        public DateTime Date {get;set;}

        [Required]
        [StringLength(250)]
        public string? Description {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public decimal? Progress {get;set;} //Optional progress metric

        //Navigation Properties
        public Challenge Challenges {get;set;} = null!;
    }
}