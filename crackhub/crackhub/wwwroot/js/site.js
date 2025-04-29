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
                //alert('thanh cong :)))))))))))');
                alert('Có lỗi xảy ra. Vui lòng thử lại sau.');
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
});
