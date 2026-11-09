
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class LoginView
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
