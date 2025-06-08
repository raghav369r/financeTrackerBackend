using System.ComponentModel.DataAnnotations;

namespace financeTrackerBackned.Domain
{
    public class User
    {
        [Key]
        public int Id { set; get; }
        [Required]
        public String Email { set; get; } = String.Empty;
        [Required]
        public String FullName { set; get; } = String.Empty;
        [Required]
        public String Password { set; get; } = String.Empty;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public User() { }
        public User(string email, string fullName, string password)
        {
            Email = email;
            FullName = fullName;
            Password = password;
        }
        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

}