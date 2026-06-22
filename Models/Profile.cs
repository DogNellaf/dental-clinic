using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinic.Models
{
    [Table("Profile")]
    public class Profile: IdentityUser<long>
    {
        [Key]
        public override long Id { get; set; }
        public long RoleId { get; set; }

        public bool IsClient { get { return RoleId == 1; } }
        public bool IsAdmin { get { return RoleId == 2; } }
        public bool IsManager { get { return RoleId == 3; } }
        public bool IsDoctor { get { return RoleId == 4; } }
    }
}
