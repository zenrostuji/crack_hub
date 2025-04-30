using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class Review
    {
        public int Id { get; set; }
        
        public int GameId { get; set; }
        
        public string UserId { get; set; } = null!;
        
        [StringLength(100)]
        public string? Title { get; set; }
        
        public string? Content { get; set; }
        
        public double Rating { get; set; }
        
        public DateTime DatePosted { get; set; } = DateTime.Now;
        
        public int HelpfulCount { get; set; } = 0;

        // Navigation properties
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
        
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}