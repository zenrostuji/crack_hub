using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class LocalizationInfo
    {
        public int Id { get; set; }
        
        public int GameId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LocalizationType { get; set; } = null!;
        
        [StringLength(100)]
        public string? LocalizedBy { get; set; }
        
        [StringLength(50)]
        public string? LocalizationVersion { get; set; }
        
        public DateTime? LocalizationDate { get; set; }
        
        public string? Description { get; set; }
        
        public string? InstallationGuide { get; set; }
        
        public string? DownloadUrl { get; set; }
        
        [StringLength(50)]
        public string? FileSize { get; set; }
        
        public bool IsComplete { get; set; } = false;

        // Navigation property
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
    }
}