using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystem.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public const int UserFirstNameMaxLength = 12;
        public const int UserFirstNameMinLength = 1;

        public const int UserLastNameMaxLength = 12;
        public const int UserLastNameMinLength = 1;

        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        public string FirstName { get; init; } = null!;

        [Required]
        [MaxLength(UserLastNameMaxLength)]
        public string LastName { get; init; } = null!;

        [Required]
        public bool IsActive { get; set; }
    }
}
