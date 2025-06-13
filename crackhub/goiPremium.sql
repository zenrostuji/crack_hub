-- Tạo dữ liệu cho bảng Premiums (Gói Premium)
-- Id: 1 = 1 tháng, 2 = 6 tháng, 3 = 1 năm

-- Xóa dữ liệu cũ nếu có
DELETE FROM [dbo].[Premiums];

-- Reset identity nếu cần
DBCC CHECKIDENT ('[dbo].[Premiums]', RESEED, 0);

-- Bật IDENTITY_INSERT để cho phép chèn giá trị cụ thể vào cột Id
SET IDENTITY_INSERT [dbo].[Premiums] ON;

-- Thêm các gói Premium
INSERT INTO [dbo].[Premiums] ([Id], [Price], [DurationInMonths]) VALUES
(1, 50000.00, N'1 tháng'),
(2, 250000.00, N'6 tháng'), 
(3, 450000.00, N'1 năm');

-- Tắt IDENTITY_INSERT sau khi chèn xong
SET IDENTITY_INSERT [dbo].[Premiums] OFF;

-- Kiểm tra dữ liệu đã thêm
SELECT * FROM [dbo].[Premiums];

-- Thông tin về giá gói:
-- Gói 1 tháng: 50,000 VNĐ
-- Gói 6 tháng: 250,000 VNĐ (tiết kiệm 50,000 VNĐ so với mua 6 lần gói 1 tháng)
-- Gói 1 năm: 450,000 VNĐ (tiết kiệm 150,000 VNĐ so với mua 12 lần gói 1 tháng)