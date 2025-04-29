using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class GameLink
    {
        public int Id { get; set; }
        
        public int GameId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string LinkName { get; set; } = null!; // 'Download', 'Torrent', 'Official Site', etc.
        
        [Required]
        public string Url { get; set; } = null!;
        
        [StringLength(50)]
        public string? FileSize { get; set; }
        
        public int PartNumber { get; set; } = 1;
        
        public int TotalParts { get; set; } = 1;
        
        [StringLength(100)]
        public string? Password { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [Column(TypeName = "datetime")]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
    }
}