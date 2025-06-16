# Tính năng giới hạn download cho tài khoản thường

## Mô tả
Đã triển khai tính năng giới hạn download 3 lần/ngày cho tài khoản thường, trong khi tài khoản Premium được tải không giới hạn.

## Các thay đổi đã thực hiện

### 1. Cơ sở dữ liệu và Repository
- **IDownloadHistoryRepository.cs**: Thêm method `GetUserDownloadsCountTodayAsync()` để đếm số lần download trong ngày
- **EFDownloadHistoryRepository.cs**: Implement method đếm download từ 00:00 đến 23:59 của ngày hiện tại

### 2. GameController - Logic kiểm tra giới hạn
- **Download action**: 
  - Kiểm tra trạng thái Premium của user
  - Nếu không có Premium, kiểm tra số lần download trong ngày
  - Redirect đến trang DownloadLimit nếu vượt quá 3 lần
- **Details action**: Hiển thị thông tin về số lần download còn lại
- **AddDownloadHistory action**: Kiểm tra giới hạn trước khi ghi lịch sử download
- **DownloadLimit action**: Hiển thị trang thông báo giới hạn

### 3. Giao diện người dùng

#### A. Trang chi tiết game (Details.cshtml)
- Hiển thị thông báo cảnh báo khi vượt giới hạn
- Hiển thị số lần download còn lại cho user thường
- Disable nút download khi hết lượt
- Thêm thông tin trạng thái download trong bảng thông tin game

#### B. Trang giới hạn download (DownloadLimit.cshtml - NEW)
- Trang chuyên dụng thông báo khi user vượt giới hạn
- Giải thích về đặc quyền Premium
- Nút nâng cấp Premium và quay về trang chủ

#### C. Trang Profile (Profile.cshtml)
- Hiển thị số lần download đã sử dụng/giới hạn
- Hiển thị thời gian reset (00:00 hôm sau)
- Badge "Không giới hạn" cho user Premium

#### D. Trang chủ (Index.cshtml)
- Banner cảnh báo khi gần đạt giới hạn
- Thông báo chào mừng user mới với thông tin về giới hạn
- Banner thông tin cho user thường

#### E. Trang Premium (Premium.cshtml)
- Highlight tính năng "Không giới hạn lượt download"
- Thêm FAQ về giới hạn download
- Nhấn mạnh lợi ích không giới hạn

### 4. Controller Updates
- **AccountController**: 
  - Thêm logic hiển thị download limit trong Profile
  - Thêm welcome message cho user mới đăng ký
- **HomeController**: 
  - Thêm DownloadHistoryRepository dependency
  - Logic hiển thị banner cảnh báo trên trang chủ

## Hằng số và quy tắc
- **DAILY_DOWNLOAD_LIMIT = 3**: Giới hạn tối đa cho tài khoản thường
- **Reset time**: 00:00 mỗi ngày (based on server time)
- **Premium users**: Không bị giới hạn download
- **Anonymous users**: Không bị theo dõi và giới hạn

## Cách hoạt động
1. User đăng nhập và cố gắng download game
2. System kiểm tra trạng thái Premium
3. Nếu không phải Premium, đếm số lần download hôm nay
4. Nếu < 3: Cho phép download và ghi lịch sử
5. Nếu >= 3: Redirect đến trang DownloadLimit
6. Hiển thị thông tin về số lần còn lại trên các trang

## Lợi ích
- Khuyến khích nâng cấp Premium
- Quản lý tải trọng server
- Tạo giá trị cho dịch vụ Premium
- Trải nghiệm user thân thiện với thông báo rõ ràng
