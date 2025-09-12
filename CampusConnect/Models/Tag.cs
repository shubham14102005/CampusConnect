using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<QuestionTag> QuestionTags { get; set; } = new HashSet<QuestionTag>();
    }
}       