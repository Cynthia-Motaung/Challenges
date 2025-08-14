using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("id")]
        public int Id {get;set;}

        [Required]
        [StringLength(20)]
        [Column("category_name")]
        public string CategoryName { get; set; } = null!;

        [NotMapped]
        public string? Slug =>
            CategoryName?.Replace(' ','-').ToLower();
        
        //Navigation Properties
        public ICollection<Challenge> Challenges {get;set;} = new List<Challenge>();
    }
}