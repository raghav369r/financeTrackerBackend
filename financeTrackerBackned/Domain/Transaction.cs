using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace financeTrackerBackned.Domain
{
    public class Transaction
    {
        [Key]
        public int Id { set; get; }
        public String Description { set; get; }
        public float Amount { set; get; }
        public DateOnly Date { get; set; }
        public String Type { set; get; }
        public String Category { set; get; }
        [ForeignKey(nameof(User))]
        public int UserId { set; get; }

        public User user { set; get; } 
        public Transaction(string description, float amount, DateOnly date, string type, string category,int userId)
        {
            Description = description;
            Amount = amount;
            Date = date;
            this.Type = type;
            Category = category;
            this.UserId = userId;
        }
        public Transaction() { }

    }
}