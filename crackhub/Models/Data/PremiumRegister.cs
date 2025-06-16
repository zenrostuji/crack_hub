using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class PremiumRegister
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        public int PackageId { get; set; }
        
        [Required]
        public int Status { get; set; } = 2; // 1: Đã xét, 2: Đang chờ, 3: Không xét
        
        [Required]
        public string ProofImageUrl { get; set; } = null!;
        
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        
        public DateTime? ApprovalDate { get; set; }
        
        public string? AdminComment { get; set; }
        
        public string? UserComment { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        [ForeignKey("PackageId")]
        public Premium? Premium { get; set; }
    }
}
