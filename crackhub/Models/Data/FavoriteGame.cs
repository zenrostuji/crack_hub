using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class FavoriteGame
    {
        public string UserId { get; set; } = null!;
        
        public int GameId { get; set; }
        
        public DateTime DateAdded { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
    }
}