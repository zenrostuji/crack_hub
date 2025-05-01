/**
 * Screenshots Gallery JavaScript
 * Xử lý tương tác cho thư viện hình ảnh
 */
$(document).ready(function() {
    // Khởi tạo biến để theo dõi vị trí hiện tại
    var currentIndex = 0;
    var totalItems = $('.highlight_player_item').length;
    // Thời gian chuyển đổi tự động (ms)
    var autoplayInterval = 5000;
    var autoplayTimer;
    var isAutoplay = true;
    var isAnimating = false; // Ngăn chặn việc nhấn nút liên tục trong khi đang chuyển ảnh
    
    // Hàm hiển thị hình ảnh theo index
    function showItem(index, direction) {
        // Nếu đang có animation, không làm gì cả
        if (isAnimating) return;
        
        isAnimating = true;
        
        // Lưu lại index cũ
        var oldIndex = currentIndex;
        
        // Xác định hướng chuyển động nếu không được cung cấp
        if (!direction) {
            direction = index > oldIndex ? 'next' : 'prev';
            // Xử lý trường hợp đặc biệt khi chuyển từ cuối về đầu hoặc ngược lại
            if (oldIndex === totalItems - 1 && index === 0) direction = 'next';
            if (oldIndex === 0 && index === totalItems - 1) direction = 'prev';
        }
        
        // Lấy các phần tử ảnh
        var $currentItem = $('#highlight_' + oldIndex);
        var $nextItem = $('#highlight_' + index);
        
        // Xóa tất cả các class trước đó
        $('.highlight_player_item').removeClass('active prev next');
        
        // Thêm class cho hiệu ứng chuyển động dựa vào hướng
        if (direction === 'next') {
            $currentItem.addClass('prev');
            $nextItem.addClass('next');
            
            // Kích hoạt reflow để CSS áp dụng ngay lập tức
            $nextItem[0].offsetWidth;
            
            // Sau đó thêm class active để bắt đầu hiệu ứng
            $nextItem.removeClass('next').addClass('active');
        } else {
            $currentItem.addClass('next');
            $nextItem.addClass('prev');
            
            // Kích hoạt reflow
            $nextItem[0].offsetWidth;
            
            // Thêm class active
            $nextItem.removeClass('prev').addClass('active');
        }
        
        // Cập nhật trạng thái active cho thumbnail
        $('.highlight_strip_item').removeClass('active');
        $('#highlight_thumb_' + index).addClass('active');
        
        // Thêm hiệu ứng đẹp mắt cho thumbnail đang active
        $('#highlight_thumb_' + index).css('transform', 'scale(1.05)');
        setTimeout(function() {
            $('#highlight_thumb_' + index).css('transform', '');
        }, 300);
        
        // Cuộn để hiển thị thumbnail đang active
        const $thumbnail = $('#highlight_thumb_' + index);
        const $strip = $('#highlight_strip');
        const stripLeft = $strip.scrollLeft();
        const stripWidth = $strip.width();
        const thumbLeft = $thumbnail.position().left;
        const thumbWidth = $thumbnail.width();
        
        if (thumbLeft < 0 || thumbLeft > stripWidth - thumbWidth) {
            $strip.animate({
                scrollLeft: stripLeft + thumbLeft - (stripWidth / 2) + (thumbWidth / 2)
            }, 200);
        }
        
        // Cập nhật currentIndex
        currentIndex = index;
        
        // Đợi cho hiệu ứng chuyển ảnh hoàn thành
        setTimeout(function() {
            isAnimating = false;
        }, 600); // Thời gian phải phù hợp với thời gian transition trong CSS
        
        // Thêm hiệu ứng âm thanh nhẹ khi chuyển ảnh (tùy chọn)
        playSlideSound();
        
        // Cập nhật số hiện tại/tổng số
        updateCounter();
        
        // Nếu đang ở chế độ tự động chuyển, reset timer
        resetAutoplayTimer();
    }
    
    // Thêm đếm số ảnh
    function addCounter() {
        // Tạo phần tử hiển thị đếm số
        const counter = $('<div class="gallery-counter"></div>');
        
        // Thêm vào gallery
        $('.highlight_overflow').append(counter);
        
        // Cập nhật số ban đầu
        updateCounter();
        
        // Thêm CSS cho counter
        $('head').append(`
            <style>
                .gallery-counter {
                    position: absolute;
                    bottom: 10px;
                    left: 10px;
                    background-color: rgba(0, 0, 0, 0.6);
                    color: white;
                    padding: 4px 10px;
                    border-radius: 15px;
                    font-size: 14px;
                    z-index: 10;
                }
            </style>
        `);
    }
    
    // Cập nhật số đếm
    function updateCounter() {
        $('.gallery-counter').text((currentIndex + 1) + ' / ' + totalItems);
    }
    
    // Tùy chọn: Phát âm thanh nhỏ khi chuyển ảnh
    function playSlideSound() {
        // Bỏ comment nếu muốn thêm âm thanh
        /*
        const audio = new Audio('/sounds/slide.mp3');
        audio.volume = 0.2;
        audio.play().catch(e => console.log('Auto-play prevented:', e));
        */
    }
    
    // Hàm để reset timer tự động chuyển
    function resetAutoplayTimer() {
        if (isAutoplay) {
            clearTimeout(autoplayTimer);
            autoplayTimer = setTimeout(function() {
                nextImage();
            }, autoplayInterval);
        }
    }
    
    // Hàm chuyển đến ảnh tiếp theo
    function nextImage() {
        if (isAnimating) return;
        var newIndex = (currentIndex + 1) % totalItems;
        showItem(newIndex, 'next');
    }
    
    // Hàm chuyển đến ảnh trước đó
    function prevImage() {
        if (isAnimating) return;
        var newIndex = (currentIndex - 1 + totalItems) % totalItems;
        showItem(newIndex, 'prev');
    }
    
    // Thêm nút điều khiển tự động chuyển
    function addAutoplayControl() {
        // Tạo nút điều khiển autoplay
        const autoplayBtn = $('<button class="autoplay-control"><i class="fas fa-pause"></i></button>');
        
        // Thêm vào gallery
        $('.highlight_overflow').append(autoplayBtn);
        
        // Xử lý sự kiện click
        autoplayBtn.on('click', function() {
            isAutoplay = !isAutoplay;
            if (isAutoplay) {
                $(this).html('<i class="fas fa-pause"></i>');
                resetAutoplayTimer();
                
                // Thêm hiệu ứng khi bật autoplay
                $(this).addClass('pulse');
                setTimeout(function() {
                    autoplayBtn.removeClass('pulse');
                }, 500);
            } else {
                $(this).html('<i class="fas fa-play"></i>');
                clearTimeout(autoplayTimer);
            }
        });
        
        // Thêm CSS cho nút
        const style = `
            <style>
                .autoplay-control {
                    position: absolute;
                    bottom: 10px;
                    right: 10px;
                    width: 36px;
                    height: 36px;
                    background-color: rgba(0, 0, 0, 0.6);
                    color: white;
                    border: none;
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    cursor: pointer;
                    z-index: 10;
                    transition: all 0.3s ease;
                    box-shadow: 0 2px 5px rgba(0,0,0,0.2);
                }
                
                .autoplay-control:hover {
                    background-color: rgba(93, 78, 199, 0.8);
                    transform: scale(1.1);
                }
                
                .autoplay-control.pulse {
                    animation: pulse-animation 0.5s ease-out;
                }
                
                @keyframes pulse-animation {
                    0% { transform: scale(1); }
                    50% { transform: scale(1.3); }
                    100% { transform: scale(1); }
                }
            </style>
        `;
        $('head').append(style);
    }
    
    // Thêm hiệu ứng cho các nút chuyển đổi
    function enhanceNavigationButtons() {
        // Thêm hiệu ứng ripple khi click vào nút
        $('.slider_left, .slider_right').append('<span class="ripple"></span>');
        
        $('.slider_left, .slider_right').on('click', function(e) {
            const $this = $(this);
            const offset = $this.offset();
            const rippleX = e.pageX - offset.left;
            const rippleY = e.pageY - offset.top;
            
            $this.find('.ripple').css({
                left: rippleX,
                top: rippleY
            }).addClass('active');
            
            setTimeout(function() {
                $this.find('.ripple').removeClass('active');
            }, 500);
        });
        
        // Thêm CSS cho hiệu ứng ripple
        $('head').append(`
            <style>
                .slider_left, .slider_right {
                    overflow: hidden;
                    position: relative;
                }
                
                .ripple {
                    position: absolute;
                    background-color: rgba(255, 255, 255, 0.4);
                    border-radius: 50%;
                    width: 5px;
                    height: 5px;
                    transform: scale(0);
                    transition: transform 0s;
                    opacity: 1;
                }
                
                .ripple.active {
                    transform: scale(20);
                    opacity: 0;
                    transition: transform 0.5s ease-out, opacity 0.5s ease-out;
                }
            </style>
        `);
    }
    
    // Xử lý sự kiện click nút mũi tên trái
    $('.slider_left').on('click', function(e) {
        e.preventDefault();
        e.stopPropagation();
        prevImage();
    });
    
    // Xử lý sự kiện click nút mũi tên phải
    $('.slider_right').on('click', function(e) {
        e.preventDefault();
        e.stopPropagation();
        nextImage();
    });
    
    // Xử lý sự kiện click vào thumbnail
    $(document).on('click', '.highlight_strip_item', function(e) {
        e.preventDefault();
        e.stopPropagation();
        var index = parseInt($(this).data('target'));
        showItem(index);
    });
    
    // Thêm điều hướng bằng bàn phím
    $(document).keydown(function(e) {
        const galleryVisible = $('.highlight_overflow').length > 0 && 
                             $('.highlight_overflow').is(':visible');
        
        if (galleryVisible) {
            if (e.keyCode === 37) { // Mũi tên trái
                prevImage();
            } else if (e.keyCode === 39) { // Mũi tên phải
                nextImage();
            } else if (e.keyCode === 32) { // Phím Space để play/pause
                isAutoplay = !isAutoplay;
                $('.autoplay-control').click();
            }
        }
    });
    
    // Thêm điều khiển bằng cử chỉ vuốt (swipe) cho thiết bị di động
    function addTouchSwipe() {
        let touchStartX = 0;
        let touchEndX = 0;
        
        $('.highlight_overflow').on('touchstart', function(e) {
            touchStartX = e.originalEvent.touches[0].clientX;
        });
        
        $('.highlight_overflow').on('touchend', function(e) {
            touchEndX = e.originalEvent.changedTouches[0].clientX;
            handleSwipe();
        });
        
        function handleSwipe() {
            // Xác định ngưỡng vuốt tối thiểu (30px)
            const minSwipeDistance = 30;
            
            // Tính khoảng cách vuốt
            const swipeDistance = touchEndX - touchStartX;
            
            // Nếu vuốt đủ xa, thực hiện chuyển ảnh
            if (Math.abs(swipeDistance) > minSwipeDistance) {
                if (swipeDistance > 0) {
                    // Vuốt sang phải -> hiển thị ảnh trước
                    prevImage();
                } else {
                    // Vuốt sang trái -> hiển thị ảnh tiếp theo
                    nextImage();
                }
            }
        }
    }
    
    // Dừng autoplay khi người dùng di chuột vào gallery
    $('.highlight_overflow').hover(
        function() {
            // Tạm dừng khi di chuột vào
            if (isAutoplay) {
                clearTimeout(autoplayTimer);
            }
        },
        function() {
            // Tiếp tục khi di chuột ra
            if (isAutoplay) {
                resetAutoplayTimer();
            }
        }
    );
    
    // Khởi tạo gallery
    function initGallery() {
        if (totalItems > 0) {
            // Thêm nút điều khiển autoplay
            addAutoplayControl();
            
            // Thêm bộ đếm ảnh
            addCounter();
            
            // Cải thiện nút điều hướng
            enhanceNavigationButtons();
            
            // Thêm hỗ trợ cử chỉ vuốt cho thiết bị di động
            addTouchSwipe();
            
            // Preload các hình ảnh tiếp theo để chuyển đổi mượt mà
            preloadImages();
            
            // Khởi tạo với hình ảnh đầu tiên
            showItem(0);
            
            // Bắt đầu tự động chuyển
            resetAutoplayTimer();
        }
    }
    
    // Preload các hình ảnh để chuyển đổi mượt mà hơn
    function preloadImages() {
        $('.highlight_player_item img').each(function() {
            const src = $(this).attr('src');
            if (src) {
                const img = new Image();
                img.src = src;
            }
        });
    }
    
    // Khởi tạo gallery
    initGallery();
});