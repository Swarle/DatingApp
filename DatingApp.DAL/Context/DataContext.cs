using DatingApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.DAL.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public required DbSet<AppUser> Users { get; set; }
        public required DbSet<UserLike> Likes { get; set; }
        public required DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLike>(entity =>
            {
                entity.HasKey(k => new { k.SourceUserId, k.TargetUserId });
                
                entity.HasOne(d => d.SourceUser)
                    .WithMany(p => p.LikedUsers)
                    .HasForeignKey(f => f.SourceUserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(d => d.TargetUser)
                    .WithMany(p => p.LikedByUsers)
                    .HasForeignKey(f => f.TargetUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(u => u.Recipient)
                    .WithMany(m => m.MessagesReceived)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Sender)
                    .WithMany(m => m.MessagesSent)
                    .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}
