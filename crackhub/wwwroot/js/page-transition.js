/**
 * Page Transition Effects
 * Xử lý hiệu ứng chuyển trang
 */
$(document).ready(function() {
    // Tạo phần tử hiệu ứng chuyển trang
    const transitionOverlay = $('<div class="page-transition-overlay"><div class="transition-spinner"></div><div class="transition-logo"></div><div class="transition-text">Đang tải...</div></div>');
    
    // Thêm vào body
    $('body').append(transitionOverlay);
    
    // Hiệu ứng khi trang đã tải xong
    $(window).on('load', function() {
        setTimeout(function() {
            // Hiển thị hiệu ứng kết thúc tải trang
            finishLoading();
        }, 500);
    });
    
    // Xử lý sự kiện click cho các liên kết trong trang
    $(document).on('click', 'a:not([target="_blank"]):not([href^="#"]):not([href^="javascript"]):not([href^="mailto"]):not(.no-transition)', function(e) {
        // Lấy URL đích
        const targetUrl = $(this).attr('href');
        
        // Kiểm tra nếu là liên kết nội bộ
        if (targetUrl && targetUrl !== '#' && !targetUrl.includes('javascript:') && targetUrl.indexOf(window.location.hostname) > -1 || targetUrl.charAt(0) === '/') {
            // Ngăn hành vi mặc định
            e.preventDefault();
            
            // Hiệu ứng chuyển trang
            startTransition();
            
            // Chuyển hướng sau khi hoàn tất hiệu ứng
            setTimeout(function() {
                window.location.href = targetUrl;
            }, 800);
        }
    });
    
    // Hàm bắt đầu hiệu ứng chuyển trang
    function startTransition() {
        // Thêm class để hiển thị overlay
        $('.page-transition-overlay').addClass('active');
        $('body').addClass('page-transitioning');
        
        // Hiển thị logo và text sau một khoảng thời gian ngắn
        setTimeout(function() {
            $('.transition-logo, .transition-text').addClass('visible');
        }, 300);
    }
    
    // Hàm kết thúc hiệu ứng tải trang
    function finishLoading() {
        // Thêm class để hiển thị overlay
        $('.page-transition-overlay').addClass('active');
        
        // Hiển thị logo và text
        $('.transition-logo, .transition-text').addClass('visible');
        
        // Ẩn overlay sau một khoảng thời gian
        setTimeout(function() {
            $('.transition-logo, .transition-text').removeClass('visible');
            
            setTimeout(function() {
                $('.page-transition-overlay').addClass('fade-out');
                
                // Xóa các class sau khi hoàn tất hiệu ứng
                setTimeout(function() {
                    $('.page-transition-overlay').removeClass('active fade-out');
                    $('body').removeClass('page-transitioning');
                }, 600);
            }, 300);
        }, 600);
    }
    
    // Hiệu ứng khi quay lại trang trước đó
    $(window).on('pageshow', function(event) {
        if (event.originalEvent.persisted) {
            // Xử lý khi trang được tải từ bộ nhớ cache
            finishLoading();
        }
    });
    
    // Tùy chỉnh thời gian tải trang (mô phỏng)
    const loadingMessages = [
        "Đang tải...",
        "Chuẩn bị nội dung...",
        "Sắp xong...",
        "Hoàn tất..."
    ];
    
    let messageIndex = 0;
    
    // Hiển thị thông báo thay đổi
    if ($('.page-transition-overlay').length) {
        const messageInterval = setInterval(function() {
            messageIndex = (messageIndex + 1) % loadingMessages.length;
            $('.transition-text').text(loadingMessages[messageIndex]);
        }, 1000);
        
        // Hủy interval sau khi hoàn tất tải trang
        setTimeout(function() {
            clearInterval(messageInterval);
        }, 3000);
    }
});