using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CampusConnect.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? FunctioningTitle { get; set; }

        public ICollection<Question> Questions { get; set; } = new HashSet<Question>();
        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
        public ICollection<AnswerVote> AnswerVotes { get; set; } = new HashSet<AnswerVote>();
    }
}