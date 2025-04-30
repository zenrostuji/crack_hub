using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class Game
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;
        
        [StringLength(500)]
        public string? ShortDescription { get; set; }
        
        public string? FullDescription { get; set; }
        
        public string? CoverImageUrl { get; set; }
        
        [StringLength(100)]
        public string? Developer { get; set; }
        
        [StringLength(100)]
        public string? Publisher { get; set; }
        
        public DateTime? ReleaseDate { get; set; }
        
        public int? CategoryId { get; set; }
        
        public double Rating { get; set; } = 0;
        
        [StringLength(50)]
        public string? Downloads { get; set; } = "0";
        
        public string? DownloadUrl { get; set; }
        
        [StringLength(50)]
        public string? Size { get; set; }
        
        public double AverageRating { get; set; } = 0;

        // Navigation properties
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        
        public ICollection<SystemRequirement> SystemRequirements { get; set; } = new List<SystemRequirement>();
        public ICollection<Feature> Features { get; set; } = new List<Feature>();
        public ICollection<Screenshot> Screenshots { get; set; } = new List<Screenshot>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<CrackInfo> CrackInfos { get; set; } = new List<CrackInfo>();
        public ICollection<FavoriteGame> FavoriteGames { get; set; } = new List<FavoriteGame>();
        public ICollection<DownloadHistory> DownloadHistory { get; set; } = new List<DownloadHistory>();
        public ICollection<GameLink> GameLinks { get; set; } = new List<GameLink>();
        public ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();
        public ICollection<LocalizationInfo> LocalizationInfos { get; set; } = new List<LocalizationInfo>();
        
        public ICollection<RelatedGame> RelatedGames { get; set; } = new List<RelatedGame>();
        public ICollection<RelatedGame> RelatedToGames { get; set; } = new List<RelatedGame>();
    }
}