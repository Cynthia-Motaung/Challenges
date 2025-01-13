using System.ComponentModel.DataAnnotations;

namespace Challenges.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {get;set;}

        [Required]
        [StringLength(20)]
        public string CategoryName {get;set;}// unique

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
        [StringLength(250)]
        public string? Description {get;set;}//optional
        
        //Navigation Properties
        public ICollection<Challenge> Challenges {get;set;} = null!;

    }
}