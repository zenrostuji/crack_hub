using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class SearchHistory
    {
        public int Id { get; set; }
        
        public string UserId { get; set; } = null!;
        
        [Required]
        [StringLength(200)]
        public string SearchQuery { get; set; } = null!;
        
        public DateTime SearchDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}