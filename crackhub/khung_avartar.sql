-- Sử dụng cơ sở dữ liệu WebDownloadGame
USE WebDownloadGame;
GO

-- Bảng AvatarFrames (Khung avatar người dùng)
-- Mô tả: Lưu trữ thông tin về các khung avatar mà người dùng có thể sử dụng
CREATE TABLE AvatarFrames (
    Id INT PRIMARY KEY IDENTITY(1,1),           -- Định danh khung avatar, tự động tăng
    FrameName NVARCHAR(100) NOT NULL,           -- Tên của khung avatar
    FrameUrl NVARCHAR(255) NOT NULL,            -- Đường dẫn đến file khung avatar (img/frameAvartar/filename)
    Description NVARCHAR(MAX),                  -- Mô tả về khung avatar
    RarityLevel INT NOT NULL DEFAULT 1,         -- Độ hiếm của khung (1: Phổ thông, 2: Hiếm, 3: Cực hiếm, 4: Giới hạn, 5: Đặc biệt)
    IsDefault BIT NOT NULL DEFAULT 0,           -- Đánh dấu khung mặc định (0: Không, 1: Có)
    RequiredLevel INT DEFAULT NULL,             -- Cấp độ yêu cầu để mở khóa (NULL nếu không yêu cầu)
    IsPremium BIT NOT NULL DEFAULT 0,           -- Chỉ dành cho người dùng Premium (0: Không, 1: Có)
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), -- Ngày tạo khung avatar
    IsActive BIT NOT NULL DEFAULT 1             -- Trạng thái kích hoạt (0: Vô hiệu, 1: Kích hoạt)
);

-- Bảng UserAvatarFrames (Mối quan hệ người dùng - khung avatar)
-- Mô tả: Lưu trữ thông tin về các khung avatar mà người dùng sở hữu và đang sử dụng
CREATE TABLE UserAvatarFrames (
    UserId NVARCHAR(450) NOT NULL,              -- ID người dùng, liên kết với bảng Users
    FrameId INT NOT NULL,                       -- ID khung avatar, liên kết với bảng AvatarFrames
    IsEquipped BIT NOT NULL DEFAULT 0,          -- Đánh dấu khung đang được sử dụng (0: Không, 1: Có)
    ObtainedDate DATETIME NOT NULL DEFAULT GETDATE(), -- Ngày nhận được khung
    PRIMARY KEY (UserId, FrameId),              -- Khóa chính kết hợp
    FOREIGN KEY (UserId) REFERENCES Users(Id),  -- Khóa ngoại tham chiếu đến bảng Users
    FOREIGN KEY (FrameId) REFERENCES AvatarFrames(Id) -- Khóa ngoại tham chiếu đến bảng AvatarFrames
);

-- Chèn dữ liệu mẫu cho khung avatar
-- Mô tả: Thêm các khung avatar mặc định và đặc biệt
INSERT INTO AvatarFrames (FrameName, FrameUrl, Description, RarityLevel, IsDefault, RequiredLevel, IsPremium)
VALUES 
    (N'Khung Cơ Bản', N'img/frameAvartar/basic_frame.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 1, NULL, 0)

-- Chèn dữ liệu mẫu cho bảng UserAvatarFrames
-- Mô tả: Gán một số khung avatar cho người dùng hiện có
INSERT INTO UserAvatarFrames (UserId, FrameId, IsEquipped)
VALUES
    (N'System.Security.Cryptography.RandomNumberGeneratorImplementation', 1, 1)  