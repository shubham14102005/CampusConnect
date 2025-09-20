using System.ComponentModel.DataAnnotations.Schema;

namespace CampusConnect.Models
{
    public class AnswerVote
    {
        public int Id { get; set; }
        public bool IsUpvote { get; set; }
        public int AnswerId { get; set; }
        [ForeignKey("AnswerId")]
        public Answer Answer { get; set; } = null!;
        public string ApplicationUserId { get; set; } = null!;
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}