using System.ComponentModel.DataAnnotations;

namespace CampusConnect.ViewModels
{
    public class RegisterFacultyViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Functioning Title")]
        public string? FunctioningTitle { get; set; }

        [Required]
        [Display(Name = "Secret Key")]
        public string? SecretKey { get; set; }
    }
}
