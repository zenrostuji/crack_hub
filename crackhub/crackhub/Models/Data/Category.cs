using System.ComponentModel.DataAnnotations;

namespace crackhub.Models.Data
{
    public class Category
    {
        public int CategoryId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = null!;
        
        public string? Description { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Slug { get; set; } = null!;
        
        [StringLength(50)]
        public string? IconClass { get; set; }
        
        public int GameCount { get; set; } = 0;

        // Navigation property
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}