using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crackhub.Models.Data
{
    public class User
    {
        [Key]
        public string Id { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; } = null!;
        [StringLength(256)]
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        [StringLength(256)]
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        [Required]
        public int RoleId { get; set; } = 1;
        public DateTime? PremiumExpiryDate { get; set; }
        public bool RememberMe { get; set; }

        // Navigation properties
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<FavoriteGame> FavoriteGames { get; set; } = new List<FavoriteGame>();
        public ICollection<DownloadHistory> DownloadHistory { get; set; } = new List<DownloadHistory>();
        public ICollection<SearchHistory> SearchHistory { get; set; } = new List<SearchHistory>();
    }
}