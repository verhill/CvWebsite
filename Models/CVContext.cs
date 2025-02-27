using CV_Website.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CV_Website.Models
{
    public class CVContext : IdentityDbContext<User, IdentityRole<int>, int>  // Inherit from IdentityDbContext
    {
        public CVContext(DbContextOptions<CVContext> options)
            : base(options)
        {
        }

        public DbSet<CV> CVs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Project> Project { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
            .HasOne(p => p.Creator)
            .WithMany()
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Users) 
                .WithMany(u => u.Project)  
                .UsingEntity(j => j.ToTable("ProjectUsers"));


        }
    }
}
