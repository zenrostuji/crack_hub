using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class GameTag
    {
        public int GameId { get; set; }
        
        public int TagId { get; set; }

        // Navigation properties
        [ForeignKey("GameId")]
        public Game? Game { get; set; }
        
        [ForeignKey("TagId")]
        public Tag? Tag { get; set; }
    }
}