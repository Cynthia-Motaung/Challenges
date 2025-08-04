using System.ComponentModel.DataAnnotations;


namespace Challenges.Models
{
    public class User
    {
        [Key]
        public int Id {get;set; }

        [Required]
        [StringLength(20, ErrorMessage = "Username must be between 3 and 20 characters long.", MinimumLength = 3)]
        public string Username {get;set;} = null!;

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email {get;set;} = null!;


        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
        //Navigation Properties
      
        public Profile? Profile {get;set;} 
        public ICollection<UserChallenge> UserChallenges {get;set;} = new List<UserChallenge> ();
    }
}