-- Xóa dữ liệu cũ nếu cần thiết
DELETE FROM UserAvatarFrames;
DELETE FROM AvatarFrames;
DELETE FROM RelatedGames;
DELETE FROM LocalizationInfos;
DELETE FROM GameTags;
DELETE FROM Tags;
DELETE FROM SearchHistory;
DELETE FROM DownloadHistory;
DELETE FROM FavoriteGames;
DELETE FROM Reviews;
DELETE FROM GameLinks;
DELETE FROM CrackInfo;
DELETE FROM Features;
DELETE FROM SystemRequirements;
DELETE FROM Screenshots;
DELETE FROM Games;
DELETE FROM Categories;
DELETE FROM Users;
DELETE FROM Roles;

-- Chèn dữ liệu cho bảng Roles
SET IDENTITY_INSERT Roles ON;
INSERT INTO Roles (Id, Name, Description, CreatedAt) VALUES 
(1, 'User', 'Standard user account', '2025-01-01'),
(2, 'Moderator', 'Moderator with content management capabilities', '2025-01-01'),
(3, 'Admin', 'Administrator with full access', '2025-01-01'),
(4, 'Premium', 'Premium user with additional benefits', '2025-01-01');
SET IDENTITY_INSERT Roles OFF;

-- Chèn dữ liệu cho bảng Users
INSERT INTO Users (Id, FirstName, LastName, DisplayName, Email, Bio, AvatarUrl, CreatedAt, PasswordHash, SecurityStamp, NormalizedEmail, EmailConfirmed, RoleId, PremiumExpiryDate, RememberMe) VALUES 
('1', 'John', 'Doe', 'JohnDoe', 'john.doe@example.com', 'Games enthusiast', '/img/avatars/avatar1.png', '2025-01-15', 'AQAAAAIAAYagAAAAELUB1JWg16jQNS/k2Z5vZt3F5LQCqRERYIs3NDJKfMlJLCVjJlVDFLvTeZb7SRQtwQ==', 'QOIRJLM2W42DH4VSAWRWGKUJEWQZF6JR', 'JOHN.DOE@EXAMPLE.COM', 1, 1, NULL, 0),
('2', 'Jane', 'Smith', 'JaneSmith', 'jane.smith@example.com', 'Hardcore gamer', '/img/avatars/avatar2.png', '2025-01-20', 'AQAAAAIAAYagAAAAELUB1JWg16jQNS/k2Z5vZt3F5LQCqRERYIs3NDJKfMlJLCVjJlVDFLvTeZb7SRQtwQ==', 'ABNCOPM2W42DH4VSAWRETKUJEWQZF6JR', 'JANE.SMITH@EXAMPLE.COM', 1, 4, '2025-12-31', 0),
('3', 'Admin', 'User', 'AdminUser', 'admin@crackhub.com', 'Site Administrator', '/img/avatars/admin.png', '2025-01-01', 'AQAAAAIAAYagAAAAELUB1JWg16jQNS/k2Z5vZt3F5LQCqRERYIs3NDJKfMlJLCVjJlVDFLvTeZb7SRQtwQ==', 'ZYXWLKM2W42DH4VSAWRWGKUJEWQZF6JR', 'ADMIN@CRACKHUB.COM', 1, 3, NULL, 0),
('4', 'Mod', 'User', 'ModUser', 'mod@crackhub.com', 'Content Moderator', '/img/avatars/mod.png', '2025-01-05', 'AQAAAAIAAYagAAAAELUB1JWg16jQNS/k2Z5vZt3F5LQCqRERYIs3NDJKfMlJLCVjJlVDFLvTeZb7SRQtwQ==', 'POQRLKM2W42DH4VSAWRWGKUJEWQZF6JR', 'MOD@CRACKHUB.COM', 1, 2, NULL, 0),
('5', 'Alex', 'Johnson', 'AlexJ', 'alex.j@example.com', 'RPG lover', '/img/avatars/avatar3.png', '2025-02-10', 'AQAAAAIAAYagAAAAELUB1JWg16jQNS/k2Z5vZt3F5LQCqRERYIs3NDJKfMlJLCVjJlVDFLvTeZb7SRQtwQ==', 'MNBLKJM2W42DH4VSAWRWGKUJEWQZF6JR', 'ALEX.J@EXAMPLE.COM', 1, 1, NULL, 0);

