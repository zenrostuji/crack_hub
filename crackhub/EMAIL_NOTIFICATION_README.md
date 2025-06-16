# Hệ thống Email Thông báo Premium Sắp Hết Hạn

## Tính năng
Hệ thống tự động gửi email thông báo cho user khi Premium của họ còn 1 ngày nữa hết hạn.

## Cấu hình

### 1. Cấu hình SMTP trong appsettings.json

```json
{
  "SmtpSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": "587",
    "EnableSsl": "true",
    "FromEmail": "your-email@gmail.com",
    "FromName": "CrackHub",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

### 2. Cấu hình Gmail App Password

Để sử dụng Gmail làm SMTP server:

1. Bật 2-Step Verification cho Gmail account
2. Tạo App Password:
   - Vào Google Account Settings
   - Security > 2-Step Verification > App passwords
   - Chọn "Mail" và thiết bị của bạn
   - Copy app password vào field "Password" trong config

### 3. Cấu hình SMTP khác

Ví dụ cho Outlook/Hotmail:
```json
{
  "SmtpSettings": {
    "SmtpServer": "smtp-mail.outlook.com",
    "SmtpPort": "587",
    "EnableSsl": "true",
    "FromEmail": "crackhub.notification@gmail.com",
    "FromName": "CrackHub",
    "Username": "crackhub.notification@gmail.com", 
    "Password": "Aptx4869"
  }
}
```

## Cách hoạt động

### 1. Background Service
- `PremiumExpiryNotificationService` chạy mỗi 6 giờ
- Tự động kiểm tra user có Premium hết hạn trong 1 ngày tới
- Gửi email thông báo cho những user này

### 2. Gửi email thủ công
- Admin có thể vào trang Admin Dashboard
- Sử dụng nút "Gửi thông báo Premium" để gửi thủ công
- Có thể gửi email test để kiểm tra cấu hình

### 3. Template email
Email sử dụng HTML template đẹp mắt với:
- Logo và header CrackHub
- Thông tin ngày hết hạn
- Danh sách tính năng Premium
- Nút call-to-action để gia hạn
- Footer với thông tin liên hệ

## API Endpoints

### POST /Notification/SendPremiumExpiryNotifications
Gửi email thông báo cho tất cả user có Premium sắp hết hạn

### POST /Notification/SendTestEmail
Gửi email test
- Parameters: `email`, `userName`

## Logs

Hệ thống ghi log chi tiết:
- Thành công gửi email
- Lỗi gửi email
- Số lượng user được thông báo

## Troubleshooting

### 1. Email không được gửi
- Kiểm tra cấu hình SMTP
- Kiểm tra app password (Gmail)
- Kiểm tra firewall/network
- Xem logs trong console

### 2. User không nhận được email
- Kiểm tra spam folder
- Xác nhận email address trong database
- Kiểm tra email template

### 3. Background service không chạy
- Kiểm tra service đã được đăng ký trong Program.cs
- Xem logs để check lỗi
- Restart application

## Customization

### 1. Thay đổi thời gian kiểm tra
Sửa `_checkInterval` trong `PremiumExpiryNotificationService.cs`:
```csharp
private readonly TimeSpan _checkInterval = TimeSpan.FromHours(12); // 12 giờ
```

### 2. Thay đổi số ngày thông báo trước
Sửa logic trong `CheckAndNotifyExpiringPremiumUsers()`:
```csharp
var tomorrow = DateTime.Now.AddDays(3).Date; // 3 ngày trước
```

### 3. Customizing email template
Sửa template trong `EmailService.SendPremiumExpiryNotificationAsync()` method.
