namespace Challenges.Models.DTOs
{
    // DTO for displaying a list of users (Index view)
    public class UserIndexDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    // DTO for creating a new user (Create POST action)
    public class UserCreateDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        // CreatedAt and UpdatedAt are set in the controller, not by user input
    }

    // DTO for editing an existing user (Edit GET and POST actions)
    public class UserEditDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        // CreatedAt is read-only for editing, UpdatedAt is set by controller
    }

    // DTO for displaying user details (Details view)
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
