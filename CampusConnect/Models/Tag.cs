using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<QuestionTag> QuestionTags { get; set; }
    }
}