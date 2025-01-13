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
        public DbSet<Milestone> Milestones {get;set;}
        public DbSet<Log> Logs {get;set;}
        public DbSet<User> Users {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            
            base.OnModelCreating(modelBuilder);
        }
    }
}