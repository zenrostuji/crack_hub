-- Tạo cơ sở dữ liệu WebDownloadGame
CREATE DATABASE WebDownloadGame;
GO

-- Sử dụng cơ sở dữ liệu WebDownloadGame
USE WebDownloadGame;
GO

-- Bảng Roles (Vai trò người dùng)
-- Mô tả: Quản lý các vai trò khác nhau trong hệ thống (Thường, Premium, Admin)
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),          -- Định danh vai trò, tự động tăng
    Name NVARCHAR(50) NOT NULL,                -- Tên vai trò (Regular, Premium, Admin)
    Description NVARCHAR(MAX),                 -- Mô tả chi tiết về vai trò
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE() -- Ngày tạo vai trò
);

-- Bảng Users (Người dùng)
-- Mô tả: Lưu trữ thông tin người dùng và tài khoản đăng nhập
CREATE TABLE Users (
    Id NVARCHAR(450) PRIMARY KEY,              -- Định danh người dùng (GUID/UUID)
    FirstName NVARCHAR(100) NOT NULL,          -- Tên
    LastName NVARCHAR(100) NOT NULL,           -- Họ
    DisplayName NVARCHAR(100) NOT NULL,        -- Tên hiển thị
    Email NVARCHAR(256),                       -- Email đăng nhập
    Bio NVARCHAR(MAX),                         -- Tiểu sử/giới thiệu
    AvatarUrl NVARCHAR(MAX),                   -- URL ảnh đại diện
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), -- Ngày tạo tài khoản
    PasswordHash NVARCHAR(MAX),                -- Mật khẩu đã được mã hóa
    SecurityStamp NVARCHAR(MAX),               -- Dấu bảo mật cho xác thực
    NormalizedEmail NVARCHAR(256),             -- Email chuẩn hóa cho tìm kiếm
    EmailConfirmed BIT NOT NULL DEFAULT 0,     -- Xác nhận email (0: Chưa, 1: Đã)
    RoleId INT NOT NULL DEFAULT 1,             -- Vai trò (1: Thường, 2: Premium, 3: Admin)
    PremiumExpiryDate DATETIME NULL,           -- Ngày hết hạn tài khoản Premium
    RememberMe BIT NOT NULL DEFAULT 0,         -- Nhớ đăng nhập
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)  -- Liên kết với bảng Roles
);

-- Bảng Categories (Thể loại game)
-- Mô tả: Phân loại game theo thể loại
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),  -- Định danh thể loại, tự động tăng
    CategoryName NVARCHAR(100) NOT NULL,       -- Tên thể loại (Hành động, Nhập vai,...)
    Description NVARCHAR(MAX),                 -- Mô tả chi tiết về thể loại
    Slug NVARCHAR(100) NOT NULL,               -- Chuỗi SEO-friendly cho URL
    IconClass NVARCHAR(50),                    -- Class CSS cho biểu tượng thể loại
    GameCount INT DEFAULT 0                    -- Số lượng game thuộc thể loại này
);

-- Bảng Games (Thông tin game)
-- Mô tả: Lưu trữ thông tin cơ bản về các trò chơi
CREATE TABLE Games (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    ShortDescription NVARCHAR(500),
    FullDescription NVARCHAR(MAX),
    CoverImageUrl NVARCHAR(MAX),
    Developer NVARCHAR(100),
    Publisher NVARCHAR(100),
    ReleaseDate DATETIME,
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    Rating FLOAT DEFAULT 0,
    Downloads NVARCHAR(50) DEFAULT '0',       -- Giá trị hiển thị số lượt tải (ví dụ: "1.2M")
    DownloadUrl NVARCHAR(MAX),                -- URL để tải game
    Size NVARCHAR(50),                        -- Kích thước game (ví dụ: "70.12 GB")
    AverageRating FLOAT DEFAULT 0             -- Đánh giá trung bình từ người dùng
);

