// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// JS Site Code
$(document).ready(function () {
    // Khởi tạo tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });

    // Khởi tạo popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
    
    // Category dropdown hover functionality
    const $categoryDropdown = $('#categoryDropdown');
    const $dropdownMenu = $('#categoryDropdownMenu');
    
    // Position dropdown centered or to the left to avoid overflow
    function positionCategoryDropdown() {
        if (window.innerWidth >= 992) {
            const btnWidth = $categoryDropdown.outerWidth();
            const menuWidth = $dropdownMenu.outerWidth();
            const viewportWidth = $(window).width();
            const btnOffset = $categoryDropdown.offset().left;
            
            // Check if dropdown would overflow right edge
            if (btnOffset + menuWidth > viewportWidth) {
                const rightAlign = Math.min(btnOffset + btnWidth - menuWidth, 0);
                $dropdownMenu.css('left', rightAlign + 'px');
            } else {
                // Center the dropdown under the button
                const leftOffset = (btnWidth - menuWidth) / 2;
                $dropdownMenu.css('left', leftOffset + 'px');
            }
        } else {
            // For mobile, reset positioning
            $dropdownMenu.css('left', '');
        }
    }
    
    // Show dropdown on hover for desktop
    if (window.innerWidth >= 992) {
        $categoryDropdown.parent().hover(
            function() {
                $dropdownMenu.addClass('show');
                $categoryDropdown.attr('aria-expanded', 'true');
                positionCategoryDropdown();
            },
            function() {
                $dropdownMenu.removeClass('show');
                $categoryDropdown.attr('aria-expanded', 'false');
            }
        );
    }
    
    $categoryDropdown.on('click', function(e) {
        if (window.innerWidth < 992) {
            if ($dropdownMenu.hasClass('show')) {
                $dropdownMenu.removeClass('show');
                $categoryDropdown.attr('aria-expanded', 'false');
            } else {
                $dropdownMenu.addClass('show');
                $categoryDropdown.attr('aria-expanded', 'true');
            }
            e.preventDefault();
            e.stopPropagation();
        }
    });

    // Handle window resize for category dropdown positioning
    $(window).on('resize', function() {
        if ($dropdownMenu.hasClass('show') && window.innerWidth >= 992) {
            positionCategoryDropdown();
        }
    });

    // Enhanced hover effect for navbar items with bounce animation
    $('.navbar-nav .nav-link').hover(
        function() {
            $(this).addClass('animate__animated animate__pulse');
            if (!$(this).hasClass('active')) {
                $(this).css('transform', 'translateY(-2px)');
            }
        },
        function() {
            $(this).removeClass('animate__animated animate__pulse');
            if (!$(this).hasClass('active')) {
                $(this).css('transform', 'translateY(0)');
            }
        }
    );

    // Navbar search input focus effect
    $('.search-input').on('focus', function() {
        $(this).parent('.search-input-group').addClass('search-focus');
    }).on('blur', function() {
        $(this).parent('.search-input-group').removeClass('search-focus');
    });
    
    // Enhanced Screenshots Gallery functionality
    $('.screenshot-item').on('click', function(e) {
        e.preventDefault();
        
        // Get the index of the clicked screenshot
        const index = $(this).data('index') || 0;
        
        // If there's a carousel, set it to the correct index
        if($('#screenshotCarousel').length) {
            const carousel = new bootstrap.Carousel(document.getElementById('screenshotCarousel'));
            carousel.to(index);
            
            // Show the modal
            const modal = new bootstrap.Modal(document.getElementById('screenshotModal'));
            modal.show();
        } else {
            // Create fullscreen view fallback
            const imgSrc = $(this).find('img').attr('src');
            const fullscreen = $('<div class="screenshot-fullscreen"><img src="' + imgSrc + '"><button class="fullscreen-close"><i class="fas fa-times"></i></button></div>');
            $('body').append(fullscreen);
            
            setTimeout(() => {
                fullscreen.addClass('active');
            }, 50);
            
            // Close on button click or background click
            fullscreen.on('click', function(e) {
                if($(e.target).hasClass('screenshot-fullscreen') || $(e.target).hasClass('fullscreen-close') || $(e.target).parent().hasClass('fullscreen-close')) {
                    $(this).removeClass('active');
                    setTimeout(() => {
                        $(this).remove();
                    }, 400);
                }
            });
        }
    });
    
    // Add animation to screenshots when they come into view
    function animateScreenshots() {
        const screenshots = $('.screenshot-item');
        
        if(screenshots.length) {
            const observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if(entry.isIntersecting) {
                        $(entry.target).css('animation-play-state', 'running');
                        observer.unobserve(entry.target);
                    }
                });
            }, {
                threshold: 0.2
            });
            
            screenshots.each(function() {
                $(this).css('animation-play-state', 'paused');
                observer.observe(this);
            });
        }
    }
    
    // Initialize screenshot animations
    if('IntersectionObserver' in window) {
        animateScreenshots();
    } else {
        // Fallback for browsers that don't support IntersectionObserver
        $('.screenshot-item').css('animation-play-state', 'running');
    }

    // Enhanced Game card hover effect - updated for both index pages
    $('.game-card').hover(
        function() {
            $(this).find('.card-overlay').addClass('active');
            $(this).find('.card-img-top').css('transform', 'scale(1.05)');
            $(this).css('transform', 'translateY(-10px)');
            $(this).css('box-shadow', '0 15px 30px rgba(121, 80, 242, 0.2)');
        },
        function() {
            $(this).find('.card-overlay').removeClass('active');
            $(this).find('.card-img-top').css('transform', 'scale(1.0)');
            $(this).css('transform', 'translateY(0)');
            $(this).css('box-shadow', '0 5px 15px rgba(0,0,0,0.1)');
        }
    );

    // Enhanced review rating selector interaction
    $('.star-rating-selection input').on('change', function() {
        const rating = parseInt($(this).val());
        let ratingText = '';
        
        switch(rating) {
            case 1:
                ratingText = 'Kém';
                break;
            case 2:
                ratingText = 'Trung bình';
                break;
            case 3:
                ratingText = 'Khá';
                break;
            case 4:
                ratingText = 'Tốt';
                break;
            case 5:
                ratingText = 'Xuất sắc';
                break;
            default:
                ratingText = 'Xuất sắc';
        }
        
        $('#rating-text').text(ratingText);
        
        // Add animation to the rating text
        $('#rating-text').removeClass('rating-text-animation');
        setTimeout(() => {
            $('#rating-text').addClass('rating-text-animation');
        }, 10);
    });
    
    // Handle review submission with enhanced UI feedback
    $('#reviewForm').on('submit', function(e) {
        e.preventDefault();
        
        const $form = $(this);
        const $submitBtn = $('#submitReview');
        const originalBtnText = $submitBtn.html();
        
        // Validate form
        if (!$form[0].checkValidity()) {
            $form.addClass('was-validated');
            return;
        }
        
        // Disable button and show loading state
        $submitBtn.html('<i class="fas fa-spinner fa-spin me-2"></i> Đang gửi...').prop('disabled', true);
        
        // Collect form data
        const gameId = $('#gameId').val();
        const rating = $('input[name="rating"]:checked').val();
        const title = $('#reviewTitle').val();
        const content = $('#reviewContent').val();
        
        // Send AJAX request
        $.ajax({
            url: '/Game/SubmitReview',
            type: 'POST',
            data: {
                gameId: gameId,
                rating: rating,
                title: title, 
                content: content
            },
            success: function(response) {
                if (response.success) {
                    // Show success message
                    $form.removeClass('was-validated').trigger('reset');
                    
                    // Add the new review to the list with highlight animation
                    if (response.reviewHtml) {
                        $('#reviewsList .empty-state').remove();
                        $(response.reviewHtml).addClass('new-review').prependTo('#reviewsList');
                    }
                    
                    // Reset button state
                    $submitBtn.html(originalBtnText).prop('disabled', false);
                    
                    // Show success toast or message
                    showToast('Thành công', 'Đánh giá của bạn đã được gửi thành công', 'success');
                } else {
                    // Reset button state
                    $submitBtn.html(originalBtnText).prop('disabled', false);
                    
                    // Show error message
                    showToast('Lỗi', response.message || 'Có lỗi xảy ra khi gửi đánh giá', 'error');
                }
            },
            error: function() {
                // Reset button state
                $submitBtn.html(originalBtnText).prop('disabled', false);
                
                // Show error message
                showToast('Lỗi', 'Có lỗi xảy ra khi gửi đánh giá. Vui lòng thử lại sau.', 'error');
            }
        });
    });
    
    // Handle helpful button clicks with visual feedback
    $(document).on('click', '.helpful-btn', function() {
        const $button = $(this);
        const reviewId = $button.data('review-id');
        
        // Prevent multiple clicks
        if ($button.prop('disabled')) {
            return;
        }
        
        // Disable button temporarily
        $button.prop('disabled', true);
        
        $.ajax({
            url: '/Game/MarkReviewHelpful',
            type: 'POST',
            data: { reviewId: reviewId },
            success: function(response) {
                if (response.success) {
                    const $count = $button.find('.helpful-count');
                    $count.text('(' + response.helpfulCount + ')');
                    
                    if (response.isHelpful) {
                        $button.addClass('active');
                    } else {
                        $button.removeClass('active');
                    }
                    
                    // Add a small animation
                    $button.removeClass('animate__animated animate__pulse');
                    setTimeout(() => {
                        $button.addClass('animate__animated animate__pulse');
                    }, 10);
                } else {
                    if (response.redirectUrl) {
                        window.location.href = response.redirectUrl;
                    } else {
                        showToast('Thông báo', response.message, 'info');
                    }
                }
                
                // Re-enable button
                setTimeout(() => {
                    $button.prop('disabled', false);
                }, 500);
            },
            error: function() {
                showToast('Lỗi', 'Chức năng đang phát triển :v ', 'error');
                
                // Re-enable button
                $button.prop('disabled', false);
            }
        });
    });
    
    // Toast notification function
    function showToast(title, message, type) {
        if (window.Toastify) {
            Toastify({
                text: message,
                duration: 3000,
                close: true,
                gravity: "top",
                position: "right",
                backgroundColor: type === 'success' ? '#28a745' : 
                                type === 'error' ? '#dc3545' : 
                                type === 'info' ? '#17a2b8' : '#ffc107',
                className: "toast-notification"
            }).showToast();
        } else {
            // Fallback for when Toastify is not available
            alert(message);
        }
    }

    // Favorite button functionality - Improved
    $(document).on('click', '.favorite-btn', function(e) {
        e.preventDefault();
        var gameId = $(this).data('game-id');
        var $button = $(this);
        
        $.ajax({
            url: '/Game/ToggleFavorite',
            type: 'POST',
            data: { gameId: gameId },
            success: function(response) {
                if (response.success) {
                    // Update button appearance
                    if (response.isFavorite) {
                        $button.addClass('active');
                        $button.find('i').removeClass('far').addClass('fas');
                        if ($button.data('type') === 'text') {
                            $button.html('<i class="fas fa-heart me-2"></i> Bỏ yêu thích');
                        }
                    } else {
                        $button.removeClass('active');
                        $button.find('i').removeClass('fas').addClass('far');
                        if ($button.data('type') === 'text') {
                            $button.html('<i class="far fa-heart me-2"></i> Yêu thích');
                        }
                    }
                } else {
                    if (response.redirectUrl) {
                        window.location.href = response.redirectUrl;
                    }
                }
            },
            error: function() {
                alert('thêm thành công ^^');
            }
        });
    });
    
    // Add animated entrance to navbar elements on page load
    $('.navbar-nav .nav-item').each(function(index) {
        $(this).css('animation-delay', (index * 0.1) + 's')
            .addClass('animate__animated animate__fadeInDown');
    });
    
    // Initialize back to top button
    const backToTopBtn = $('.back-to-top');
    
    $(window).scroll(function() {
        if ($(this).scrollTop() > 300) {
            backToTopBtn.addClass('active');
        } else {
            backToTopBtn.removeClass('active');
        }
    });
    
    backToTopBtn.click(function() {
        $('html, body').animate({scrollTop: 0}, 600);
        return false;
    });
    
    // New screenshot fullscreen functionality
    $('#openFullscreenBtn').on('click', function() {
        // Get all screenshots
        const screenshots = [];
        $('#screenshotsCarousel .carousel-item img').each(function() {
            screenshots.push($(this).attr('src'));
        });
        
        // Get current active index
        const currentIndex = $('#screenshotsCarousel .carousel-item.active').index();
        
        // Create fullscreen modal with better pixel art handling
        const fullscreenModal = $(`
            <div class="screenshots-fullscreen-modal">
                <div class="fullscreen-content">
                    <img src="${screenshots[currentIndex]}" class="fullscreen-image" alt="Screenshot" style="image-rendering: pixelated;">
                    <button class="fullscreen-close"><i class="fas fa-times"></i></button>
                    <div class="fullscreen-nav">
                        <button class="fullscreen-prev"><i class="fas fa-chevron-left"></i></button>
                        <button class="fullscreen-next"><i class="fas fa-chevron-right"></i></button>
                    </div>
                    <div class="fullscreen-counter">
                        ${currentIndex + 1} / ${screenshots.length}
                    </div>
                </div>
            </div>
        `);
        
        // Add to body
        $('body').append(fullscreenModal);
        $('body').addClass('overflow-hidden');
        
        // Show with animation
        setTimeout(() => {
            fullscreenModal.css('display', 'block');
        }, 50);
        
        // Track current index
        let activeIndex = currentIndex;
        
        // Close button functionality
        fullscreenModal.find('.fullscreen-close').on('click', function() {
            fullscreenModal.fadeOut(300, function() {
                $(this).remove();
                $('body').removeClass('overflow-hidden');
            });
        });
        
        // Previous button functionality
        fullscreenModal.find('.fullscreen-prev').on('click', function() {
            activeIndex = (activeIndex - 1 + screenshots.length) % screenshots.length;
            fullscreenModal.find('.fullscreen-image')
                .attr('src', screenshots[activeIndex])
                .css('opacity', 0)
                .animate({opacity: 1}, 300);
            fullscreenModal.find('.fullscreen-counter').text(`${activeIndex + 1} / ${screenshots.length}`);
        });
        
        // Next button functionality
        fullscreenModal.find('.fullscreen-next').on('click', function() {
            activeIndex = (activeIndex + 1) % screenshots.length;
            fullscreenModal.find('.fullscreen-image')
                .attr('src', screenshots[activeIndex])
                .css('opacity', 0)
                .animate({opacity: 1}, 300);
            fullscreenModal.find('.fullscreen-counter').text(`${activeIndex + 1} / ${screenshots.length}`);
        });
        
        // Click outside to close
        fullscreenModal.on('click', function(e) {
            if ($(e.target).hasClass('screenshots-fullscreen-modal') || 
                $(e.target).hasClass('fullscreen-content')) {
                fullscreenModal.fadeOut(300, function() {
                    $(this).remove();
                    $('body').removeClass('overflow-hidden');
                });
            }
        });
    });
    
    // New thumbnail functionality
    $('.screenshots-thumbnails .thumbnail').on('click', function() {
        const slideIndex = $(this).data('bs-slide-to');
        $('.screenshots-thumbnails .thumbnail').removeClass('active');
        $(this).addClass('active');
        
        // Activate the corresponding carousel slide
        const carousel = new bootstrap.Carousel(document.getElementById('screenshotsCarousel'));
        carousel.to(slideIndex);
    });
    
    // Carousel event listener to update active thumbnail
    $('#screenshotsCarousel').on('slid.bs.carousel', function() {
        const activeIndex = $('#screenshotsCarousel .carousel-item.active').index();
        $('.screenshots-thumbnails .thumbnail').removeClass('active');
        $(`.screenshots-thumbnails .thumbnail[data-bs-slide-to="${activeIndex}"]`).addClass('active');
    });
});
