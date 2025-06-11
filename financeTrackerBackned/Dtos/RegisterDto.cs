using System.ComponentModel.DataAnnotations;

namespace financeTrackerBackned.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Email { set; get; }
        [Required]
        [MinLength(5, ErrorMessage = "FullName must be of size atleast 5!!")]
        public string FullName { set; get; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be of size atleast 8!!")]
        public string Password { set; get; }
        public RegisterDto() { }
        public RegisterDto(string email, string password)
        {
            Email = email;
            Password = password;
        }

    }
}
