using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    public class User
    {
        [StringLength(20)]
        public string? FirstName {get;set;}

        [StringLength(20)]
        public string? LastName {get;set;}

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string Slug =>
            FullName?.Replace(' ','-').ToLower();


        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public string? ProfilePicture {get;set;}
        
        //Navigation Properties
        public ICollection<Challenge> Challenges {get;set;} = null!;
    }
}