﻿using System;
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
        public int UpvoteCount { get; set; } = 0;
        public int DownvoteCount { get; set; } = 0;
        public bool IsBestAnswer { get; set; } = false;
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; } = null!;
        public string ApplicationUserId { get; set; } = null!;
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public ICollection<AnswerVote> AnswerVotes { get; set; } = new HashSet<AnswerVote>();
    }
}