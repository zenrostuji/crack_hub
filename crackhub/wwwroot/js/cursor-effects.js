// Custom cursor và hiệu ứng làn sương
document.addEventListener('DOMContentLoaded', function() {
    // Kiểm tra nếu là mobile hoặc touch device thì không tạo hiệu ứng
    if (window.innerWidth <= 768 || ('ontouchstart' in window)) return;

    // Tạo custom cursor
    const cursor = document.createElement('div');
    cursor.className = 'custom-cursor';
    document.body.appendChild(cursor);

    // Tạo mist effect container
    const mistContainer = document.createElement('div');
    mistContainer.style.position = 'fixed';
    mistContainer.style.top = '0';
    mistContainer.style.left = '0';
    mistContainer.style.width = '100%';
    mistContainer.style.height = '100%';
    mistContainer.style.pointerEvents = 'none';
    mistContainer.style.zIndex = '1';
    document.body.appendChild(mistContainer);

    let mouseX = 0, mouseY = 0;
    let lastMouseX = 0, lastMouseY = 0;
    let isMoving = false;
    let moveTimeout;

    // Theo dõi vị trí chuột
    document.addEventListener('mousemove', function(e) {
        mouseX = e.clientX;
        mouseY = e.clientY;
        
        // Cập nhật vị trí cursor
        cursor.style.left = (mouseX - 10) + 'px';
        cursor.style.top = (mouseY - 10) + 'px';

        // Kiểm tra tốc độ di chuyển
        const speed = Math.sqrt(Math.pow(mouseX - lastMouseX, 2) + Math.pow(mouseY - lastMouseY, 2));
        
        if (speed > 5) {
            cursor.classList.add('moving');
            isMoving = true;
            clearTimeout(moveTimeout);
            moveTimeout = setTimeout(() => {
                cursor.classList.remove('moving');
                isMoving = false;
            }, 100);
        }

        // Tạo trail effect
        if (Math.random() < 0.7) {
            createTrail(mouseX, mouseY);
        }
        
        // Tạo mist effect ngẫu nhiên (ít hơn khi di chuyển nhanh)
        const mistProbability = isMoving ? 0.15 : 0.25;
        if (Math.random() < mistProbability) {
            createMistEffect(mouseX, mouseY);
        }

        // Tạo particle effect khi di chuyển nhanh
        if (speed > 10 && Math.random() < 0.3) {
            createParticle(mouseX, mouseY);
        }

        lastMouseX = mouseX;
        lastMouseY = mouseY;
    });

    // Tạo hiệu ứng trail
    function createTrail(x, y) {
        const trail = document.createElement('div');
        trail.className = 'cursor-trail';
        trail.style.left = (x - 4 + Math.random() * 8 - 4) + 'px';
        trail.style.top = (y - 4 + Math.random() * 8 - 4) + 'px';
        document.body.appendChild(trail);

        // Xóa trail sau khi animation kết thúc
        setTimeout(() => {
            if (trail.parentNode) {
                trail.parentNode.removeChild(trail);
            }
        }, 800);
    }

    // Tạo hiệu ứng sương mù
    function createMistEffect(x, y) {
        const mist = document.createElement('div');
        mist.className = 'mist-effect';
        mist.style.left = (x - 40 + Math.random() * 80 - 40) + 'px';
        mist.style.top = (y - 40 + Math.random() * 80 - 40) + 'px';
        
        // Random size
        const size = 60 + Math.random() * 40;
        mist.style.width = size + 'px';
        mist.style.height = size + 'px';
        
        mistContainer.appendChild(mist);

        // Xóa mist sau animation
        setTimeout(() => {
            if (mist.parentNode) {
                mist.parentNode.removeChild(mist);
            }
        }, 4000);
    }

    // Tạo particle effect
    function createParticle(x, y) {
        const particle = document.createElement('div');
        particle.className = 'cursor-particle';
        particle.style.left = (x - 2 + Math.random() * 20 - 10) + 'px';
        particle.style.top = (y - 2 + Math.random() * 20 - 10) + 'px';
        document.body.appendChild(particle);

        setTimeout(() => {
            if (particle.parentNode) {
                particle.parentNode.removeChild(particle);
            }
        }, 1200);
    }

    // Hiệu ứng khi hover các elements có thể click
    const hoverElements = 'a, button, input, textarea, select, [onclick], .btn, .game-card, .category-item, .nav-link, .navbar-brand';
    
    document.addEventListener('mouseover', function(e) {
        if (e.target.matches(hoverElements) || e.target.closest(hoverElements)) {
            cursor.classList.add('hover');
            
            // Kiểm tra loại element để thêm class đặc biệt
            if (e.target.matches('input, textarea')) {
                cursor.classList.add('text');
            }
        }
    });

    document.addEventListener('mouseout', function(e) {
        if (e.target.matches(hoverElements) || e.target.closest(hoverElements)) {
            cursor.classList.remove('hover', 'text');
        }
    });

    // Hiệu ứng khi click
    document.addEventListener('mousedown', function(e) {
        cursor.classList.add('click');
        createClickRipple(e.clientX, e.clientY);
    });

    document.addEventListener('mouseup', function(e) {
        cursor.classList.remove('click');
    });

    function createClickRipple(x, y) {
        const ripple = document.createElement('div');
        ripple.className = 'click-ripple';
        ripple.style.left = x + 'px';
        ripple.style.top = y + 'px';
        document.body.appendChild(ripple);

        // Tạo nhiều particles khi click
        for (let i = 0; i < 5; i++) {
            setTimeout(() => createParticle(x, y), i * 50);
        }

        setTimeout(() => {
            if (ripple.parentNode) {
                ripple.parentNode.removeChild(ripple);
            }
        }, 600);
    }

    // Ẩn cursor khi rời khỏi window
    document.addEventListener('mouseleave', function() {
        cursor.style.opacity = '0';
    });

    document.addEventListener('mouseenter', function() {
        cursor.style.opacity = '1';
    });

    // Hiệu ứng đặc biệt cho scroll
    let scrollTimeout;
    window.addEventListener('scroll', function() {
        cursor.style.transform = 'scale(0.8)';
        clearTimeout(scrollTimeout);
        scrollTimeout = setTimeout(() => {
            cursor.style.transform = 'scale(1)';
        }, 150);
    });

    // Tạo ambient mist effects
    function createAmbientMist() {
        if (Math.random() < 0.1) {
            const randomX = Math.random() * window.innerWidth;
            const randomY = Math.random() * window.innerHeight;
            createMistEffect(randomX, randomY);
        }
    }

    // Chạy ambient mist mỗi 2 giây
    setInterval(createAmbientMist, 2000);

    // Cleanup khi resize window
    window.addEventListener('resize', function() {
        if (window.innerWidth <= 768) {
            cursor.style.display = 'none';
            mistContainer.style.display = 'none';
        } else {
            cursor.style.display = 'block';
            mistContainer.style.display = 'block';
        }
    });
});
