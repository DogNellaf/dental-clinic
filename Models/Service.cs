using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinic.Models
{
    [Table("Service")]
    public class Service
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<Staff> Staff { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
    }
}
