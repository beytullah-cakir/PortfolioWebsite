using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Project Link")]
        public string? ProjectLink { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