-- Bảng SystemRequirements (Cấu hình yêu cầu)
-- Mô tả: Lưu trữ thông tin về cấu hình tối thiểu và đề xuất cho game
CREATE TABLE SystemRequirements (
    Id INT PRIMARY KEY IDENTITY(1,1),
    GameId INT NOT NULL,
    RequirementType NVARCHAR(50) NOT NULL, -- 'Minimum' or 'Recommended'
    OS NVARCHAR(100),
    Processor NVARCHAR(100),
    Memory NVARCHAR(100),
    Graphics NVARCHAR(100),
    DirectX NVARCHAR(100),
    Storage NVARCHAR(100),
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Bảng Features (Tính năng game)
-- Mô tả: Lưu trữ các tính năng đặc biệt của game
CREATE TABLE Features (
    Id INT PRIMARY KEY IDENTITY(1,1),
    GameId INT NOT NULL,
    FeatureDescription NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Bảng Screenshots (Ảnh chụp màn hình)
-- Mô tả: Lưu trữ đường dẫn đến các ảnh chụp màn hình của game
CREATE TABLE Screenshots (
    Id INT PRIMARY KEY IDENTITY(1,1),
    GameId INT NOT NULL,
    ImageUrl NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Bảng Reviews (Đánh giá)
-- Mô tả: Lưu trữ đánh giá và nhận xét của người dùng về game
CREATE TABLE Reviews (
    Id INT PRIMARY KEY IDENTITY(1,1),
    GameId INT NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    Title NVARCHAR(100),
    Content NVARCHAR(MAX),
    Rating FLOAT NOT NULL,
    DatePosted DATETIME NOT NULL DEFAULT GETDATE(),
    HelpfulCount INT DEFAULT 0,
    FOREIGN KEY (GameId) REFERENCES Games(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Bảng CrackInfo (Thông tin về bản crack)
-- Mô tả: Lưu trữ thông tin về bản crack của game (phiên bản, nhóm crack,...)
CREATE TABLE CrackInfo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    GameId INT NOT NULL,
    Version NVARCHAR(50),
    [Group] NVARCHAR(100),
    Description NVARCHAR(MAX),
    DownloadUrl NVARCHAR(MAX),
    FileSize NVARCHAR(50),
    ReleaseDate DATETIME,
    IsRecommended BIT DEFAULT 0,
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Bảng FavoriteGames (Game yêu thích)
-- Mô tả: Theo dõi game yêu thích của người dùng
CREATE TABLE FavoriteGames (
    UserId NVARCHAR(450) NOT NULL,
    GameId INT NOT NULL,
    DateAdded DATETIME NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (UserId, GameId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Bảng DownloadHistory (Lịch sử tải)
-- Mô tả: Ghi lại lịch sử tải game của người dùng
CREATE TABLE DownloadHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NOT NULL,
    GameId INT NOT NULL,
    DownloadDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Bảng GameLinks (Liên kết tải game)
-- Mô tả: Lưu trữ các liên kết để tải game từ các nguồn khác nhau
CREATE TABLE GameLinks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    GameId INT NOT NULL,
    LinkName NVARCHAR(100) NOT NULL, -- E.g., "Google Drive", "MediaFire", "MEGA", etc.
    Url NVARCHAR(MAX) NOT NULL,
    FileSize NVARCHAR(50),
    PartNumber INT DEFAULT 1, -- For split archives (Part 1, Part 2, etc.)
    TotalParts INT DEFAULT 1,
    Password NVARCHAR(100), -- Archive password if required
    IsActive BIT DEFAULT 1,
    DateAdded DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Bảng Tags (Thẻ)
-- Mô tả: Lưu trữ các thẻ mô tả đặc điểm của game
CREATE TABLE Tags (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Slug NVARCHAR(50) NOT NULL
);

-- Bảng GameTags (Mối quan hệ Game-Thẻ)
-- Mô tả: Bảng liên kết giữa game và thẻ (quan hệ nhiều-nhiều)
CREATE TABLE GameTags (
    GameId INT NOT NULL,
    TagId INT NOT NULL,
    PRIMARY KEY (GameId, TagId),
    FOREIGN KEY (GameId) REFERENCES Games(Id),
    FOREIGN KEY (TagId) REFERENCES Tags(Id)
);

-- Bảng PopularTags (Thẻ phổ biến cho tìm kiếm)
-- Mô tả: Lưu trữ thẻ phổ biến cho kết quả tìm kiếm
CREATE TABLE PopularTags (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TagName NVARCHAR(50) NOT NULL,
    SearchCount INT DEFAULT 0,
    IsActive BIT DEFAULT 1
);

-- Bảng LocalizationInfo (Thông tin Việt hóa)
-- Mô tả: Lưu trữ thông tin về bản việt hóa của game
CREATE TABLE LocalizationInfo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    GameId INT NOT NULL,
    LocalizationType NVARCHAR(50) NOT NULL, -- 'Đầy đủ', 'Một phần', 'Menu & UI',...
    LocalizedBy NVARCHAR(100),              -- Người/nhóm thực hiện việt hóa
    LocalizationVersion NVARCHAR(50),       -- Phiên bản việt hóa
    LocalizationDate DATETIME,              -- Ngày việt hóa
    Description NVARCHAR(MAX),              -- Mô tả về bản việt hóa
    InstallationGuide NVARCHAR(MAX),        -- Hướng dẫn cài đặt bản việt hóa
    DownloadUrl NVARCHAR(MAX),              -- Link tải bản việt hóa
    FileSize NVARCHAR(50),                  -- Dung lượng file việt hóa
    IsComplete BIT DEFAULT 0,               -- Việt hóa đã hoàn thiện chưa?
    FOREIGN KEY (GameId) REFERENCES Games(Id)
);

-- Bảng RelatedGames (Liên kết các game liên quan)
-- Mô tả: Liên kết các game liên quan với nhau
CREATE TABLE RelatedGames (
    GameId INT NOT NULL,
    RelatedGameId INT NOT NULL,
    RelationStrength INT DEFAULT 1, -- Mức độ liên quan (1-10)
    PRIMARY KEY (GameId, RelatedGameId),
    FOREIGN KEY (GameId) REFERENCES Games(Id),
    FOREIGN KEY (RelatedGameId) REFERENCES Games(Id)
);

-- Bảng SearchHistory (Lịch sử tìm kiếm)
-- Mô tả: Lưu lịch sử tìm kiếm của người dùng
CREATE TABLE SearchHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NULL, -- Có thể null nếu người dùng không đăng nhập
    SearchTerm NVARCHAR(255) NOT NULL,
    SearchDate DATETIME NOT NULL DEFAULT GETDATE(),
    ResultCount INT DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Chèn dữ liệu mẫu cho vai trò người dùng
-- Mô tả: Tạo 3 vai trò mặc định: Thường, Premium và Admin
INSERT INTO Roles (Name, Description)
VALUES 
    (N'Regular', N'Người dùng thường với quyền truy cập cơ bản vào trang web'),
    (N'Premium', N'Người dùng premium với các tính năng và lợi ích bổ sung'),
    (N'Admin', N'Quản trị viên với toàn quyền kiểm soát trang web');

-- Chèn dữ liệu mẫu cho thể loại game
-- Mô tả: Tạo các thể loại game phổ biến
INSERT INTO Categories (CategoryName, Description, Slug, IconClass, GameCount)
VALUES 
    (N'Hành Động', N'Game hành động với nhịp độ nhanh và kích thích', N'hanh-dong', N'fas fa-running', 25),
    (N'Nhập Vai', N'Game nhập vai với cốt truyện sâu sắc và phát triển nhân vật', N'nhap-vai', N'fas fa-dragon', 18),
    (N'Phiêu Lưu', N'Game phiêu lưu với các thử thách và khám phá', N'phieu-luu', N'fas fa-mountain', 15),
    (N'Chiến Thuật', N'Game chiến thuật đòi hỏi tư duy và lập kế hoạch', N'chien-thuat', N'fas fa-chess', 12),
    (N'Thể Thao', N'Game thể thao mô phỏng các môn thể thao thực tế', N'the-thao', N'fas fa-football-ball', 10),
    (N'Mô Phỏng', N'Game mô phỏng các hoạt động và trải nghiệm thực tế', N'mo-phong', N'fas fa-gamepad', 14),
    (N'Giải Đố', N'Game giải đố thử thách trí tuệ', N'giai-do', N'fas fa-puzzle-piece', 8),
    (N'Kinh Dị', N'Game kinh dị với bầu không khí căng thẳng và đáng sợ', N'kinh-di', N'fas fa-ghost', 9),
    (N'Indie', N'Game độc lập từ các nhà phát triển nhỏ', N'indie', N'fas fa-university', 20),
    (N'Sinh Tồn', N'Game sinh tồn với các cơ chế quản lý tài nguyên', N'sinh-ton', N'fas fa-tree', 11),
    (N'Việt Hóa', N'Game đã được dịch và Việt hóa sang tiếng Việt', N'viet-hoa', N'fas fa-language', 30),
    (N'FPS', N'Game bắn súng góc nhìn thứ nhất', N'fps', N'fas fa-crosshairs', 15),
    (N'Battle Royale', N'Game battle royale với hình thức sống sót cuối cùng', N'battle-royale', N'fas fa-users-slash', 7),
    (N'Đua Xe', N'Game đua xe với nhiều loại phương tiện khác nhau', N'dua-xe', N'fas fa-car', 8),
    (N'Sandbox', N'Game thế giới mở cho phép tự do sáng tạo', N'sandbox', N'fas fa-cubes', 9),
    (N'RPG', N'Game nhập vai với hệ thống nâng cấp nhân vật', N'rpg', N'fas fa-hat-wizard', 22);

-- Chèn dữ liệu mẫu cho thẻ
-- Mô tả: Tạo các thẻ phổ biến để phân loại game
INSERT INTO Tags (Name, Slug)
VALUES
    (N'Multiplayer', N'multiplayer'),
    (N'Single Player', N'single-player'),
    (N'Open World', N'open-world'),
    (N'First Person', N'first-person'),
    (N'Third Person', N'third-person'),
    (N'Co-op', N'co-op'),
    (N'Battle Royale', N'battle-royale'),
    (N'Sandbox', N'sandbox'),
    (N'Stealth', N'stealth'),
    (N'VR Support', N'vr-support'),
    (N'Offline', N'offline'),
    (N'Story Rich', N'story-rich'),
    (N'Competitive', N'competitive'),
    (N'Action RPG', N'action-rpg'),
    (N'Simulation', N'simulation'),
    (N'Free to Play', N'free-to-play'),
    (N'Việt Hóa', N'viet-hoa'),
    (N'Horror', N'horror'),
    (N'Racing', N'racing'),
    (N'Sports', N'sports');

-- Thêm các thẻ phổ biến
INSERT INTO PopularTags (TagName, SearchCount, IsActive)
VALUES 
    (N'Open World', 1250, 1),
    (N'Việt Hóa', 980, 1),
    (N'Multiplayer', 870, 1),
    (N'FPS', 760, 1),
    (N'RPG', 650, 1),
    (N'Story Rich', 580, 1),
    (N'Horror', 450, 1),
    (N'Free to Play', 620, 1),
    (N'Racing', 380, 1),
    (N'Co-op', 490, 1);

-- Thêm dữ liệu mẫu cho bảng Users
-- Mô tả: Tạo một số người dùng mẫu với các vai trò khác nhau
INSERT INTO Users (Id, FirstName, LastName, DisplayName, Email, Bio, AvatarUrl, PasswordHash, SecurityStamp, NormalizedEmail, EmailConfirmed, RoleId)
VALUES 
    (N'1', N'Quản', N'Trị', N'Admin', N'admin@gamedownload.vn', N'Quản trị viên của trang web', N'/img/avatars/admin.jpg', N'AQAAAAIAAYagAAAAELNkx65qoomSNC1vSbaTngNud0Xa0j1NyYWm1KjG66TUP0oFt0kT+JkZpOLzM7Wluw==', N'NKEQBMO6DGGBIQVWTZDJ6WEKH4LMRJMC', N'ADMIN@GAMEDOWNLOAD.VN', 1, 3),
    (N'2', N'Hồng', N'Nguyễn', N'Hong_Premium', N'hong@gmail.com', N'Game thủ chuyên nghiệp', N'/img/avatars/premium1.jpg', N'AQAAAAIAAYagAAAAEN5D0cB++VJP3F1XKdU/tc9S/AkJNG8FUQdXVLLw9RBmGLvEfR2XtcB71RffMyJK6Q==', N'YUKQDZVJSMA5PPRWWPTIO2D5OJAEZM7K', N'HONG@GMAIL.COM', 1, 2),
    (N'3', N'Minh', N'Lê', N'MinhGamer', N'minh@hotmail.com', N'Đam mê game AAA', N'/img/avatars/user1.jpg', N'AQAAAAIAAYagAAAAECVfZoIDaUBgqAZvQZGFP0fQjAyryXytdzlTIPXqU04x4Esx+WBGpGLqTb7ty96Yjg==', N'BLGA3XZXJ2LI7CRXBWCMFDSSK4MSD7YD', N'MINH@HOTMAIL.COM', 1, 1),
    (N'4', N'Thảo', N'Trần', N'ThaoGaming', N'thao@yahoo.com', N'Chuyên game indie', N'/img/default-avatar.png', N'AQAAAAIAAYagAAAAEGrf+P5iJ3XYdYR+np2ci+yBKFMJXkQ9YVpNcz2aGo0d2cwemzehId+nUJ388zANsA==', N'Z4KCUCISL6BQ6LVBPFMKVU4KSCGZMNYX', N'THAO@YAHOO.COM', 1, 1),
    (N'5', N'Tuấn', N'Phạm', N'TuanPro', N'tuan@outlook.com', N'Reviewer game', N'/img/avatars/user2.jpg', N'AQAAAAIAAYagAAAAEE797xdf3RZaKxvigkbdkl0dMvSN7xbEc5eIn5J1WZfP2ScXM9YDRGZT2vTtqGmQnA==', N'XYZ5NGGWCOJ5KTVFAPTJZGHWWL5FSPEX', N'TUAN@OUTLOOK.COM', 1, 2);

-- Thêm dữ liệu mẫu cho bảng Games
-- Mô tả: Thêm thông tin về các game phổ biến
INSERT INTO Games (Title, ShortDescription, FullDescription, CoverImageUrl, Developer, Publisher, ReleaseDate, CategoryId, Rating, Downloads, Size, AverageRating)
VALUES 
    (N'The Witcher 3: Wild Hunt', 
     N'Một trò chơi nhập vai thế giới mở hoành tráng với cốt truyện phong phú và thế giới rộng lớn để khám phá.', 
     N'The Witcher 3: Wild Hunt là một trò chơi nhập vai thế giới mở năm 2015 được phát triển bởi CD Projekt Red. Lấy bối cảnh trong một thế giới giả tưởng dựa trên thần thoại Slav, người chơi điều khiển Geralt xứ Rivia, một thợ săn quái vật được gọi là "witcher" phải tìm con gái nuôi của anh ta, Ciri, đang bị Wildchase truy đuổi, một lực lượng ma quái bí ẩn đe dọa thế giới.', 
     N'/img/games/thewitcher3.jpg', 
     N'CD Projekt Red', 
     N'CD Projekt', 
     '2015-05-19', 
     2, 
     9.5, 
     N'5.2M', 
     N'50 GB', 
     9.3),
     
    (N'Grand Theft Auto V', 
     N'Một trò chơi hành động phiêu lưu thế giới mở, diễn ra tại thành phố hư cấu Los Santos.', 
     N'Grand Theft Auto V là một tựa game hành động-phiêu lưu năm 2013 được phát triển bởi Rockstar North và được xuất bản bởi Rockstar Games. Là phần chính thứ bảy trong series Grand Theft Auto, trò chơi có ba nhân vật chính: tên tội phạm đã về hưu Michael De Santa, gangster đường phố Franklin Clinton và gã buôn ma túy Trevor Philips, kể về việc họ thực hiện các vụ cướp dưới áp lực của một cơ quan chính phủ tham nhũng và những tên tội phạm mạnh mẽ.', 
     N'/img/games/gta5.jpg', 
     N'Rockstar North', 
     N'Rockstar Games', 
     '2013-09-17', 
     1, 
     9.6, 
     N'8.7M', 
     N'72 GB', 
     9.5),
     
    (N'Cyberpunk 2077', 
     N'Một trò chơi nhập vai hành động thế giới mở lấy bối cảnh trong một thành phố đầy rẫy tội phạm và rất nhiều công nghệ cao.', 
     N'Cyberpunk 2077 là một trò chơi nhập vai hành động thế giới mở lấy bối cảnh ở Night City, một đô thị cyberpunk nơi quyền lực, sự xa hoa, và những phẫu thuật cơ thể đang là những thứ được săn đón. Bạn đóng vai V, một lính đánh thuê ngoài vòng pháp luật đang theo đuổi một bộ phận cấy ghép độc nhất vô nhị là chìa khóa dẫn đến sự bất tử.', 
     N'/img/games/cyberpunk2077.jpg', 
     N'CD Projekt Red', 
     N'CD Projekt', 
     '2020-12-10', 
     1, 
     8.7, 
     N'4.1M', 
     N'70 GB', 
     7.9),
     
    (N'FIFA 23', 
     N'Trò chơi bóng đá mô phỏng mới nhất từ EA Sports với các giải đấu, câu lạc bộ và cầu thủ chính thức.', 
     N'FIFA 23 là một trò chơi mô phỏng bóng đá được phát triển bởi EA Vancouver và EA Romania và được xuất bản bởi EA Sports. Đây là phần thứ 30 và cũng là phần cuối cùng trong loạt trò chơi FIFA do EA phát triển. Được hỗ trợ bởi công nghệ HyperMotion2, FIFA 23 mang đến trải nghiệm bóng đá thực tế hơn với các hoạt ảnh cầu thủ chân thực được xây dựng từ dữ liệu ghi nhận từ các trận đấu thực tế.', 
     N'/img/games/fifa23.jpg', 
     N'EA Vancouver', 
     N'EA Sports', 
     '2022-09-30', 
     5, 
     8.5, 
     N'3.8M', 
     N'50 GB', 
     8.2),
     
    (N'Minecraft', 
     N'Một trò chơi sandbox cho phép người chơi khám phá, xây dựng và sinh tồn trong một thế giới khối hình học.', 
     N'Minecraft là một trò chơi sandbox được phát triển bởi Mojang Studios. Trò chơi được tạo ra bởi Markus "Notch" Persson trong ngôn ngữ lập trình Java. Trong Minecraft, người chơi khám phá một thế giới block 3D được tạo ra theo cách khởi tạo thủ tục, khám phá và khai thác nguyên liệu thô, chế tạo các công cụ và vật phẩm, xây dựng các công trình, và tùy chọn có thể chiến đấu với quái vật máy tính.', 
     N'/img/games/minecraft.jpg', 
     N'Mojang Studios', 
     N'Mojang Studios', 
     '2011-11-18', 
     15, 
     9.2, 
     N'10.5M', 
     N'2 GB', 
     9.4);

-- Thêm dữ liệu mẫu cho bảng SystemRequirements
-- Mô tả: Thêm cấu hình tối thiểu và đề xuất cho các game
INSERT INTO SystemRequirements (GameId, RequirementType, OS, Processor, Memory, Graphics, DirectX, Storage)
VALUES
    -- The Witcher 3
    (1, N'Minimum', N'Windows 7/8/10 (64-bit)', N'Intel CPU Core i5-2500K 3.3GHz / AMD CPU Phenom II X4 940', N'6 GB RAM', N'Nvidia GPU GeForce GTX 660 / AMD GPU Radeon HD 7870', N'11', N'35 GB'),
    (1, N'Recommended', N'Windows 7/8/10 (64-bit)', N'Intel CPU Core i7 3770 3.4 GHz / AMD CPU AMD FX-8350 4 GHz', N'8 GB RAM', N'Nvidia GPU GeForce GTX 770 / AMD GPU Radeon R9 290', N'11', N'50 GB'),
    
    -- GTA 5
    (2, N'Minimum', N'Windows 10 64-bit', N'Intel Core i5-4460 (4 CPU) @ 3.40GHz / AMD Ryzen 3 1200', N'8 GB RAM', N'NVIDIA GTX 750 Ti 2GB / AMD Radeon R7 260x 2GB', N'11', N'72 GB'),
    (2, N'Recommended', N'Windows 10 64-bit', N'Intel Core i7-4770 / AMD Ryzen 5 1500X', N'16 GB RAM', N'NVIDIA GTX 1060 6GB / AMD RX 580 4GB', N'11', N'72 GB'),
    
    -- Cyberpunk 2077
    (3, N'Minimum', N'Windows 10 (64-bit)', N'Intel Core i5-3570K / AMD FX-8310', N'8 GB RAM', N'NVIDIA GeForce GTX 780 3GB / AMD Radeon RX 470', N'12', N'70 GB SSD'),
    (3, N'Recommended', N'Windows 10 (64-bit)', N'Intel Core i7-4790 / AMD Ryzen 3 3200G', N'12 GB RAM', N'NVIDIA GeForce GTX 1060 6GB / AMD Radeon R9 Fury', N'12', N'70 GB SSD'),
    
    -- FIFA 23
    (4, N'Minimum', N'Windows 10 64-bit', N'Intel Core i5-6600K / AMD Ryzen 5 1600', N'8 GB RAM', N'NVIDIA GeForce GTX 1050 Ti / AMD Radeon RX 570', N'12', N'50 GB'),
    (4, N'Recommended', N'Windows 10 64-bit', N'Intel Core i7-6700 / AMD Ryzen 7 2700X', N'12 GB RAM', N'NVIDIA GeForce GTX 1660 / AMD Radeon RX 5600 XT', N'12', N'50 GB'),
    
    -- Minecraft
    (5, N'Minimum', N'Windows 7 or later', N'Intel Core i3-3210 / AMD A8-7600', N'4 GB RAM', N'Integrated: Intel HD Graphics 4000 / AMD Radeon R5', N'11', N'1 GB'),
    (5, N'Recommended', N'Windows 10', N'Intel Core i5-4690 / AMD A10-7800', N'8 GB RAM', N'NVIDIA GeForce GTX 700 Series / AMD Radeon Rx 200 Series', N'11', N'4 GB');

-- Thêm dữ liệu mẫu cho bảng Screenshots
-- Mô tả: Thêm đường dẫn ảnh chụp màn hình cho các game
INSERT INTO Screenshots (GameId, ImageUrl)
VALUES
    (1, N'/img/screenshots/witcher3_ss1.jpg'),
    (1, N'/img/screenshots/witcher3_ss2.jpg'),
    (1, N'/img/screenshots/witcher3_ss3.jpg'),
    (1, N'/img/screenshots/witcher3_ss4.jpg'),
    
    (2, N'/img/screenshots/gta5_ss1.jpg'),
    (2, N'/img/screenshots/gta5_ss2.jpg'),
    (2, N'/img/screenshots/gta5_ss3.jpg'),
    (2, N'/img/screenshots/gta5_ss4.jpg'),
    
    (3, N'/img/screenshots/cyberpunk_ss1.jpg'),
    (3, N'/img/screenshots/cyberpunk_ss2.jpg'),
    (3, N'/img/screenshots/cyberpunk_ss3.jpg'),
    
    (4, N'/img/screenshots/fifa23_ss1.jpg'),
    (4, N'/img/screenshots/fifa23_ss2.jpg'),
    (4, N'/img/screenshots/fifa23_ss3.jpg'),
    
    (5, N'/img/screenshots/minecraft_ss1.jpg'),
    (5, N'/img/screenshots/minecraft_ss2.jpg'),
    (5, N'/img/screenshots/minecraft_ss3.jpg');

-- Thêm dữ liệu mẫu cho bảng CrackInfo
-- Mô tả: Thêm thông tin về bản crack của game
INSERT INTO CrackInfo (GameId, Version, [Group], Description, DownloadUrl, FileSize, ReleaseDate, IsRecommended)
VALUES
    (1, N'1.32', N'CODEX', N'Bản crack hoàn chỉnh bao gồm tất cả DLC và nội dung bổ sung. Có thể chơi ngay sau khi cài đặt.', N'/downloads/witcher3-codex.zip', N'48.5 GB', '2019-06-15', 1),
    (2, N'1.0.2245', N'RELOADED', N'Bản crack đầy đủ, không cần cài đặt Social Club. Chạy tốt trên hầu hết các cấu hình.', N'/downloads/gta5-reloaded.zip', N'70.2 GB', '2021-03-10', 1),
    (3, N'1.6', N'EMPRESS', N'Bản crack mới nhất với hiệu suất được cải thiện, bao gồm tất cả các bản vá.', N'/downloads/cyberpunk-empress.zip', N'65.8 GB', '2022-09-20', 1),
    (4, N'1.0', N'CPY', N'Bản crack đầy đủ, đã khắc phục lỗi kết nối server, chơi offline tốt.', N'/downloads/fifa23-cpy.zip', N'45.6 GB', '2022-10-30', 1),
    (5, N'1.19', N'TLAUNCHER', N'Launcher tùy chỉnh cho phép cài đặt và chơi Minecraft miễn phí với nhiều tính năng bổ sung.', N'/downloads/minecraft-tlauncher.zip', N'250 MB', '2022-08-12', 1);

-- Thêm dữ liệu mẫu cho bảng Features
-- Mô tả: Thêm các tính năng nổi bật của game
INSERT INTO Features (GameId, FeatureDescription)
VALUES
    (1, N'Thế giới mở rộng lớn với hơn 100 giờ chơi'),
    (1, N'Hệ thống chiến đấu đầy thử thách và sâu sắc'),
    (1, N'Đồ họa tuyệt đẹp với chu kỳ ngày đêm và thời tiết thay đổi'),
    (1, N'Cốt truyện hấp dẫn với nhiều lựa chọn ảnh hưởng đến diễn biến của game'),
    
    (2, N'Thành phố Los Santos rộng lớn để khám phá'),
    (2, N'Ba nhân vật chính có thể chuyển đổi qua lại'),
    (2, N'Hơn 100 nhiệm vụ chính và phụ'),
    (2, N'Chế độ multiplayer GTA Online cực kỳ phong phú'),
    
    (3, N'Thế giới cyberpunk chi tiết và đầy sống động'),
    (3, N'Hệ thống tùy biến nhân vật sâu rộng'),
    (3, N'Nhiều cách tiếp cận khác nhau cho từng nhiệm vụ'),
    (3, N'Cốt truyện phức tạp với nhiều kết thúc khác nhau'),
    
    (4, N'Hơn 700 đội bóng được cấp phép chính thức'),
    (4, N'Chế độ Ultimate Team với nhiều cải tiến'),
    (4, N'Công nghệ HyperMotion2 mang đến chuyển động cầu thủ chân thực'),
    (4, N'Chế độ Career Mode được cập nhật toàn diện'),
    
    (5, N'Thế giới vô hạn để khám phá và xây dựng'),
    (5, N'Hai chế độ chơi chính: Sáng tạo và Sinh tồn'),
    (5, N'Hệ thống crafting đa dạng'),
    (5, N'Cập nhật thường xuyên với nội dung mới');

-- Thêm dữ liệu mẫu cho bảng GameTags
-- Mô tả: Liên kết game với các thẻ phù hợp
INSERT INTO GameTags (GameId, TagId)
VALUES
    -- The Witcher 3
    (1, 2), -- Single Player
    (1, 3), -- Open World
    (1, 5), -- Third Person
    (1, 11), -- Offline
    (1, 12), -- Story Rich
    (1, 14), -- Action RPG
    
    -- GTA 5
    (2, 1), -- Multiplayer
    (2, 2), -- Single Player
    (2, 3), -- Open World
    (2, 5), -- Third Person
    (2, 8), -- Sandbox
    
    -- Cyberpunk 2077
    (3, 2), -- Single Player
    (3, 3), -- Open World
    (3, 4), -- First Person
    (3, 11), -- Offline
    (3, 12), -- Story Rich
    (3, 14), -- Action RPG
    
    -- FIFA 23
    (4, 1), -- Multiplayer
    (4, 2), -- Single Player
    (4, 13), -- Competitive
    (4, 20), -- Sports
    
    -- Minecraft
    (5, 1), -- Multiplayer
    (5, 2), -- Single Player
    (5, 3), -- Open World
    (5, 4), -- First Person
    (5, 6), -- Co-op
    (5, 8), -- Sandbox
    (5, 11); -- Offline

-- Thêm dữ liệu mẫu cho bảng GameLinks
-- Mô tả: Thêm các liên kết tải game từ các nguồn khác nhau
INSERT INTO GameLinks (GameId, LinkName, Url, FileSize, PartNumber, TotalParts, Password, IsActive)
VALUES
    -- The Witcher 3
    (1, N'Google Drive', N'https://drive.google.com/file/d/1abc123/view', N'25 GB', 1, 2, NULL, 1),
    (1, N'Google Drive', N'https://drive.google.com/file/d/2def456/view', N'25 GB', 2, 2, NULL, 1),
    (1, N'MediaFire', N'https://mediafire.com/file/witcher3_part1', N'25 GB', 1, 2, NULL, 1),
    (1, N'MediaFire', N'https://mediafire.com/file/witcher3_part2', N'25 GB', 2, 2, NULL, 1),
    
    -- GTA 5
    (2, N'Google Drive', N'https://drive.google.com/file/d/3ghi789/view', N'35 GB', 1, 2, NULL, 1),
    (2, N'Google Drive', N'https://drive.google.com/file/d/4jkl012/view', N'37 GB', 2, 2, NULL, 1),
    (2, N'MEGA', N'https://mega.nz/file/gta5_part1', N'35 GB', 1, 2, NULL, 1),
    (2, N'MEGA', N'https://mega.nz/file/gta5_part2', N'37 GB', 2, 2, NULL, 1),
    
    -- Cyberpunk 2077
    (3, N'Google Drive', N'https://drive.google.com/file/d/5mno345/view', N'34 GB', 1, 2, NULL, 1),
    (3, N'Google Drive', N'https://drive.google.com/file/d/6pqr678/view', N'36 GB', 2, 2, NULL, 1),
    (3, N'MEGA', N'https://mega.nz/file/cyberpunk_part1', N'34 GB', 1, 2, N'cp2077', 1),
    (3, N'MEGA', N'https://mega.nz/file/cyberpunk_part2', N'36 GB', 2, 2, N'cp2077', 1),
    
    -- FIFA 23
    (4, N'Google Drive', N'https://drive.google.com/file/d/7stu901/view', N'50 GB', 1, 1, NULL, 1),
    (4, N'MEGA', N'https://mega.nz/file/fifa23_full', N'50 GB', 1, 1, NULL, 1),
    (4, N'MediaFire', N'https://mediafire.com/file/fifa23_full', N'50 GB', 1, 1, NULL, 1),
    
    -- Minecraft
    (5, N'Google Drive', N'https://drive.google.com/file/d/8vwx234/view', N'250 MB', 1, 1, NULL, 1),
    (5, N'MediaFire', N'https://mediafire.com/file/minecraft_launcher', N'250 MB', 1, 1, NULL, 1);

-- Thêm dữ liệu mẫu cho bảng LocalizationInfo
-- Mô tả: Thêm thông tin về bản Việt hóa của game
INSERT INTO LocalizationInfo (GameId, LocalizationType, LocalizedBy, LocalizationVersion, LocalizationDate, Description, InstallationGuide, DownloadUrl, FileSize, IsComplete)
VALUES
    (1, N'Đầy đủ', N'VietHoaGame Team', N'1.2', '2019-08-20', N'Việt hóa đầy đủ các hội thoại, menu và nội dung game. Chất lượng dịch thuật cao.', N'Giải nén file và copy vào thư mục cài đặt game, ghi đè khi được hỏi.', N'/downloads/viet-hoa/witcher3-viet-hoa.zip', N'1.2 GB', 1),
    (2, N'Menu & UI', N'GTA5-VN', N'2.1', '2020-05-15', N'Việt hóa các menu và giao diện người dùng, không bao gồm hội thoại.', N'Cài đặt bằng installer đi kèm, chọn thư mục cài đặt game khi được hỏi.', N'/downloads/viet-hoa/gta5-viet-hoa.exe', N'350 MB', 0),
    (3, N'Một phần', N'CP2077-VN Collective', N'0.9 Beta', '2022-02-28', N'Việt hóa một phần các menu và nhiệm vụ chính, đang trong quá trình hoàn thiện.', N'1. Sao lưu thư mục game\n2. Giải nén và copy vào thư mục game\n3. Chạy script cài đặt kèm theo', N'/downloads/viet-hoa/cp2077-viet-hoa.rar', N'800 MB', 0);

-- Thêm dữ liệu mẫu cho bảng Reviews
-- Mô tả: Thêm đánh giá của người dùng về game
INSERT INTO Reviews (GameId, UserId, Title, Content, Rating, DatePosted, HelpfulCount)
VALUES
    (1, N'2', N'Game nhập vai hay nhất từ trước đến nay', N'The Witcher 3 là một kiệt tác về mặt cốt truyện và gameplay. Thế giới mở rộng lớn với các nhiệm vụ phụ có chiều sâu, không hề nhàm chán như nhiều game khác. Hệ thống chiến đấu đầy thách thức nhưng rất thỏa mãn khi làm chủ. Đồ họa tuyệt đẹp ngay cả khi game đã ra mắt được vài năm.', 9.8, '2021-06-15', 45),
    (1, N'3', N'Tuyệt phẩm không thể bỏ qua', N'Từ cốt truyện, âm nhạc đến gameplay đều hoàn hảo. Các DLC như Blood and Wine còn hay hơn cả game gốc. Một trong những trải nghiệm chơi game tuyệt vời nhất của tôi.', 9.5, '2021-08-20', 32),
    
    (2, N'4', N'Los Santos sống động đến không ngờ', N'GTA 5 mang đến một thế giới sống động với chi tiết đáng kinh ngạc. Cốt truyện hấp dẫn với 3 nhân vật có tính cách khác biệt hoàn toàn. Tôi đặc biệt thích hệ thống nhiệm vụ cướp ngân hàng. GTA Online thì vô tận và luôn có điều mới mẻ.', 9.2, '2020-12-10', 28),
    (2, N'5', N'Đáng giá từng xu', N'Game thế giới mở hay nhất mà tôi từng chơi. Dù đã ra mắt từ lâu nhưng vẫn giữ được sức hút nhờ cập nhật thường xuyên và cộng đồng lớn. Đồ họa vẫn đẹp so với các game hiện tại.', 9.7, '2022-01-05', 36),
    
    (3, N'2', N'Đẹp nhưng chưa hoàn thiện', N'Cyberpunk 2077 có đồ họa tuyệt đẹp và thế giới Night City vô cùng chi tiết, nhưng game vẫn còn nhiều lỗi và chưa đạt được kỳ vọng ban đầu. Cốt truyện và nhân vật thì rất tốt, đặc biệt là Johnny Silverhand.', 7.5, '2021-02-18', 20),
    (3, N'3', N'Quá nhiều tiềm năng bị bỏ phí', N'Game có tiềm năng rất lớn nhưng bị phát hành quá sớm. Thế giới đẹp và nhân vật thú vị, nhưng gameplay còn nhiều điểm cần cải thiện. Các bản cập nhật gần đây đã làm game tốt hơn nhiều.', 7.0, '2021-03-25', 15),
    
    (4, N'5', N'FIFA tốt nhất trong nhiều năm', N'FIFA 23 đã cải thiện nhiều so với các phiên bản trước. Công nghệ HyperMotion2 mang lại cảm giác chân thực hơn hẳn. Ultimate Team vẫn là chế độ hấp dẫn nhất với nhiều tính năng mới.', 8.5, '2022-10-10', 18),
    (4, N'4', N'Tiến bộ nhưng vẫn còn vấn đề', N'Game đã có những cải tiến về gameplay nhưng vẫn còn một số vấn đề về cân bằng và trí tuệ nhân tạo của cầu thủ. Career mode được cải thiện đáng kể so với các phiên bản trước.', 7.8, '2022-11-15', 12),
    
    (5, N'3', N'Sáng tạo vô tận', N'Minecraft là game mà bạn có thể chơi hàng nghìn giờ mà không chán. Khả năng sáng tạo gần như vô hạn và cộng đồng mod khổng lồ luôn mang đến những trải nghiệm mới.', 9.5, '2020-07-20', 40),
    (5, N'2', N'Đơn giản nhưng sâu sắc', N'Đồ họa đơn giản nhưng gameplay sâu sắc đến ngạc nhiên. Có thể chơi theo nhiều cách khác nhau, từ xây dựng, khám phá đến chiến đấu. Hoàn hảo để chơi với bạn bè.', 9.0, '2021-09-05', 25);

-- Thêm dữ liệu mẫu cho bảng RelatedGames
-- Mô tả: Liên kết các game có liên quan với nhau
INSERT INTO RelatedGames (GameId, RelatedGameId, RelationStrength)
VALUES
    (1, 3, 7), -- Witcher 3 liên quan đến Cyberpunk 2077 (cùng nhà phát triển)
    (3, 1, 7), -- Cyberpunk 2077 liên quan đến Witcher 3 (cùng nhà phát triển)
    (1, 2, 4), -- Witcher 3 liên quan đến GTA 5 (cùng thể loại thế giới mở)
    (2, 1, 4), -- GTA 5 liên quan đến Witcher 3 (cùng thể loại thế giới mở)
    (2, 3, 5), -- GTA 5 liên quan đến Cyberpunk 2077 (cùng thể loại thế giới mở)
    (3, 2, 5); -- Cyberpunk 2077 liên quan đến GTA 5 (cùng thể loại thế giới mở)

-- Thêm dữ liệu mẫu cho bảng FavoriteGames
-- Mô tả: Thêm game yêu thích của người dùng
INSERT INTO FavoriteGames (UserId, GameId, DateAdded)
VALUES
    (N'2', 1, '2021-07-10'),
    (N'2', 3, '2021-08-15'),
    (N'3', 1, '2021-06-20'),
    (N'3', 5, '2022-01-05'),
    (N'4', 2, '2021-11-30'),
    (N'4', 4, '2022-10-15'),
    (N'5', 2, '2021-05-12'),
    (N'5', 3, '2022-02-28');

-- Thêm dữ liệu mẫu cho bảng DownloadHistory
-- Mô tả: Thêm lịch sử tải game của người dùng
INSERT INTO DownloadHistory (UserId, GameId, DownloadDate)
VALUES
    (N'2', 1, '2021-07-10'),
    (N'2', 3, '2021-08-15'),
    (N'2', 2, '2022-03-20'),
    (N'3', 1, '2021-06-20'),
    (N'3', 5, '2022-01-05'),
    (N'4', 2, '2021-11-30'),
    (N'4', 4, '2022-10-15'),
    (N'5', 2, '2021-05-12'),
    (N'5', 3, '2022-02-28'),
    (N'5', 1, '2022-09-15'),
    (N'3', 2, '2022-10-22');

-- Thêm dữ liệu mẫu cho bảng SearchHistory
-- Mô tả: Thêm lịch sử tìm kiếm của người dùng
INSERT INTO SearchHistory (UserId, SearchTerm, SearchDate, ResultCount)
VALUES
    (N'2', N'witcher 3', '2022-10-15', 5),
    (N'2', N'game việt hóa', '2022-09-20', 30),
    (N'3', N'game offline hay 2022', '2022-11-05', 15),
    (N'3', N'cyberpunk 2077 crack', '2022-10-10', 3),
    (N'4', N'game đua xe', '2022-09-18', 8),
    (N'5', N'game fps mới nhất', '2022-11-12', 12),
    (NULL, N'game sinh tồn', '2022-11-10', 11),
    (NULL, N'gta 6 release date', '2022-11-11', 0),
    (NULL, N'game việt hóa hay nhất', '2022-10-25', 25);