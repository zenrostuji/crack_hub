using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class SystemRequirement
    {
        public int Id { get; set; }
        
        public int GameId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string RequirementType { get; set; } = null!; // 'Minimum' or 'Recommended'
        
        [StringLength(100)]
        public string? OS { get; set; }
        
        [StringLength(100)]
        public string? Processor { get; set; }
        
        [StringLength(100)]
        public string? Memory { get; set; }
        
        [StringLength(100)]
        public string? Graphics { get; set; }
        
        [StringLength(100)]
        public string? DirectX { get; set; }
        
        [StringLength(100)]
        public string? Storage { get; set; }

        // Navigation property
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
    }
}