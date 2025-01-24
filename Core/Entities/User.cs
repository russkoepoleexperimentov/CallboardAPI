using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string PhoneNumber { get; set; } = null!;
        [Key] public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; } = null!;
        public bool IsSuperuser { get; set; } = false;
    }
}
