using CampusConnect.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerVote> AnswerVotes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // composite primary key for the QuestionTag table
            builder.Entity<QuestionTag>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            // Prevents a cascade delete cycle from the User to Questions
            builder.Entity<Question>()
                .HasOne(q => q.ApplicationUser)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevents a cascade delete cycle from the User to Answers
            builder.Entity<Answer>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}