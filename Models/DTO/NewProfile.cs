using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models.DTO
{
    public class NewProfile
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(RoleTitle))]
        public RoleTitle RoleTitle { get; set; }
    }
}
