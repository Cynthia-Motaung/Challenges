using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id {get;set; }

        [Required]
        [StringLength(20, ErrorMessage = "Username must be between 3 and 20 characters long.", MinimumLength = 3)]
        [Column("username")]
        public string Username {get;set;} = null!;

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column("email")]
        public string Email {get;set;} = null!;

        [Column("created_at")]
        public DateTime CreatedAt {get;set;} = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
        //Navigation Properties
        public Profile? Profile {get;set;} 
        public ICollection<UserChallenge> UserChallenges {get;set;} = new List<UserChallenge> ();
    }
}