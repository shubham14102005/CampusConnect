using System.ComponentModel.DataAnnotations;

namespace CampusConnect.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }
    }
}
