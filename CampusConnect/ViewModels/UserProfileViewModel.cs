using CampusConnect.Models;
using System.Collections.Generic;

namespace CampusConnect.ViewModels
{
    public class UserProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public int QuestionCount { get; set; }
        public int AnswerCount { get; set; }
        public IEnumerable<Question>? RecentQuestions { get; set; }
        public IEnumerable<Answer>? RecentAnswers { get; set; }
        public int Reputation { get; set; }
    }
}
