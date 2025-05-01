/**
 * Game Features JavaScript
 * Xử lý tương tác cho phần tính năng trò chơi
 */
$(document).ready(function() {
    // Thêm hiệu ứng hover và click cho các mục tính năng
    $('.feature-item').each(function(index) {
        // Thêm hiệu ứng hiện dần cho các mục tính năng
        const $item = $(this);
        setTimeout(function() {
            $item.addClass('show');
        }, 100 * index);
        
        // Xử lý sự kiện click
        $item.on('click', function() {
            const $this = $(this);
            
            // Kiểm tra nếu đã có highlight
            if ($this.hasClass('highlight')) {
                $this.removeClass('highlight');
                return;
            }
            
            // Xóa highlight khỏi tất cả các item khác
            $('.feature-item').removeClass('highlight');
            
            // Thêm highlight cho item được click
            $this.addClass('highlight');
            
            // Tạo hiệu ứng nhẹ
            $this.css('transform', 'scale(1.05)');
            setTimeout(function() {
                $this.css('transform', '');
            }, 300);
        });
    });

    // Thêm biểu tượng ngẫu nhiên cho tính năng nếu sử dụng biểu tượng mặc định
    $('.feature-icon .fa-check-circle').each(function() {
        const icons = [
            'fa-gamepad', 'fa-trophy', 'fa-bolt', 'fa-rocket', 
            'fa-crown', 'fa-magic', 'fa-shield-alt', 'fa-award',
            'fa-bullseye', 'fa-fire', 'fa-gem', 'fa-puzzle-piece'
        ];
        
        // Chọn biểu tượng ngẫu nhiên từ mảng
        const randomIcon = icons[Math.floor(Math.random() * icons.length)];
        
        // Thay thế biểu tượng mặc định bằng biểu tượng ngẫu nhiên
        $(this).removeClass('fa-check-circle').addClass(randomIcon);
    });
    
    // Chức năng tìm kiếm tính năng (tùy chọn - có thể bổ sung sau)
    $('#featureSearch').on('input', function() {
        const searchTerm = $(this).val().toLowerCase();
        
        if (searchTerm.length > 1) {
            $('.feature-item').each(function() {
                const text = $(this).find('span').text().toLowerCase();
                if (text.includes(searchTerm)) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            });
        } else {
            // Hiển thị tất cả nếu không có từ khóa tìm kiếm
            $('.feature-item').show();
        }
    });
});
