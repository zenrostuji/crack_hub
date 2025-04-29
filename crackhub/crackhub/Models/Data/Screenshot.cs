using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class Screenshot
    {
        public int Id { get; set; }
        
        public int GameId { get; set; }
        
        [Required]
        public string ImageUrl { get; set; } = null!;

        // Navigation property
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
    }
}