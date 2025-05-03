using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class UserAvatarFrame
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }
        
        [Key]
        [Column(Order = 1)]
        public int FrameId { get; set; }
        
        [Required]
        public bool IsEquipped { get; set; } = false;
        
        [Required]
        public DateTime ObtainedDate { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        [ForeignKey("FrameId")]
        public virtual AvatarFrame AvatarFrame { get; set; }
    }
}