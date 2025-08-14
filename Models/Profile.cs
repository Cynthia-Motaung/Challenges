using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenges.Models
{
    [Table("profiles")]
    public class Profile
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [StringLength(20)]
        [Column("first_name")]
        public string? FirstName { get; set; }

        [StringLength(20)]
        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("date_of_birth")]
        public DateOnly DateOfBirth { get; set; }

        [Column("profile_picture")]
        public string? ProfilePicture { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string? Slug =>
            FullName?.Replace(' ', '-').ToLower();

        public User? User { get; set; }
    }
}