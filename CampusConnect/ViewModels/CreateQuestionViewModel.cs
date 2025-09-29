using CampusConnect.Models; // Make sure you have a using statement for your models
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CampusConnect.ViewModels // This namespace must be correct
{
    public class CreateQuestionViewModel // This class name must be correct
    {
        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string Title { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Tags (comma-separated)")]
        public string Tags { get; set; } = string.Empty;

        public List<Question> AllQuestions { get; set; } = new List<Question>();
    }
}