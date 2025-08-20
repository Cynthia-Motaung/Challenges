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
            //Category entity configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.CategoryName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(c => c.CategoryName)
                    .IsUnique();

                // NotMapped property for Slug
                entity.Ignore(c => c.Slug);
            });

            //Challenge entity configuration
            modelBuilder.Entity<Challenge>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.Description)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(c => c.StartDate)
                    .IsRequired();

                entity.Property(c => c.EndDate)
                    .IsRequired();

                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                // NotMapped property for Slug
                entity.Ignore(c => c.Slug);

                entity.HasOne(c => c.Category)
                    .WithMany(cat => cat.Challenges)
                    .HasForeignKey(c => c.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            //User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.HasIndex(u => u.Username)
                    .IsUnique();

                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(u => u.UpdatedAt)
                    .HasDefaultValueSql("GETDATE()");

                // NotMapped properties

                entity.Ignore(u => u.Profile);
                entity.Ignore(u => u.UserChallenges);

                entity.HasOne(u => u.Profile)
                    .WithOne(p => p.User)
                    .HasForeignKey<Profile>(p => p.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            //Profile entity configuration
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.UserId)
                    .IsRequired();

               entity.Property(p => p.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(p => p.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(p => p.DateOfBirth)
                    .IsRequired();

                entity.Property(p => p.ProfilePicture)
                    .HasMaxLength(100);

                // NotMapped properties
                entity.Ignore(p => p.FullName);
                entity.Ignore(p => p.Slug);
                
                entity.HasOne(p => p.User)
                    .WithOne(u => u.Profile)
                    .HasForeignKey<Profile>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Progress>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.UserId)
                    .IsRequired();

                entity.Property(p => p.ChallengeId)
                    .IsRequired();

                entity.Property(p => p.ProgressPercentage)
                    .IsRequired()
                    .HasPrecision(5, 2); // Precision for percentage values

                entity.Property(p => p.ProgressDetails)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(p => p.UpdatedAt)
                    .HasDefaultValueSql("GETDATE()");

                
            });


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