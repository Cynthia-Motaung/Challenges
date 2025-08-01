using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    public class Category
    {
        [Key]
        public int Id {get;set;}

        [Required]
        [StringLength(20)]
        public string CategoryName { get; set; } = null!;

        [NotMapped]
        public string? Slug =>
            CategoryName?.Replace(' ','-').ToLower();
        
        //Navigation Properties
        public ICollection<Challenge> Challenges {get;set;} = new List<Challenge>();

    }
}