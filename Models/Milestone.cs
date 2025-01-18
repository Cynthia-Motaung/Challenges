using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    public class Milestone
    {
        [Key]
        public int MilestoneId {get;set;}

        public int ChallengeId {get;set;}

        [Required]
        [StringLength(50)]
        public string Title {get;set;}

        /**[Required]
        [StringLength(20)]
        public string Status {get;set;} // "Not Started","In Progress", "Completed"**/

        /**public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;**/

        public bool IsCompleted {get;set;}

        [StringLength(250)]
        public string? Description {get;set;}//optional

        public DateTime? DueDate {get;set;}

        [NotMapped]
        public string Slug =>
            Title?.Replace(' ','-').ToLower();
    
        //Navigation Properties
        public Challenge Challenges {get;set;} = null!;


    }
}