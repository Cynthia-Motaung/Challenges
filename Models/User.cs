using System.ComponentModel.DataAnnotations;


namespace Challenges.Models
{
    public class User
    {
        
        public int Id {get;set; }

        
        public string Username {get;set;} = null!;

        public string Email {get;set;} = null!;

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
        //Navigation Properties
      
        public Profile? Profile {get;set;} 
        public ICollection<UserChallenge> UserChallenges {get;set;} = new List<UserChallenge> ();
    }
}