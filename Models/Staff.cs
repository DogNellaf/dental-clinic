using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinic.Models
{
    [Table("Staff")]
    public class Staff
    {
        public long Id { get; set; }
        public Profile Profile { get; set; } = null!;
        public string ExternalLogin { get; set; } = string.Empty;
        public List<Service> Services { get; set; } = new();
    }
}
