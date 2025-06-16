// Game Leaderboard JavaScript
document.addEventListener('DOMContentLoaded', function() {
    loadUserRank();
    setupAutoRefresh();
    enhancePlayButton();
});

// Enhance play button with extra effects
function enhancePlayButton() {
    const playBtn = document.querySelector('.btn-play-game');
    if (playBtn) {
        // Add click effect
        playBtn.addEventListener('click', function(e) {
            // Create ripple effect
            const ripple = document.createElement('div');
            ripple.style.cssText = `
                position: absolute;
                border-radius: 50%;
                background: rgba(255, 255, 255, 0.6);
                transform: scale(0);
                animation: ripple 0.6s linear;
                pointer-events: none;
            `;
            
            const rect = this.getBoundingClientRect();
            const size = Math.max(rect.width, rect.height);
            const x = e.clientX - rect.left - size / 2;
            const y = e.clientY - rect.top - size / 2;
            
            ripple.style.width = ripple.style.height = size + 'px';
            ripple.style.left = x + 'px';
            ripple.style.top = y + 'px';
            
            this.appendChild(ripple);
            
            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
        
        // Add hover sound effect (optional)
        playBtn.addEventListener('mouseenter', function() {
            // You can add sound effect here if needed
            this.style.animationDuration = '1s';
        });
        
        playBtn.addEventListener('mouseleave', function() {
            this.style.animationDuration = '3s';
        });
    }
}

// Load user's current rank and score
async function loadUserRank() {
    try {
        const response = await fetch('/Home/GetUserRank?gameName=CHIẾN TRƯỜNG');
        const data = await response.json();
        
        if (data.rank > 0) {
            document.getElementById('yourRankSection').style.display = 'block';
            document.getElementById('noScoreSection').style.display = 'none';
            document.getElementById('yourRank').textContent = '#' + data.rank;
            document.getElementById('yourScore').textContent = data.score.toLocaleString();
            document.getElementById('yourLevel').textContent = data.level;
            document.getElementById('yourSurvival').textContent = data.survivalTime + 's';
        } else {
            // User has no score, show encouragement
            document.getElementById('yourRankSection').style.display = 'none';
            document.getElementById('noScoreSection').style.display = 'block';
        }
    } catch (error) {
        console.error('Error loading user rank:', error);
        // Show encouragement on error (user likely not logged in)
        document.getElementById('yourRankSection').style.display = 'none';
        document.getElementById('noScoreSection').style.display = 'block';
    }
}

// Refresh leaderboard
async function refreshLeaderboard() {
    const refreshBtn = document.querySelector('.refresh-btn i');
    refreshBtn.classList.add('loading');
    
    try {
        // Reload the page to get fresh data
        window.location.reload();
    } catch (error) {
        console.error('Error refreshing leaderboard:', error);
        refreshBtn.classList.remove('loading');
    }
}

// Auto refresh every 30 seconds
function setupAutoRefresh() {
    setInterval(() => {
        loadUserRank();
    }, 30000);
}

// Share score function
function shareScore() {
    const gameUrl = window.location.origin + '/Home/GameRating';
    const leaderboardUrl = window.location.href;
    
    if (navigator.share) {
        navigator.share({
            title: 'Bảng Xếp Hạng - CHIẾN TRƯỜNG',
            text: 'Hãy xem bảng xếp hạng game CHIẾN TRƯỜNG và thử thách bản thân!',
            url: leaderboardUrl
        }).catch(err => console.log('Error sharing:', err));
    } else {
        // Fallback: copy to clipboard
        const textToCopy = `Hãy xem bảng xếp hạng game CHIẾN TRƯỜNG và thử thách bản thân! ${leaderboardUrl}`;
        
        if (navigator.clipboard) {
            navigator.clipboard.writeText(textToCopy).then(() => {
                showNotification('Đã sao chép link chia sẻ!', 'success');
            });
        } else {
            // Fallback for older browsers
            const textArea = document.createElement('textarea');
            textArea.value = textToCopy;
            document.body.appendChild(textArea);
            textArea.select();
            document.execCommand('copy');
            document.body.removeChild(textArea);
            showNotification('Đã sao chép link chia sẻ!', 'success');
        }
    }
}

// Show notification
function showNotification(message, type = 'info') {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.innerHTML = `
        <i class="fas fa-${type === 'success' ? 'check-circle' : 'info-circle'}"></i>
        <span>${message}</span>
    `;
    
    // Add styles
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: ${type === 'success' ? 'linear-gradient(135deg, #4ecdc4, #44a08d)' : 'linear-gradient(135deg, #3498db, #2980b9)'};
        color: white;
        padding: 15px 20px;
        border-radius: 10px;
        box-shadow: 0 10px 25px rgba(0, 0, 0, 0.3);
        backdrop-filter: blur(10px);
        z-index: 1000;
        display: flex;
        align-items: center;
        gap: 10px;
        font-weight: 600;
        transform: translateX(400px);
        transition: transform 0.3s ease;
    `;
    
    document.body.appendChild(notification);
    
    // Animate in
    setTimeout(() => {
        notification.style.transform = 'translateX(0)';
    }, 100);
    
    // Remove after 3 seconds
    setTimeout(() => {
        notification.style.transform = 'translateX(400px)';
        setTimeout(() => {
            document.body.removeChild(notification);
        }, 300);
    }, 3000);
}

// Animation for rank badges
function animateRankBadges() {
    const rankElements = document.querySelectorAll('.rank-1, .rank-2, .rank-3');
    
    rankElements.forEach((element, index) => {
        element.style.animationDelay = `${index * 0.2}s`;
        element.classList.add('rank-animate');
    });
}

// Add CSS for rank animation
const style = document.createElement('style');
style.textContent = `
    @keyframes ripple {
        to {
            transform: scale(2);
            opacity: 0;
        }
    }
    
    .rank-animate {
        animation: rankAppear 0.8s ease-out forwards;
    }
    
    @keyframes rankAppear {
        from {
            opacity: 0;
            transform: translateY(20px) scale(0.8);
        }
        to {
            opacity: 1;
            transform: translateY(0) scale(1);
        }
    }
    
    .table-row:hover .score-value {
        animation: scoreGlow 0.5s ease-in-out;
    }
    
    @keyframes scoreGlow {
        0%, 100% {
            text-shadow: 0 0 5px rgba(255, 217, 61, 0.5);
        }
        50% {
            text-shadow: 0 0 20px rgba(255, 217, 61, 0.8), 0 0 30px rgba(255, 217, 61, 0.6);
        }
    }
`;
document.head.appendChild(style);

// Initialize animations when page loads
window.addEventListener('load', () => {
    setTimeout(animateRankBadges, 500);
});

// Smooth scroll to user's rank if they're in the list
function scrollToUserRank() {
    const userRow = document.querySelector('.current-user');
    if (userRow) {
        setTimeout(() => {
            userRow.scrollIntoView({ 
                behavior: 'smooth', 
                block: 'center' 
            });
            
            // Highlight the user's row briefly
            userRow.style.animation = 'highlightUser 2s ease-in-out';
        }, 1000);
    }
}

// Add CSS for user highlight animation
style.textContent += `
    @keyframes highlightUser {
        0%, 100% {
            transform: scale(1);
            box-shadow: none;
        }
        50% {
            transform: scale(1.02);
            box-shadow: 0 0 30px rgba(78, 205, 196, 0.5);
        }
    }
`;

// Call scroll to user rank
scrollToUserRank();