-- Chèn dữ liệu cho bảng Categories
SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (CategoryId, CategoryName, Description, Slug, IconClass, GameCount) VALUES 
(1, 'Action', 'Fast-paced games focusing on physical challenges', 'action', 'fa-fist-raised', 0),
(2, 'Adventure', 'Games that emphasize exploration and puzzle-solving', 'adventure', 'fa-compass', 0),
(3, 'RPG', 'Games where players assume the roles of characters', 'rpg', 'fa-dice-d20', 0),
(4, 'Strategy', 'Games that focus on skillful thinking and planning', 'strategy', 'fa-chess', 0),
(5, 'Simulation', 'Games designed to simulate real-world activities', 'simulation', 'fa-plane', 0),
(6, 'Sports', 'Games that simulate sports', 'sports', 'fa-futbol', 0),
(7, 'Racing', 'Games that involve competing in races', 'racing', 'fa-car-side', 0),
(8, 'Horror', 'Games designed to scare the player', 'horror', 'fa-ghost', 0);
SET IDENTITY_INSERT Categories OFF;

-- Chèn dữ liệu cho bảng Games
SET IDENTITY_INSERT Games ON;
INSERT INTO Games (Id, Title, ShortDescription, FullDescription, CoverImageUrl, Developer, Publisher, ReleaseDate, CategoryId, Rating, Downloads, DownloadUrl, Size, AverageRating, IsFeatured, CreatedDate) VALUES 
(1, 'Phantom Odyssey', 'Epic sci-fi adventure across the galaxy', 'Embark on an epic journey across the stars in this open-world sci-fi adventure. As Commander Alex Reed, you must navigate political intrigue, alien civilizations, and cosmic mysteries to save humanity from an ancient threat. Features revolutionary AI-driven storytelling that adapts to your choices and playstyle.', '/img/games/phantom_odyssey.jpg', 'Cosmic Studios', 'Stellar Entertainment', '2024-11-15', 2, 4.8, '25670', NULL, '64.7 GB', 4.8, 1, '2025-01-15'),
(2, 'Shadow Tactics: Elite Force', 'Tactical stealth action game', 'Lead a team of elite operatives across high-stakes missions in this tactical stealth game. Plan your approach, utilize each agent''s unique skills, and execute perfect synchronous takedowns. Features 24 challenging missions across diverse environments, from dense urban cities to remote military outposts.', '/img/games/shadow_tactics.jpg', 'Stealth Works', 'Tactical Games', '2024-09-22', 4, 4.5, '18340', NULL, '32.1 GB', 4.5, 1, '2025-01-17'),
(3, 'Crimson Kingdoms', 'Medieval fantasy RPG with deep lore', 'Forge your destiny in this vast open-world fantasy RPG. Create your character, master magical arts or combat skills, and shape the fate of the realm through your choices. Features a living world with complex faction relationships and consequences that resonate throughout your journey.', '/img/games/crimson_kingdoms.jpg', 'Epic Realm Studios', 'Fantasy Works', '2024-12-05', 3, 4.7, '28950', NULL, '78.3 GB', 4.7, 1, '2025-01-20'),
(4, 'Velocity Surge', 'High-octane futuristic racing', 'Race at supersonic speeds through gravity-defying tracks in this futuristic racing experience. Customize your anti-gravity vehicle with performance upgrades and visual modifications. Features online multiplayer with ranked leagues and a championship mode.', '/img/games/velocity_surge.jpg', 'Momentum Games', 'Speed Interactive', '2024-10-18', 7, 4.3, '14590', NULL, '28.5 GB', 4.3, 0, '2025-01-25'),
(5, 'Haunted Hollows', 'Psychological horror in an abandoned town', 'Uncover the disturbing secrets of Raven''s Creek as journalist Emily Hayes. Navigate the fog-shrouded town while evading supernatural entities driven by your own fears. Features innovative sanity mechanics and multiple endings based on your investigative choices.', '/img/games/haunted_hollows.jpg', 'Nightmare Factory', 'Dread Entertainment', '2025-01-10', 8, 4.6, '12780', NULL, '42.9 GB', 4.6, 0, '2025-02-01');
SET IDENTITY_INSERT Games OFF;

