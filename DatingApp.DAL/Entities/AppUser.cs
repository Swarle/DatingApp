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
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public  string KnownAs { get; set; } = null!;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public  string Gender { get; set; } = null!;
        public  string Introduction { get; set; } = null!;
        public  string LookingFor { get; set; } = null!;
        public  string City { get; set; } = null!;
        public  string Country { get; set; } = null!;
        public List<Photo> Photos { get; set; } = [];
    }
}
