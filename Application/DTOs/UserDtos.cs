using Common.Enums;
using System.ComponentModel;

namespace Application.DTOs
{
    public class UserRegistrationDto
    {
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        [DefaultValue("+70000000000")] public string PhoneNumber { get; set; } = null!;
        [DefaultValue("user@example.com")] public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; } = null!;
    }

    public class UserAuthenticationDto
    {
        [DefaultValue("user@example.com")] public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UserUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; } = null!;
    }

    public class TokenDto
    {
        public string Token { get; set; } = null!;
    }
}
