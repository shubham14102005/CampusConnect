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

            // Composite primary key for the QuestionTag join table
            builder.Entity<QuestionTag>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            // Define the relationship from QuestionTag to Question
            builder.Entity<QuestionTag>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionTags)
                .HasForeignKey(qt => qt.QuestionId);

            // Define the relationship from QuestionTag to Tag
            builder.Entity<QuestionTag>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionTags)
                .HasForeignKey(qt => qt.TagId);

            // Configure the relationship between Question and ApplicationUser
            builder.Entity<Question>()
                .HasOne(q => q.ApplicationUser)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents deleting a user if they have questions

            // Configure the relationship between Answer and ApplicationUser
            builder.Entity<Answer>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between AnswerVote and ApplicationUser
            builder.Entity<AnswerVote>()
                .HasOne(v => v.ApplicationUser)
                .WithMany(u => u.AnswerVotes)
                .HasForeignKey(v => v.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between AnswerVote and Answer
            builder.Entity<AnswerVote>()
                .HasOne(v => v.Answer)
                .WithMany(a => a.AnswerVotes)
                .HasForeignKey(v => v.AnswerId)
                .OnDelete(DeleteBehavior.Cascade); // Deletes votes if the parent answer is deleted
        }
    }
}