-- Chèn dữ liệu cho bảng Screenshots
SET IDENTITY_INSERT Screenshots ON;
INSERT INTO Screenshots (Id, GameId, ImageUrl) VALUES 
(1, 1, '/img/screenshots/phantom_odyssey_1.jpg'),
(2, 1, '/img/screenshots/phantom_odyssey_2.jpg'),
(3, 1, '/img/screenshots/phantom_odyssey_3.jpg'),
(4, 2, '/img/screenshots/shadow_tactics_1.jpg'),
(5, 2, '/img/screenshots/shadow_tactics_2.jpg'),
(6, 3, '/img/screenshots/crimson_kingdoms_1.jpg'),
(7, 3, '/img/screenshots/crimson_kingdoms_2.jpg'),
(8, 3, '/img/screenshots/crimson_kingdoms_3.jpg'),
(9, 4, '/img/screenshots/velocity_surge_1.jpg'),
(10, 4, '/img/screenshots/velocity_surge_2.jpg'),
(11, 5, '/img/screenshots/haunted_hollows_1.jpg'),
(12, 5, '/img/screenshots/haunted_hollows_2.jpg');
SET IDENTITY_INSERT Screenshots OFF;

-- Chèn dữ liệu cho bảng SystemRequirements
SET IDENTITY_INSERT SystemRequirements ON;
INSERT INTO SystemRequirements (Id, GameId, RequirementType, OS, Processor, Memory, Graphics, DirectX, Storage, AdditionalNotes) VALUES 
(1, 1, 'Minimum', 'Windows 10 64-bit', 'Intel Core i5-8400 / AMD Ryzen 5 2600', '16 GB RAM', 'NVIDIA GTX 1060 6GB / AMD RX 580 8GB', 'Version 12', '80 GB', 'SSD recommended'),
(2, 1, 'Recommended', 'Windows 10/11 64-bit', 'Intel Core i7-10700K / AMD Ryzen 7 5800X', '32 GB RAM', 'NVIDIA RTX 3070 / AMD RX 6800 XT', 'Version 12', '80 GB', 'SSD required'),
(3, 2, 'Minimum', 'Windows 10 64-bit', 'Intel Core i5-7500 / AMD Ryzen 5 1600', '8 GB RAM', 'NVIDIA GTX 970 4GB / AMD RX 570 4GB', 'Version 11', '40 GB', NULL),
(4, 2, 'Recommended', 'Windows 10/11 64-bit', 'Intel Core i7-9700K / AMD Ryzen 7 3700X', '16 GB RAM', 'NVIDIA RTX 2060 / AMD RX 5700', 'Version 12', '40 GB', 'SSD recommended'),
(5, 3, 'Minimum', 'Windows 10 64-bit', 'Intel Core i5-8600K / AMD Ryzen 5 3600', '16 GB RAM', 'NVIDIA GTX 1660 Super / AMD RX 5600 XT', 'Version 12', '100 GB', 'SSD recommended'),
(6, 3, 'Recommended', 'Windows 10/11 64-bit', 'Intel Core i7-10700K / AMD Ryzen 7 5800X', '32 GB RAM', 'NVIDIA RTX 3080 / AMD RX 6800 XT', 'Version 12', '100 GB', 'SSD required'),
(7, 4, 'Minimum', 'Windows 10 64-bit', 'Intel Core i5-6600K / AMD Ryzen 3 3300X', '8 GB RAM', 'NVIDIA GTX 1050 Ti / AMD RX 560', 'Version 11', '30 GB', NULL),
(8, 4, 'Recommended', 'Windows 10/11 64-bit', 'Intel Core i5-9600K / AMD Ryzen 5 3600X', '16 GB RAM', 'NVIDIA GTX 1660 Ti / AMD RX 5600 XT', 'Version 12', '30 GB', 'SSD recommended'),
(9, 5, 'Minimum', 'Windows 10 64-bit', 'Intel Core i5-7600K / AMD Ryzen 5 2600', '8 GB RAM', 'NVIDIA GTX 1050 Ti / AMD RX 570', 'Version 11', '50 GB', NULL),
(10, 5, 'Recommended', 'Windows 10/11 64-bit', 'Intel Core i7-9700K / AMD Ryzen 7 3700X', '16 GB RAM', 'NVIDIA RTX 2060 / AMD RX 5700 XT', 'Version 12', '50 GB', 'SSD recommended');
SET IDENTITY_INSERT SystemRequirements OFF;

