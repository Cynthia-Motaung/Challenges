using Microsoft.EntityFrameworkCore;
using Challenges.Models;


namespace Challenges.Data
{
    public class ChallengesDbContext : DbContext
    {
        public ChallengesDbContext(DbContextOptions<ChallengesDbContext> options) :base(options)
        {
            
        }

        public DbSet<Challenge> Challenges {get;set;}
        public DbSet<Category> Categories {get;set;}
        public DbSet<User> Users {get;set;}
        public DbSet<UserChallenge> UserChallenges {get;set; }
        public DbSet<Progress> Progresses {get;set; }
        public DbSet<Profile> Profiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Challenge entity configuration
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();

            //User entity configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<Profile>(p => p.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);


            //User Challenge many-to-many relationship
            modelBuilder.Entity<UserChallenge>()
                .HasKey(uc => new { uc.UserId, uc.ChallengeId });

            modelBuilder.Entity<UserChallenge>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserChallenges)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserChallenge>()
                .HasOne(uc => uc.Challenge)
                .WithMany(c => c.UserChallenges)
                .HasForeignKey(uc => uc.ChallengeId);

            modelBuilder.Entity<Progress>()
                .Property(p => p.ProgressPercentage)
                .HasPrecision(5, 2); // Precision for percentage values

            base.OnModelCreating(modelBuilder);
        }
    }
}