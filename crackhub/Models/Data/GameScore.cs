using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class GameScore
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        [StringLength(50)]
        public string GameName { get; set; } = null!;
        
        [Required]
        public int Score { get; set; }
        
        [Required]
        public int Level { get; set; }
        
        [Required]
        public int EnemiesKilled { get; set; }
        
        [Required]
        public int SurvivalTime { get; set; } // Thời gian sống tính bằng giây
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
