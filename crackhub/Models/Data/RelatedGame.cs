using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class RelatedGame
    {
        public int GameId { get; set; }
        
        public int RelatedGameId { get; set; }
        
        [StringLength(50)]
        public string? RelationType { get; set; } // "Similar", "DLC", "Sequel", etc.

        // Navigation properties
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
        
        [ForeignKey("RelatedGameId")]
        public Game? RelatedTo { get; set; }
    }
}