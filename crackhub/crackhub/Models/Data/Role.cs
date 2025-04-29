using System.ComponentModel.DataAnnotations;

namespace crackhub.Models.Data
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}