using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class DownloadHistory
    {
        public int Id { get; set; }
        
        public string UserId { get; set; } = null!;
        
        public int GameId { get; set; }
        
        public DateTime DownloadDate { get; set; } = DateTime.Now;
        
        [StringLength(50)]
        public string? IP { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
    }
}