-- Chèn dữ liệu cho bảng Features
SET IDENTITY_INSERT Features ON;
INSERT INTO Features (Id, GameId, FeatureDescription) VALUES 
(1, 1, 'Explore over 100 unique planets with diverse ecosystems'),
(2, 1, 'Adaptive AI storytelling that responds to player choices'),
(3, 1, 'Epic space combat with fully customizable starships'),
(4, 1, 'Dynamic relationship system with crew members and alien species'),
(5, 2, 'Plan and execute perfect stealth missions with a team of specialists'),
(6, 2, 'Synchronous takedown system for coordinated actions'),
(7, 2, '24 diverse missions across unique environments'),
(8, 3, 'Create your character with extensive customization options'),
(9, 3, 'Vast open world with dynamic weather and day/night cycle'),
(10, 3, 'Complex faction system with reputation mechanics'),
(11, 3, 'Over 200 hours of gameplay with multiple endings'),
(12, 4, '50+ futuristic tracks with dynamic obstacles'),
(13, 4, 'Online multiplayer with ranked leagues and tournaments'),
(14, 4, 'Extensive vehicle customization system'),
(15, 5, 'Psychological horror with sanity management system'),
(16, 5, 'Dynamic entity AI that adapts to player behavior'),
(17, 5, 'Multiple endings based on investigative choices');
SET IDENTITY_INSERT Features OFF;

-- Chèn dữ liệu cho bảng CrackInfo
SET IDENTITY_INSERT CrackInfo ON;
INSERT INTO CrackInfo (Id, GameId, Version, [Group], Description, DownloadUrl, FileSize, ReleaseDate, IsRecommended) VALUES 
(1, 1, 'v1.0.5', 'CODEX', 'Clean crack with all DLCs included', NULL, '58.3 GB', '2024-11-20', 1),
(2, 1, 'v1.1.0', 'PLAZA', 'Latest patch with bug fixes', NULL, '65.2 GB', '2025-01-05', 0),
(3, 2, 'v2.0.0', 'SKIDROW', 'Includes all bonus missions', NULL, '30.7 GB', '2024-09-25', 1),
(4, 3, 'v1.2.1', 'CPY', 'All expansions and language packs included', NULL, '76.9 GB', '2024-12-10', 1),
(5, 4, 'v1.0.0', 'RELOADED', 'Base game with online fix', NULL, '27.3 GB', '2024-10-22', 0),
(6, 4, 'v1.0.2', 'CODEX', 'Latest version with all DLC vehicles', NULL, '29.1 GB', '2024-12-15', 1),
(7, 5, 'v1.0.1', 'EMPRESS', 'Complete edition with developer commentary', NULL, '41.5 GB', '2025-01-15', 1);
SET IDENTITY_INSERT CrackInfo OFF;

-- Chèn dữ liệu cho bảng GameLinks
SET IDENTITY_INSERT GameLinks ON;
INSERT INTO GameLinks (Id, GameId, LinkType, LinkUrl, LinkText, IsDeleted) VALUES 
(1, 1, 'Download', 'https://example.com/downloads/phantom_odyssey', 'Direct Download', 0),
(2, 1, 'Torrent', 'magnet:?xt=urn:btih:123456abcdef', 'Torrent Download', 0),
(3, 2, 'Download', 'https://example.com/downloads/shadow_tactics', 'Direct Download', 0),
(4, 2, 'Torrent', 'magnet:?xt=urn:btih:654321fedcba', 'Torrent Download', 0),
(5, 3, 'Download', 'https://example.com/downloads/crimson_kingdoms', 'Direct Download', 0),
(6, 3, 'Part 1', 'https://example.com/downloads/crimson_kingdoms_part1', 'Part 1 (38GB)', 0),
(7, 3, 'Part 2', 'https://example.com/downloads/crimson_kingdoms_part2', 'Part 2 (40GB)', 0),
(8, 4, 'Download', 'https://example.com/downloads/velocity_surge', 'Direct Download', 0),
(9, 5, 'Download', 'https://example.com/downloads/haunted_hollows', 'Direct Download', 0),
(10, 5, 'Torrent', 'magnet:?xt=urn:btih:789123abcdef', 'Torrent Download', 0);
SET IDENTITY_INSERT GameLinks OFF;

