using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Tag { get; set; } = string.Empty;
    }
}
