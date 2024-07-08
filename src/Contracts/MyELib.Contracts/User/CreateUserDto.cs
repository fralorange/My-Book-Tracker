using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.User
{
    /// <summary>
    /// ДТО для создания пользователя.
    /// </summary>
    public class CreateUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