-- Chèn dữ liệu cho bảng Reviews
SET IDENTITY_INSERT Reviews ON;
INSERT INTO Reviews (Id, GameId, UserId, Title, Content, Rating, DatePosted, HelpfulCount) VALUES 
(1, 1, '1', 'Mindblowing space adventure', 'The most immersive sci-fi experience I''ve ever played. The attention to detail in planet design is incredible, and the story truly reacts to your choices in meaningful ways.', 5.0, '2025-01-20', 42),
(2, 1, '2', 'Almost perfect', 'Beautiful visuals and compelling story, but occasional performance issues on my system. Still, highly recommended for any sci-fi fan.', 4.5, '2025-01-25', 18),
(3, 2, '5', 'Strategic masterpiece', 'The synchronized takedown system is so satisfying when you pull off a perfect mission. Challenging in the best way possible.', 5.0, '2025-01-30', 27),
(4, 3, '1', 'Lost in a fantasy world', 'I''ve spent 120 hours in this game and still discovering new areas and quests. The faction system makes every choice feel consequential.', 4.8, '2025-02-05', 35),
(5, 3, '4', 'RPG fans will love this', 'Deep character progression and meaningful choices. Some minor bugs, but the developers are actively patching them.', 4.5, '2025-02-10', 19),
(6, 4, '2', 'Pure adrenaline racing', 'The sense of speed is incredible, and the track design is creative and challenging. Multiplayer is addictive!', 4.7, '2025-02-15', 15),
(7, 5, '5', 'Genuinely terrifying', 'I had to take breaks because the atmosphere is so tense. The sanity system creates moments of pure dread.', 4.9, '2025-02-20', 31),
(8, 5, '1', 'Horror done right', 'Relies on psychological horror rather than cheap jump scares. The multiple endings add great replay value.', 4.7, '2025-02-25', 22);
SET IDENTITY_INSERT Reviews OFF;

-- Chèn dữ liệu cho bảng FavoriteGames
INSERT INTO FavoriteGames (UserId, GameId, DateAdded) VALUES 
('1', 1, '2025-01-22'),
('1', 3, '2025-02-06'),
('2', 1, '2025-01-26'),
('2', 4, '2025-02-16'),
('5', 3, '2025-02-12'),
('5', 5, '2025-02-21');

-- Chèn dữ liệu cho bảng DownloadHistory
SET IDENTITY_INSERT DownloadHistory ON;
INSERT INTO DownloadHistory (Id, UserId, GameId, DownloadDate, DownloadedFileUrl) VALUES 
(1, '1', 1, '2025-01-22', 'https://example.com/downloads/phantom_odyssey'),
(2, '1', 3, '2025-02-06', 'https://example.com/downloads/crimson_kingdoms'),
(3, '2', 1, '2025-01-26', 'https://example.com/downloads/phantom_odyssey'),
(4, '2', 4, '2025-02-16', 'https://example.com/downloads/velocity_surge'),
(5, '5', 3, '2025-02-12', 'https://example.com/downloads/crimson_kingdoms'),
(6, '5', 5, '2025-02-21', 'https://example.com/downloads/haunted_hollows'),
(7, '4', 2, '2025-01-30', 'https://example.com/downloads/shadow_tactics'),
(8, '3', 3, '2025-02-05', 'https://example.com/downloads/crimson_kingdoms');
SET IDENTITY_INSERT DownloadHistory OFF;

-- Chèn dữ liệu cho bảng SearchHistory
SET IDENTITY_INSERT SearchHistory ON;
INSERT INTO SearchHistory (Id, UserId, SearchQuery, SearchDate) VALUES 
(1, '1', 'space adventure games', '2025-01-15'),
(2, '1', 'RPG open world fantasy', '2025-02-01'),
(3, '2', 'sci-fi exploration games', '2025-01-20'),
(4, '2', 'racing games 2024', '2025-02-10'),
(5, '5', 'best fantasy RPGs', '2025-02-05'),
(6, '5', 'horror games psychological', '2025-02-18'),
(7, '4', 'tactical stealth games', '2025-01-25'),
(8, '3', 'medieval fantasy games', '2025-02-03');
SET IDENTITY_INSERT SearchHistory OFF;

-- Chèn dữ liệu cho bảng Tags
SET IDENTITY_INSERT Tags ON;
INSERT INTO Tags (Id, Name, Slug) VALUES 
(1, 'Open World', 'open-world'),
(2, 'Sci-Fi', 'sci-fi'),
(3, 'Fantasy', 'fantasy'),
(4, 'Stealth', 'stealth'),
(5, 'Tactical', 'tactical'),
(6, 'Racing', 'racing'),
(7, 'Horror', 'horror'),
(8, 'RPG', 'rpg'),
(9, 'Action', 'action'),
(10, 'Adventure', 'adventure'),
(11, 'Multiplayer', 'multiplayer'),
(12, 'Singleplayer', 'singleplayer'),
(13, 'Atmospheric', 'atmospheric'),
(14, 'Story-Rich', 'story-rich'),
(15, 'Futuristic', 'futuristic');
SET IDENTITY_INSERT Tags OFF;

