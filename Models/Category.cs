using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {get;set;}

        [Required]
        [StringLength(20)]
        public string CategoryName {get;set;}// unique

        [StringLength(250)]
        public string? Description {get;set;}//optional

        [NotMapped]
        public string Slug =>
            CategoryName?.Replace(' ','-').ToLower();
        
        //Navigation Properties
        public ICollection<Challenge> Challenges {get;set;} = null!;

    }
}