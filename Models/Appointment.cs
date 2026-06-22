using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinic.Models
{
    [Table("Appointment")]
    public class Appointment
    {
        public long Id { get; set; }
        public long StaffId { get; set; }
        public long ClientId { get; set; }
        public DateTime StartAt { get; set; }
        public short Duration { get; set; }
        public string Recommendation { get; set; } = string.Empty;
        public string DurationChangeReason { get; set; } = string.Empty;
        public List<Service> Services { get; set; } = new();

        [NotMapped]
        public bool IsBooked => ClientId != 0;
    }
}
