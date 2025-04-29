using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class CrackInfo
    {
        public int Id { get; set; }
        
        public int GameId { get; set; }
        
        [StringLength(50)]
        public string? Version { get; set; }
        
        [StringLength(100)]
        public string? Group { get; set; }
        
        public string? Description { get; set; }
        
        public string? DownloadUrl { get; set; }
        
        [StringLength(50)]
        public string? FileSize { get; set; }
        
        public DateTime? ReleaseDate { get; set; }
        
        public bool IsRecommended { get; set; } = false;

        // Navigation property
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
    }
}