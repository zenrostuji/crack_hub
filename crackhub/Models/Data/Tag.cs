using System.ComponentModel.DataAnnotations;

namespace crackhub.Models.Data
{
    public class Tag
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        
        [StringLength(100)]
        public string? Slug { get; set; }

        // Navigation property
        public ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();
    }
}