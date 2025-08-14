
namespace Challenges.Models
{
    public class Category
    {
      
        public int Id {get;set;}

        public string CategoryName { get; set; } = null!;

        public string? Slug =>
            CategoryName?.Replace(' ','-').ToLower();
        
        
        public ICollection<Challenge> Challenges {get;set;} = new List<Challenge>();

    }
}