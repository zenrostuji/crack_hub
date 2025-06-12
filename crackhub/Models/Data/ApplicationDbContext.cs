using Microsoft.EntityFrameworkCore;

namespace crackhub.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Screenshot> Screenshots { get; set; }
        public DbSet<SystemRequirement> SystemRequirements { get; set; }
        public DbSet<Feature> Features { get; set; }
        
        // Sửa đổi định nghĩa DbSet để khớp với tên bảng trong cơ sở dữ liệu
        public DbSet<CrackInfo> CrackInfo { get; set; }
        
        public DbSet<GameLink> GameLinks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<FavoriteGame> FavoriteGames { get; set; }
        public DbSet<DownloadHistory> DownloadHistory { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<GameTag> GameTags { get; set; }
        public DbSet<LocalizationInfo> LocalizationInfos { get; set; }
        public DbSet<RelatedGame> RelatedGames { get; set; }
        public DbSet<AvatarFrame> AvatarFrames { get; set; }
        public DbSet<UserAvatarFrame> UserAvatarFrames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình các quan hệ một-nhiều
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Category)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CategoryId);

            modelBuilder.Entity<Screenshot>()
                .HasOne(s => s.Game)
                .WithMany(g => g.Screenshots)
                .HasForeignKey(s => s.GameId);

            modelBuilder.Entity<SystemRequirement>()
                .HasOne(sr => sr.Game)
                .WithMany(g => g.SystemRequirements)
                .HasForeignKey(sr => sr.GameId);

            modelBuilder.Entity<Feature>()
                .HasOne(f => f.Game)
                .WithMany(g => g.Features)
                .HasForeignKey(f => f.GameId);

            // Chỉ định tên bảng chính xác cho model CrackInfo
            modelBuilder.Entity<CrackInfo>()
                .ToTable("CrackInfo")
                .HasOne(c => c.Game)
                .WithMany(g => g.CrackInfos)
                .HasForeignKey(c => c.GameId);

            modelBuilder.Entity<GameLink>()
                .HasOne(gl => gl.Game)
                .WithMany(g => g.GameLinks)
                .HasForeignKey(gl => gl.GameId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GameId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            // Cấu hình khóa chính cho các bảng nhiều-nhiều
            modelBuilder.Entity<FavoriteGame>()
                .HasKey(fg => new { fg.UserId, fg.GameId });

            modelBuilder.Entity<FavoriteGame>()
                .HasOne(fg => fg.User)
                .WithMany(u => u.FavoriteGames)
                .HasForeignKey(fg => fg.UserId);

            modelBuilder.Entity<FavoriteGame>()
                .HasOne(fg => fg.Game)
                .WithMany(g => g.FavoriteGames)
                .HasForeignKey(fg => fg.GameId);

            modelBuilder.Entity<GameTag>()
                .HasKey(gt => new { gt.GameId, gt.TagId });

            modelBuilder.Entity<GameTag>()
                .HasOne(gt => gt.Game)
                .WithMany(g => g.GameTags)
                .HasForeignKey(gt => gt.GameId);

            modelBuilder.Entity<GameTag>()
                .HasOne(gt => gt.Tag)
                .WithMany(t => t.GameTags)
                .HasForeignKey(gt => gt.TagId);

            modelBuilder.Entity<RelatedGame>()
                .HasKey(rg => new { rg.GameId, rg.RelatedGameId });

            modelBuilder.Entity<RelatedGame>()
                .HasOne(rg => rg.Game)
                .WithMany(g => g.RelatedGames)
                .HasForeignKey(rg => rg.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RelatedGame>()
                .HasOne(rg => rg.RelatedTo)
                .WithMany()
                .HasForeignKey(rg => rg.RelatedGameId)
                .OnDelete(DeleteBehavior.NoAction);
                
            // Cấu hình cho AvatarFrame và UserAvatarFrame
            modelBuilder.Entity<AvatarFrame>()
                .ToTable("AvatarFrames");
                
            modelBuilder.Entity<UserAvatarFrame>()
                .ToTable("UserAvatarFrames")
                .HasKey(uaf => new { uaf.UserId, uaf.FrameId });
                
            modelBuilder.Entity<UserAvatarFrame>()
                .HasOne(uaf => uaf.User)
                .WithMany(u => u.UserAvatarFrames)
                .HasForeignKey(uaf => uaf.UserId);
                
            modelBuilder.Entity<UserAvatarFrame>()
                .HasOne(uaf => uaf.AvatarFrame)
                .WithMany(af => af.UserAvatarFrames)
                .HasForeignKey(uaf => uaf.FrameId);

            base.OnModelCreating(modelBuilder);
        }
    }
}