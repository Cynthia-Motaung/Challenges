using System.ComponentModel.DataAnnotations;

namespace Challenges.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required]
        [StringLength(20)]
        public string? Username {get;set;} //unique

        [Required]
        [StringLength(20)]
        public string FirstName {get;set;}

        [Required]
        [StringLength(20)]
        public string LastName {get;set;}

        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [StringLength(20)]
        [EmailAddress]
        public string Email {get;set;} //unique

        [Required]
        public string PasswordHash {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public string? ProfilePicture {get;set;}//optional
        
        //Navigation Properties
        public ICollection<Challenge> Challenges {get;set;} = null!;
    }
}