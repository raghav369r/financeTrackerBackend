using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using financeTrackerBackned.Domain;
using financeTrackerBackned.Domain.Enums;

namespace financeTrackerBackned.Dtos
{
    public class TransactionDto
    {
        [DefaultValue(0)]
        public int Id { set; get; }
        [Required]
        public string Description { set; get; }
        [Required]
        public float Amount { set; get; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TypeEnum Type { set; get; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CategoryEnum Category { set; get; }

        public Transaction DtoToEntity(int userId)
        {
            return new Transaction
            {
                Amount = this.Amount,
                Category = this.Category.ToString(),
                Date = this.Date,
                Description = this.Description,
                Type = this.Type.ToString(),
                UserId = userId
            };
        }
    }
}
