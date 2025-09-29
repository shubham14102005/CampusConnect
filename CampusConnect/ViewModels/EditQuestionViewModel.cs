using System.ComponentModel.DataAnnotations;

namespace CampusConnect.ViewModels
{
    public class EditQuestionViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string Title { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Tags (comma-separated)")]
        public string Tags { get; set; } = string.Empty;
    }
}