-- Chèn dữ liệu cho bảng GameTags
INSERT INTO GameTags (GameId, TagId) VALUES 
(1, 1), (1, 2), (1, 10), (1, 12), (1, 13), (1, 14),
(2, 4), (2, 5), (2, 9), (2, 12),
(3, 1), (3, 3), (3, 8), (3, 10), (3, 12), (3, 14),
(4, 6), (4, 11), (4, 15), (4, 9),
(5, 7), (5, 13), (5, 14), (5, 12);

-- Chèn dữ liệu cho bảng LocalizationInfos
SET IDENTITY_INSERT LocalizationInfos ON;
INSERT INTO LocalizationInfos (Id, GameId, Language, TextLocalized, AudioLocalized) VALUES 
(1, 1, 'English', 1, 1),
(2, 1, 'Spanish', 1, 1),
(3, 1, 'French', 1, 1),
(4, 1, 'German', 1, 1),
(5, 1, 'Japanese', 1, 1),
(6, 1, 'Chinese', 1, 0),
(7, 2, 'English', 1, 1),
(8, 2, 'German', 1, 1),
(9, 2, 'Spanish', 1, 0),
(10, 3, 'English', 1, 1),
(11, 3, 'French', 1, 1),
(12, 3, 'German', 1, 1),
(13, 3, 'Spanish', 1, 1),
(14, 3, 'Italian', 1, 0),
(15, 3, 'Russian', 1, 0),
(16, 4, 'English', 1, 1),
(17, 4, 'French', 1, 0),
(18, 4, 'Japanese', 1, 0),
(19, 5, 'English', 1, 1),
(20, 5, 'Spanish', 1, 1),
(21, 5, 'Japanese', 1, 1);
SET IDENTITY_INSERT LocalizationInfos OFF;

-- Chèn dữ liệu cho bảng RelatedGames
SET IDENTITY_INSERT RelatedGames ON;
INSERT INTO RelatedGames (Id, GameId, RelatedGameId) VALUES 
(1, 1, 3),
(2, 1, 2),
(3, 2, 1),
(4, 3, 1),
(5, 3, 5),
(6, 4, 2),
(7, 5, 3);
SET IDENTITY_INSERT RelatedGames OFF;

-- Chèn dữ liệu cho bảng AvatarFrames
SET IDENTITY_INSERT AvatarFrames ON;
INSERT INTO AvatarFrames (Id, FrameName, FrameUrl, Description, RarityLevel, IsDefault, RequiredLevel, IsPremium, CreatedAt, IsActive) VALUES 
(1, 'Standard Frame', '/img/frames/standard.png', 'Default avatar frame for all users', 1, 1, NULL, 0, '2025-01-01', 1),
(2, 'Gold Elite', '/img/frames/gold_elite.png', 'Premium gold border with animated effects', 5, 0, 15, 1, '2025-01-01', 1),
(3, 'Silver Star', '/img/frames/silver_star.png', 'Silver frame with star accents', 3, 0, 5, 0, '2025-01-01', 1),
(4, 'Ruby Frame', '/img/frames/ruby.png', 'Deep red frame with gem-like finish', 4, 0, 10, 1, '2025-01-01', 1),
(5, 'Sapphire Elegance', '/img/frames/sapphire.png', 'Blue glowing frame with elegant design', 4, 0, 10, 1, '2025-01-01', 1),
(6, 'Bronze Basic', '/img/frames/bronze.png', 'Simple bronze frame for new users', 2, 0, 1, 0, '2025-01-01', 1);
SET IDENTITY_INSERT AvatarFrames OFF;

-- Chèn dữ liệu cho bảng UserAvatarFrames
INSERT INTO UserAvatarFrames (UserId, FrameId, IsEquipped, ObtainedDate) VALUES 
('1', 1, 0, '2025-01-15'),
('1', 3, 1, '2025-01-30'),
('2', 1, 0, '2025-01-20'),
('2', 2, 1, '2025-01-25'),
('3', 1, 0, '2025-01-01'),
('3', 4, 1, '2025-01-05'),
('4', 1, 1, '2025-01-05'),
('5', 1, 0, '2025-02-10'),
('5', 6, 1, '2025-02-15');

-- Cập nhật bảng Games để tính toán lại AverageRating
UPDATE Games SET AverageRating = (
    SELECT AVG(Rating) FROM Reviews WHERE GameId = Games.Id
);

-- Cập nhật bảng Categories để cập nhật lại GameCount
UPDATE Categories SET GameCount = (
    SELECT COUNT(*) FROM Games WHERE CategoryId = Categories.CategoryId
);