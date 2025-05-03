using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class AvatarFrame
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FrameName { get; set; }
        
        [Required]
        [StringLength(255)]
        public string FrameUrl { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public int RarityLevel { get; set; } = 1;
        
        [Required]
        public bool IsDefault { get; set; } = false;
        
        public int? RequiredLevel { get; set; }
        
        [Required]
        public bool IsPremium { get; set; } = false;
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        // Navigation property
        public virtual ICollection<UserAvatarFrame> UserAvatarFrames { get; set; }
    }
}