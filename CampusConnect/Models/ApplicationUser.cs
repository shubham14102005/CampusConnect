using Microsoft.AspNetCore.Identity;

namespace CampusConnect.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<AnswerVote> AnswerVotes { get; set; }
    }
}