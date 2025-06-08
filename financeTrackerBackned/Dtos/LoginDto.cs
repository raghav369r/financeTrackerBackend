using System.ComponentModel.DataAnnotations;

namespace financeTrackerBackned.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Email { set; get; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be of size atleast 8!!")]
        public string Password { set; get; }
    }
}
