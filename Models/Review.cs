using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinic.Models
{
    [Table("Review")]
    public class Review
    {
        public long Id { get; set; }
        public long ProfileId { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        public bool IsVisible { get; set; }
    }
}
