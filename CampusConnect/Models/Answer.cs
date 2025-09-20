using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusConnect.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int Score { get; set; } = 0; // This will now be correctly updated
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; } = null!;
        public string ApplicationUserId { get; set; } = null!;
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public ICollection<AnswerVote> AnswerVotes { get; set; } = new HashSet<AnswerVote>();
    }
}