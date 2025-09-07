using System.ComponentModel.DataAnnotations.Schema;

namespace CampusConnect.Models
{
    public class AnswerVote
    {
        public int Id { get; set; }
        public int Value { get; set; } // +1 for like, -1 for dislike

        public int AnswerId { get; set; }
        [ForeignKey("AnswerId")]
        public Answer Answer { get; set; } = null!;

        public string ApplicationUserId { get; set; } = null!;
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}