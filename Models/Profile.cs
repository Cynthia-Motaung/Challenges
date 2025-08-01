using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        [StringLength(20)]
        public string? FirstName { get; set; }

        [StringLength(20)]
        public string? LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }
        public string? ProfilePicture { get; set; }


        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";



        [NotMapped]
        public string? Slug =>
            FullName?.Replace(' ', '-').ToLower();
        public User User { get; set; } = null!;
    }
}
