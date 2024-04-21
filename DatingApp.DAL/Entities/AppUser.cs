﻿using Microsoft.AspNetCore.Identity;

namespace DatingApp.DAL.Entities
{
    public class AppUser : IdentityUser<int>
    {
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
        public List<UserLike> LikedByUsers { get; set; } = [];
        public List<UserLike> LikedUsers { get; set; } = [];
        public List<Message> MessagesSent { get; set; } = [];
        public List<Message> MessagesReceived { get; set; } = [];

        public ICollection<AppUserRole> UserRoles { get; set; } = null!;

    }
}
