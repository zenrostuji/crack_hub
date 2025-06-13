using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class Premium
    {
        public int Id { get; set; } // 1: 1 tháng, 2: 6 tháng, 3: 1 năm
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        [Required]
        public string? DurationInMonths { get; set; }
        
        // Navigation property
        public ICollection<PremiumRegister> PremiumRegisters { get; set; } = new List<PremiumRegister>();
    }
}
