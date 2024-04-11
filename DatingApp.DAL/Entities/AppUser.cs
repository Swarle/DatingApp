using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.DAL.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string KnownAs { get; set; } = null!;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; } = null!;
        public string? Introduction { get; set; }
        public string? LookingFor { get; set; }
        public string? Interest { get; set; }
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public List<Photo> Photos { get; set; } = [];
    }
}
