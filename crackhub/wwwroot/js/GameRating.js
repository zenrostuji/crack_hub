// Enhanced Top-Down Shooter Game with Beautiful UI
// Mouse tracking for enhanced UI effects
let mousePosition = { x: 0, y: 0 };
let isMouseActive = false;

// Initialize mouse tracking for CSS effects
function initMouseTracking() {
    document.addEventListener('mousemove', (e) => {
        const x = (e.clientX / window.innerWidth) * 100;
        const y = (e.clientY / window.innerHeight) * 100;

        mousePosition.x = x;
        mousePosition.y = y;

        // Update CSS custom properties
        document.documentElement.style.setProperty('--mouse-x', x + '%');
        document.documentElement.style.setProperty('--mouse-y', y + '%');

        // Add mouse active class
        if (!isMouseActive) {
            document.body.classList.add('mouse-active');
            isMouseActive = true;
        }
    });

    document.addEventListener('mouseleave', () => {
        document.body.classList.remove('mouse-active');
        isMouseActive = false;
    });
}

// Enhanced menu content hover effects
function enhanceMenuInteractions() {
    const menuContents = document.querySelectorAll('.menu-content');

    menuContents.forEach(menuContent => {
        menuContent.addEventListener('mousemove', (e) => {
            const rect = menuContent.getBoundingClientRect();
            const x = ((e.clientX - rect.left) / rect.width) * 100;
            const y = ((e.clientY - rect.top) / rect.height) * 100;

            menuContent.style.setProperty('--mouse-x', x + '%');
            menuContent.style.setProperty('--mouse-y', y + '%');
        });
    });

    // Enhanced button interactions
    const buttons = document.querySelectorAll('.btn-primary, .btn-secondary');
    buttons.forEach(button => {
        button.addEventListener('mouseenter', () => {
            button.style.transform = 'translateY(-5px) scale(1.02)';
        });

        button.addEventListener('mouseleave', () => {
            button.style.transform = 'translateY(0) scale(1)';
        });
    });
}

// Enhanced trailer interactions
function enhanceTrailerInteractions() {
    const trailerElements = document.querySelectorAll('.trailer-title, .trailer-subtitle, .trailer-text, .skip-button');

    trailerElements.forEach(element => {
        element.addEventListener('mouseenter', () => {
            element.style.filter = 'brightness(1.2) saturate(1.1)';
        });

        element.addEventListener('mouseleave', () => {
            element.style.filter = 'brightness(1) saturate(1)';
        });
    });
}

// Các biến toàn cục
let canvas, ctx, trailerCanvas, trailerCtx;
let gameState = 'trailer'; // trailer, menu, playing, paused, gameOver
let gameStats = {
    score: 0,
    level: 1,
    health: 100,
    maxHealth: 100,
    enemiesKilled: 0,
    startTime: Date.now(),
    survivalTime: 0
};

// Cài đặt game
let gameSettings = {
    volume: 0.5,
    difficulty: 'normal'
};

// Trailer variables
let trailerState = {
    currentScene: 0,
    sceneTimer: 0,
    totalDuration: 20000, // 20 seconds
    sceneDuration: 4000,  // 4 seconds per scene
    startTime: Date.now(),
    fadeTransition: 0,
    isTransitioning: false,
    textRevealProgress: 0,
    cameraShake: { x: 0, y: 0, intensity: 0 },
    scenes: [
        {
            text: "Năm 2087...",
            subtext: "Thế giới đã thay đổi",
            background: { r: 10, g: 15, b: 35 },
            effectType: "stars",
            mood: "mysterious"
        },
        {
            text: "Cuộc xâm lược đã bắt đầu...",
            subtext: "Kẻ thù từ không gian sâu thẳm",
            background: { r: 50, g: 20, b: 20 },
            effectType: "invasion",
            mood: "intense"
        },
        {
            text: "Bạn là hy vọng cuối cùng...",
            subtext: "Chiến binh tinh nhuệ nhất nhân loại",
            background: { r: 20, g: 40, b: 60 },
            effectType: "hero",
            mood: "heroic"
        },
        {
            text: "Hãy chiến đấu vì tương lai!",
            subtext: "Số phận nhân loại trong tay bạn",
            background: { r: 60, g: 40, b: 10 },
            effectType: "battle",
            mood: "epic"
        },
        {
            text: "CHIẾN TRƯỜNG",
            subtext: "Cuộc chiến sinh tồn bắt đầu...",
            background: { r: 30, g: 30, b: 30 },
            effectType: "title",
            mood: "climax"
        }
    ],
    effects: [],
    particles: [],
    cinematicBars: 0,
    glowIntensity: 0
};

// Enhanced player object với improved graphics
let player = {
    x: 0,
    y: 0,
    width: 45,
    height: 45,
    speed: 5,
    angle: 0,
    health: 100,
    maxHealth: 100,
    lastShot: 0,
    shootCooldown: 150,
    // Visual properties
    color: '#4ecdc4',
    glowColor: '#7ff3ff',
    bodyColor: '#2c3e50',
    weaponColor: '#e74c3c',
    shieldActive: false,
    shieldTimer: 0,
    // Animation properties
    walkAnimation: 0,
    shootAnimation: 0,
    hitAnimation: 0
};

// Enhanced enemy types với improved graphics
let enemyTypes = {
    basic: {
        width: 38,
        height: 38,
        speed: 2,
        health: 1,
        score: 10,
        color: '#e74c3c',
        glowColor: '#ff6b6b',
        eyeColor: '#ff0000',
        spikes: 6,
        pattern: 'basic'
    },
    fast: {
        width: 32,
        height: 32,
        speed: 4,
        health: 1,
        score: 20,
        color: '#f39c12',
        glowColor: '#ffd93d',
        eyeColor: '#ff8c00',
        spikes: 8,
        pattern: 'fast'
    },
    tank: {
        width: 55,
        height: 55,
        speed: 1,
        health: 3,
        score: 50,
        color: '#8e44ad',
        glowColor: '#bb8fce',
        eyeColor: '#9b59b6',
        spikes: 4,
        pattern: 'tank'
    },
    assassin: {
        width: 35,
        height: 35,
        speed: 3.5,
        health: 2,
        score: 35,
        color: '#2c3e50',
        glowColor: '#5d6d7e',
        eyeColor: '#e74c3c',
        spikes: 0,
        pattern: 'stealth'
    }
};

// Mảng chứa đạn và kẻ thù
let bullets = [];
let enemies = [];
let particles = [];
let explosions = [];

// Input handling
let keys = {};
let mouse = { x: 0, y: 0, clicked: false };

// Game timing
let lastTime = 0;
let enemySpawnTimer = 0;
let difficultyTimer = 0;

// Visual effects
let screenShake = { x: 0, y: 0, intensity: 0, duration: 0 };

// Khởi tạo game
function init() {
    // Setup main canvas
    canvas = document.getElementById('gameCanvas');
    ctx = canvas.getContext('2d');

    // Setup trailer canvas
    trailerCanvas = document.getElementById('trailerCanvas');
    trailerCtx = trailerCanvas.getContext('2d');

    resizeCanvas();
    setupEventListeners();

    // Start with trailer
    showTrailer();
    gameLoop();
}

function resizeCanvas() {
    // Resize main canvas
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;

    // Resize trailer canvas
    trailerCanvas.width = window.innerWidth;
    trailerCanvas.height = window.innerHeight;

    // Reset player position
    player.x = canvas.width / 2;
    player.y = canvas.height / 2;
}

function setupEventListeners() {
    // Keyboard events
    document.addEventListener('keydown', (e) => {
        keys[e.key.toLowerCase()] = true;

        if (gameState === 'trailer' && e.code === 'Space') {
            skipTrailer();
        }

        if (e.key === 'Escape') {
            if (gameState === 'playing') {
                pauseGame();
            } else if (gameState === 'paused') {
                resumeGame();
            }
        }
    });

    document.addEventListener('keyup', (e) => {
        keys[e.key.toLowerCase()] = false;
    });

    // Mouse events
    canvas.addEventListener('mousemove', (e) => {
        const rect = canvas.getBoundingClientRect();
        mouse.x = e.clientX - rect.left;
        mouse.y = e.clientY - rect.top;
    });

    canvas.addEventListener('mousedown', (e) => {
        if (gameState === 'playing') {
            mouse.clicked = true;
            shoot();
        }
    });

    canvas.addEventListener('mouseup', () => {
        mouse.clicked = false;
    });

    // Window resize
    window.addEventListener('resize', resizeCanvas);

    // Menu button events
    setupMenuEvents();
}

function setupMenuEvents() {
    // Start button
    document.getElementById('startBtn').addEventListener('click', startGame);

    // Instructions
    document.getElementById('instructionsBtn').addEventListener('click', () => {
        showScreen('instructionsMenu');
    });

    document.getElementById('backFromInstructions').addEventListener('click', () => {
        showScreen('mainMenu');
    });

    // Settings
    document.getElementById('settingsBtn').addEventListener('click', () => {
        showScreen('settingsMenu');
    });

    document.getElementById('backFromSettings').addEventListener('click', () => {
        showScreen('mainMenu');
    });

    // Volume slider
    const volumeSlider = document.getElementById('volumeSlider');
    const volumeValue = document.getElementById('volumeValue');

    volumeSlider.addEventListener('input', (e) => {
        gameSettings.volume = e.target.value / 100;
        volumeValue.textContent = e.target.value + '%';
    });

    // Difficulty select
    document.getElementById('difficultySelect').addEventListener('change', (e) => {
        gameSettings.difficulty = e.target.value;
    });

    // Pause menu
    document.getElementById('pauseBtn').addEventListener('click', pauseGame);
    document.getElementById('resumeBtn').addEventListener('click', resumeGame);
    document.getElementById('restartBtn').addEventListener('click', () => {
        startGame();
    });
    document.getElementById('mainMenuBtn').addEventListener('click', () => {
        showScreen('mainMenu');
        gameState = 'menu';
    });

    // Game over menu
    document.getElementById('playAgainBtn').addEventListener('click', startGame);
    document.getElementById('backToMenuBtn').addEventListener('click', () => {
        showScreen('mainMenu');
        gameState = 'menu';
    });
}

// Trailer functions
function showTrailer() {
    gameState = 'trailer';
    showScreen('trailerScreen');
    trailerState.startTime = Date.now();
    trailerState.currentScene = 0;
    trailerState.sceneTimer = 0;
    trailerState.fadeTransition = 0;
    trailerState.isTransitioning = false;
    trailerState.textRevealProgress = 0;
    trailerState.cinematicBars = 0;
    trailerState.glowIntensity = 0;

    // Initialize enhanced trailer effects
    trailerState.effects = [];
    trailerState.particles = [];

    // Create initial particles based on scene
    initializeTrailerEffects();

    // Start with fade in
    setTimeout(() => {
        trailerState.cinematicBars = 50; // Add cinematic black bars
    }, 500);
}

function initializeTrailerEffects() {
    // Create background stars/particles
    for (let i = 0; i < 100; i++) {
        trailerState.particles.push({
            x: Math.random() * trailerCanvas.width,
            y: Math.random() * trailerCanvas.height,
            vx: (Math.random() - 0.5) * 0.5,
            vy: (Math.random() - 0.5) * 0.5,
            size: Math.random() * 2 + 0.5,
            opacity: Math.random() * 0.8 + 0.2,
            type: 'star',
            pulseSpeed: Math.random() * 0.02 + 0.01
        });
    }
}

function updateTrailer(deltaTime) {
    const scene = trailerState.scenes[trailerState.currentScene];
    if (!scene) return;

    trailerState.sceneTimer += deltaTime;

    // Update text reveal animation
    const revealDuration = 1000; // 1 second to reveal text
    if (trailerState.sceneTimer < revealDuration) {
        trailerState.textRevealProgress = trailerState.sceneTimer / revealDuration;
    } else {
        trailerState.textRevealProgress = 1;
    }

    // Update transition effect
    const transitionStart = trailerState.sceneDuration - 800; // Start transition 800ms before scene end
    if (trailerState.sceneTimer >= transitionStart && !trailerState.isTransitioning) {
        trailerState.isTransitioning = true;
        trailerState.fadeTransition = 0;
    }

    if (trailerState.isTransitioning) {
        trailerState.fadeTransition += deltaTime / 800; // 800ms transition
    }

    // Update camera shake for intense scenes
    if (scene.mood === 'intense' || scene.mood === 'epic') {
        trailerState.cameraShake.intensity = Math.sin(Date.now() * 0.01) * 3;
        trailerState.cameraShake.x = (Math.random() - 0.5) * trailerState.cameraShake.intensity;
        trailerState.cameraShake.y = (Math.random() - 0.5) * trailerState.cameraShake.intensity;
    } else {
        trailerState.cameraShake.intensity *= 0.95;
        trailerState.cameraShake.x *= 0.95;
        trailerState.cameraShake.y *= 0.95;
    }

    // Update glow intensity
    trailerState.glowIntensity = 0.5 + Math.sin(Date.now() * 0.003) * 0.3;

    // Update background particles
    updateTrailerParticles(scene);

    // Change scene
    if (trailerState.sceneTimer >= trailerState.sceneDuration) {
        trailerState.currentScene++;
        trailerState.sceneTimer = 0;
        trailerState.isTransitioning = false;
        trailerState.fadeTransition = 0;
        trailerState.textRevealProgress = 0;

        if (trailerState.currentScene >= trailerState.scenes.length) {
            skipTrailer();
            return;
        }

        // Update UI text with enhanced animation
        updateTrailerUI();

        // Add new particles for new scene
        addSceneSpecificParticles(trailerState.scenes[trailerState.currentScene]);
    }

    // Auto skip after total duration
    if (Date.now() - trailerState.startTime >= trailerState.totalDuration) {
        skipTrailer();
    }
}

function updateTrailerParticles(scene) {
    trailerState.particles.forEach(particle => {
        particle.x += particle.vx;
        particle.y += particle.vy;

        // Wrap particles around screen
        if (particle.x < 0) particle.x = trailerCanvas.width;
        if (particle.x > trailerCanvas.width) particle.x = 0;
        if (particle.y < 0) particle.y = trailerCanvas.height;
        if (particle.y > trailerCanvas.height) particle.y = 0;

        // Pulse effect
        particle.opacity = Math.sin(Date.now() * particle.pulseSpeed) * 0.3 + 0.5;
    });
}

function addSceneSpecificParticles(scene) {
    // Add particles based on scene type
    const particleCount = scene.effectType === 'battle' ? 50 : 20;

    for (let i = 0; i < particleCount; i++) {
        let particleType = 'star';
        let particleColor = 'white';

        switch (scene.effectType) {
            case 'invasion':
                particleType = 'ember';
                particleColor = 'red';
                break;
            case 'battle':
                particleType = 'spark';
                particleColor = 'yellow';
                break;
            case 'hero':
                particleType = 'energy';
                particleColor = 'blue';
                break;
        }

        trailerState.particles.push({
            x: Math.random() * trailerCanvas.width,
            y: Math.random() * trailerCanvas.height,
            vx: (Math.random() - 0.5) * 2,
            vy: (Math.random() - 0.5) * 2,
            size: Math.random() * 3 + 1,
            opacity: Math.random() * 0.8 + 0.2,
            type: particleType,
            color: particleColor,
            life: 3000,
            pulseSpeed: Math.random() * 0.02 + 0.01
        });
    }

    // Remove old particles
    trailerState.particles = trailerState.particles.filter(p => !p.life || p.life > 0);
}

function updateTrailerUI() {
    const scene = trailerState.scenes[trailerState.currentScene];
    if (!scene) return;

    const textElement = document.getElementById('trailerText');
    const subtitleElement = document.getElementById('trailerSubtitle');

    // Update main text
    textElement.textContent = scene.text;
    textElement.style.animation = 'none';
    textElement.offsetHeight; // Force reflow
    textElement.style.animation = 'typeWriter 2s ease-out';

    // Update subtitle with scene subtext
    if (scene.subtext) {
        subtitleElement.textContent = scene.subtext;
        subtitleElement.style.animation = 'none';
        setTimeout(() => {
            subtitleElement.style.animation = 'fadeInUp 1.5s ease-out 0.5s both';
        }, 500);
    }
}

function renderTrailer() {
    const scene = trailerState.scenes[trailerState.currentScene];
    if (!scene) return;

    // Clear canvas
    trailerCtx.clearRect(0, 0, trailerCanvas.width, trailerCanvas.height);

    // Apply camera shake
    trailerCtx.save();
    trailerCtx.translate(trailerState.cameraShake.x, trailerState.cameraShake.y);

    // Draw animated background with enhanced effects
    drawTrailerBackground(scene);

    // Draw particles
    drawTrailerParticles();

    // Draw preview characters with enhanced animation
    drawTrailerCharacters(scene);

    // Draw transition effect
    if (trailerState.isTransitioning) {
        trailerCtx.fillStyle = `rgba(0, 0, 0, ${trailerState.fadeTransition})`;
        trailerCtx.fillRect(0, 0, trailerCanvas.width, trailerCanvas.height);
    }

    trailerCtx.restore();

    // Draw cinematic bars
    drawCinematicBars();

    // Draw scene-specific overlays
    drawSceneOverlays(scene);
}

function drawTrailerBackground(scene) {
    const time = Date.now() * 0.001;
    const bg = scene.background;

    // Base gradient
    const gradient = trailerCtx.createRadialGradient(
        trailerCanvas.width / 2, trailerCanvas.height / 2, 0,
        trailerCanvas.width / 2, trailerCanvas.height / 2, trailerCanvas.width * 0.8
    );

    const intensity = trailerState.glowIntensity;
    gradient.addColorStop(0, `rgba(${bg.r + 30 * intensity}, ${bg.g + 30 * intensity}, ${bg.b + 30 * intensity}, 0.9)`);
    gradient.addColorStop(0.5, `rgba(${bg.r + 10 * intensity}, ${bg.g + 10 * intensity}, ${bg.b + 10 * intensity}, 0.7)`);
    gradient.addColorStop(1, `rgba(${bg.r}, ${bg.g}, ${bg.b}, 1)`);

    trailerCtx.fillStyle = gradient;
    trailerCtx.fillRect(0, 0, trailerCanvas.width, trailerCanvas.height);

    // Add animated noise texture for film grain effect
    if (scene.mood === 'intense' || scene.mood === 'epic') {
        trailerCtx.globalAlpha = 0.1;
        for (let i = 0; i < 200; i++) {
            trailerCtx.fillStyle = Math.random() > 0.5 ? 'white' : 'black';
            trailerCtx.fillRect(
                Math.random() * trailerCanvas.width,
                Math.random() * trailerCanvas.height,
                2, 2
            );
        }
        trailerCtx.globalAlpha = 1;
    }

    // Scene-specific background effects
    drawSceneBackground(scene, time);
}

function drawSceneBackground(scene, time) {
    switch (scene.effectType) {
        case 'stars':
            drawStarField(time);
            break;
        case 'invasion':
            drawInvasionEffect(time);
            break;
        case 'hero':
            drawHeroEffect(time);
            break;
        case 'battle':
            drawBattleEffect(time);
            break;
        case 'title':
            drawTitleEffect(time);
            break;
    }
}

function drawStarField(time) {
    trailerCtx.fillStyle = 'rgba(255, 255, 255, 0.8)';
    for (let i = 0; i < 50; i++) {
        const x = (Math.sin(time * 0.5 + i) * 200 + trailerCanvas.width / 2 + i * 50) % trailerCanvas.width;
        const y = (Math.cos(time * 0.3 + i) * 100 + trailerCanvas.height / 2 + i * 30) % trailerCanvas.height;
        const size = Math.sin(time * 2 + i) * 2 + 2;

        trailerCtx.globalAlpha = Math.sin(time * 3 + i) * 0.5 + 0.5;
        trailerCtx.beginPath();
        trailerCtx.arc(x, y, size, 0, Math.PI * 2);
        trailerCtx.fill();
    }
    trailerCtx.globalAlpha = 1;
}

function drawInvasionEffect(time) {
    // Draw red warning lines
    trailerCtx.strokeStyle = 'rgba(255, 0, 0, 0.7)';
    trailerCtx.lineWidth = 3;
    for (let i = 0; i < 5; i++) {
        trailerCtx.globalAlpha = Math.sin(time * 4 + i) * 0.5 + 0.5;
        trailerCtx.beginPath();
        trailerCtx.moveTo(0, i * trailerCanvas.height / 5 + Math.sin(time + i) * 20);
        trailerCtx.lineTo(trailerCanvas.width, i * trailerCanvas.height / 5 + Math.sin(time + i + 1) * 20);
        trailerCtx.stroke();
    }
    trailerCtx.globalAlpha = 1;
}

function drawHeroEffect(time) {
    // Draw blue energy rings
    trailerCtx.strokeStyle = 'rgba(0, 150, 255, 0.6)';
    trailerCtx.lineWidth = 2;
    for (let i = 1; i <= 3; i++) {
        const radius = Math.sin(time * 2) * 50 + i * 100;
        trailerCtx.globalAlpha = (4 - i) * 0.3;
        trailerCtx.beginPath();
        trailerCtx.arc(trailerCanvas.width / 2, trailerCanvas.height / 2, radius, 0, Math.PI * 2);
        trailerCtx.stroke();
    }
    trailerCtx.globalAlpha = 1;
}

function drawBattleEffect(time) {
    // Draw explosive flashes
    trailerCtx.fillStyle = 'rgba(255, 255, 0, 0.3)';
    for (let i = 0; i < 3; i++) {
        const x = Math.sin(time * 3 + i * 2) * 200 + trailerCanvas.width / 2;
        const y = Math.cos(time * 2 + i * 2) * 150 + trailerCanvas.height / 2;
        const size = Math.sin(time * 5 + i) * 30 + 40;

        trailerCtx.globalAlpha = Math.sin(time * 4 + i) * 0.4 + 0.2;
        trailerCtx.beginPath();
        trailerCtx.arc(x, y, size, 0, Math.PI * 2);
        trailerCtx.fill();
    }
    trailerCtx.globalAlpha = 1;
}

function drawTitleEffect(time) {
    // Draw dramatic lighting
    const gradient = trailerCtx.createLinearGradient(0, 0, trailerCanvas.width, trailerCanvas.height);
    gradient.addColorStop(0, 'rgba(255, 255, 255, 0.1)');
    gradient.addColorStop(0.5, 'rgba(255, 255, 255, 0.3)');
    gradient.addColorStop(1, 'rgba(255, 255, 255, 0.1)');

    trailerCtx.fillStyle = gradient;
    trailerCtx.globalAlpha = Math.sin(time * 2) * 0.3 + 0.4;
    trailerCtx.fillRect(0, 0, trailerCanvas.width, trailerCanvas.height);
    trailerCtx.globalAlpha = 1;
}

function drawTrailerParticles() {
    trailerState.particles.forEach(particle => {
        trailerCtx.save();
        trailerCtx.globalAlpha = particle.opacity;

        switch (particle.type) {
            case 'star':
                trailerCtx.fillStyle = 'white';
                trailerCtx.shadowColor = 'white';
                trailerCtx.shadowBlur = 10;
                break;
            case 'ember':
                trailerCtx.fillStyle = '#ff6b6b';
                trailerCtx.shadowColor = '#ff6b6b';
                trailerCtx.shadowBlur = 8;
                break;
            case 'spark':
                trailerCtx.fillStyle = '#ffd93d';
                trailerCtx.shadowColor = '#ffd93d';
                trailerCtx.shadowBlur = 12;
                break;
            case 'energy':
                trailerCtx.fillStyle = '#4ecdc4';
                trailerCtx.shadowColor = '#4ecdc4';
                trailerCtx.shadowBlur = 15;
                break;
        }

        trailerCtx.beginPath();
        trailerCtx.arc(particle.x, particle.y, particle.size, 0, Math.PI * 2);
        trailerCtx.fill();

        // Decrease life for temporary particles
        if (particle.life) {
            particle.life -= 16; // Assuming 60fps
        }

        trailerCtx.restore();
    });

    // Remove dead particles
    trailerState.particles = trailerState.particles.filter(p => !p.life || p.life > 0);
}

function drawCinematicBars() {
    if (trailerState.cinematicBars > 0) {
        trailerCtx.fillStyle = 'black';
        trailerCtx.fillRect(0, 0, trailerCanvas.width, trailerState.cinematicBars);
        trailerCtx.fillRect(0, trailerCanvas.height - trailerState.cinematicBars,
            trailerCanvas.width, trailerState.cinematicBars);
    }
}

function drawSceneOverlays(scene) {
    // Add text overlay effects based on scene mood
    const overlay = document.getElementById('trailerUI');

    switch (scene.mood) {
        case 'intense':
            overlay.style.filter = 'hue-rotate(10deg) saturate(1.5)';
            break;
        case 'heroic':
            overlay.style.filter = 'hue-rotate(200deg) saturate(1.2)';
            break;
        case 'epic':
            overlay.style.filter = 'contrast(1.3) brightness(1.1)';
            break;
        case 'climax':
            overlay.style.filter = 'contrast(1.5) saturate(2)';
            break;
        default:
            overlay.style.filter = 'none';
    }
}

function drawTrailerCharacters() {
    const centerX = trailerCanvas.width / 2;
    const centerY = trailerCanvas.height / 2;
    const time = Date.now() * 0.001;

    // Draw player preview
    trailerCtx.save();
    trailerCtx.translate(centerX - 100, centerY);
    trailerCtx.rotate(Math.sin(time) * 0.1);
    drawEnhancedPlayer(trailerCtx, 0, 0, 60, 0);
    trailerCtx.restore();

    // Draw enemy previews
    const enemyTypes = ['basic', 'fast', 'tank'];
    enemyTypes.forEach((type, index) => {
        trailerCtx.save();
        trailerCtx.translate(centerX + 150, centerY - 80 + index * 80);
        trailerCtx.rotate(time + index);
        drawEnhancedEnemy(trailerCtx, 0, 0, { type: type, width: 50, height: 50 });
        trailerCtx.restore();
    });
}

function skipTrailer() {
    showScreen('mainMenu');
    gameState = 'menu';
}

// Enhanced drawing functions
function drawEnhancedPlayer(context, x, y, size, angle) {
    context.save();
    context.translate(x, y);
    context.rotate(angle);

    const scale = size / 45;

    // Body glow
    context.shadowColor = player.glowColor;
    context.shadowBlur = 15 * scale;

    // Main body (hexagon)
    context.fillStyle = player.color;
    context.beginPath();
    for (let i = 0; i < 6; i++) {
        const angle = (i * Math.PI) / 3;
        const px = Math.cos(angle) * 18 * scale;
        const py = Math.sin(angle) * 18 * scale;
        if (i === 0) context.moveTo(px, py);
        else context.lineTo(px, py);
    }
    context.closePath();
    context.fill();

    // Core body
    context.fillStyle = player.bodyColor;
    context.beginPath();
    context.arc(0, 0, 12 * scale, 0, Math.PI * 2);
    context.fill();

    // Weapon
    context.fillStyle = player.weaponColor;
    context.fillRect(15 * scale, -3 * scale, 20 * scale, 6 * scale);
    context.beginPath();
    context.arc(35 * scale, 0, 4 * scale, 0, Math.PI * 2);
    context.fill();

    // Eye/core
    context.fillStyle = '#ffffff';
    context.beginPath();
    context.arc(0, 0, 6 * scale, 0, Math.PI * 2);
    context.fill();

    context.fillStyle = '#00ffff';
    context.beginPath();
    context.arc(0, 0, 3 * scale, 0, Math.PI * 2);
    context.fill();

    context.restore();
}

function drawEnhancedEnemy(context, x, y, enemy) {
    const type = enemyTypes[enemy.type] || enemyTypes.basic;

    context.save();
    context.translate(x, y);

    const scale = enemy.width / 38;
    const time = Date.now() * 0.005;

    // Glow effect
    context.shadowColor = type.glowColor;
    context.shadowBlur = 10 * scale;

    if (type.pattern === 'basic') {
        // Spiky enemy
        context.fillStyle = type.color;
        context.beginPath();
        for (let i = 0; i < type.spikes; i++) {
            const angle = (i * Math.PI * 2) / type.spikes + time;
            const outerRadius = 18 * scale;
            const innerRadius = 12 * scale;

            const outerX = Math.cos(angle) * outerRadius;
            const outerY = Math.sin(angle) * outerRadius;
            const innerX = Math.cos(angle + Math.PI / type.spikes) * innerRadius;
            const innerY = Math.sin(angle + Math.PI / type.spikes) * innerRadius;

            if (i === 0) context.moveTo(outerX, outerY);
            else context.lineTo(outerX, outerY);
            context.lineTo(innerX, innerY);
        }
        context.closePath();
        context.fill();
    } else if (type.pattern === 'fast') {
        // Lightning enemy
        context.fillStyle = type.color;
        context.beginPath();
        for (let i = 0; i < 8; i++) {
            const angle = (i * Math.PI * 2) / 8 + time * 2;
            const radius = (12 + Math.sin(time * 3 + i) * 3) * scale;
            const px = Math.cos(angle) * radius;
            const py = Math.sin(angle) * radius;
            if (i === 0) context.moveTo(px, py);
            else context.lineTo(px, py);
        }
        context.closePath();
        context.fill();
    } else if (type.pattern === 'tank') {
        // Heavy tank enemy
        context.fillStyle = type.color;
        context.fillRect(-22 * scale, -22 * scale, 44 * scale, 44 * scale);

        // Armor plates
        context.fillStyle = type.glowColor;
        context.fillRect(-18 * scale, -18 * scale, 36 * scale, 36 * scale);

        // Core
        context.fillStyle = type.color;
        context.beginPath();
        context.arc(0, 0, 12 * scale, 0, Math.PI * 2);
        context.fill();
    } else if (type.pattern === 'stealth') {
        // Stealth assassin
        const opacity = 0.3 + Math.sin(time * 4) * 0.3;
        context.globalAlpha = opacity;

        context.fillStyle = type.color;
        context.beginPath();
        context.moveTo(0, -18 * scale);
        context.lineTo(15 * scale, 18 * scale);
        context.lineTo(-15 * scale, 18 * scale);
        context.closePath();
        context.fill();

        context.globalAlpha = 1;
    }

    // Eyes
    context.shadowBlur = 5 * scale;
    context.fillStyle = type.eyeColor;
    context.beginPath();
    context.arc(-6 * scale, -6 * scale, 3 * scale, 0, Math.PI * 2);
    context.fill();
    context.beginPath();
    context.arc(6 * scale, -6 * scale, 3 * scale, 0, Math.PI * 2);
    context.fill();

    context.restore();
}

// Game logic functions
function startGame() {
    gameState = 'playing';
    showScreen('gameScreen');

    // Reset game stats
    gameStats.score = 0;
    gameStats.level = 1;
    gameStats.health = 100;
    gameStats.maxHealth = 100;
    gameStats.enemiesKilled = 0;
    gameStats.startTime = Date.now();

    // Reset player
    player.health = player.maxHealth;
    player.x = canvas.width / 2;
    player.y = canvas.height / 2;

    // Clear arrays
    bullets.length = 0;
    enemies.length = 0;
    particles.length = 0;
    explosions.length = 0;

    // Reset timers
    enemySpawnTimer = 0;
    difficultyTimer = 0;

    updateUI();
}

function pauseGame() {
    if (gameState === 'playing') {
        gameState = 'paused';
        showScreen('pauseMenu');
    }
}

function resumeGame() {
    if (gameState === 'paused') {
        gameState = 'playing';
        showScreen('gameScreen');
    }
}

function gameOver() {
    gameState = 'gameOver';
    showScreen('gameOverMenu');

    // Calculate survival time
    gameStats.survivalTime = Math.floor((Date.now() - gameStats.startTime) / 1000);

    // Update game over display
    document.getElementById('finalScore').textContent = gameStats.score;
    document.getElementById('enemiesKilled').textContent = gameStats.enemiesKilled;
    document.getElementById('survivalTime').textContent = gameStats.survivalTime + 's';
}

function showScreen(screenId) {
    document.querySelectorAll('.screen').forEach(screen => {
        screen.classList.add('hidden');
    });
    document.getElementById(screenId).classList.remove('hidden');
}

// Update and render functions
function update(deltaTime) {
    if (gameState === 'trailer') {
        updateTrailer(deltaTime);
        return;
    }

    if (gameState !== 'playing') return;

    // Update player
    updatePlayer(deltaTime);

    // Update bullets
    updateBullets(deltaTime);

    // Update enemies
    updateEnemies(deltaTime);

    // Update particles
    updateParticles(deltaTime);

    // Update screen shake
    updateScreenShake(deltaTime);

    // Spawn enemies
    updateEnemySpawning(deltaTime);

    // Update difficulty
    updateDifficulty(deltaTime);

    // Update UI
    updateUI();
}

function updatePlayer(deltaTime) {
    // Movement
    let dx = 0, dy = 0;
    if (keys['w'] || keys['arrowup']) dy -= 1;
    if (keys['s'] || keys['arrowdown']) dy += 1;
    if (keys['a'] || keys['arrowleft']) dx -= 1;
    if (keys['d'] || keys['arrowright']) dx += 1;

    // Normalize diagonal movement
    if (dx !== 0 && dy !== 0) {
        dx *= 0.707;
        dy *= 0.707;
    }

    player.x += dx * player.speed;
    player.y += dy * player.speed;

    // Keep player in bounds
    player.x = Math.max(player.width / 2, Math.min(canvas.width - player.width / 2, player.x));
    player.y = Math.max(player.height / 2, Math.min(canvas.height - player.height / 2, player.y));

    // Update angle to face mouse
    player.angle = Math.atan2(mouse.y - player.y, mouse.x - player.x);

    // Update animations
    if (dx !== 0 || dy !== 0) {
        player.walkAnimation += deltaTime * 0.01;
    }

    if (player.shootAnimation > 0) {
        player.shootAnimation -= deltaTime * 0.005;
    }

    if (player.hitAnimation > 0) {
        player.hitAnimation -= deltaTime * 0.008;
    }

    // Update shield
    if (player.shieldTimer > 0) {
        player.shieldTimer -= deltaTime;
        player.shieldActive = true;
    } else {
        player.shieldActive = false;
    }
}

function shoot() {
    const now = Date.now();
    if (now - player.lastShot < player.shootCooldown) return;

    player.lastShot = now;
    player.shootAnimation = 1;

    const bullet = {
        x: player.x + Math.cos(player.angle) * 30,
        y: player.y + Math.sin(player.angle) * 30,
        vx: Math.cos(player.angle) * 10,
        vy: Math.sin(player.angle) * 10,
        size: 8,
        color: '#00ffff',
        glowColor: '#7ff3ff',
        life: 1000
    };

    bullets.push(bullet);

    // Screen shake
    addScreenShake(2, 50);
}

function updateBullets(deltaTime) {
    for (let i = bullets.length - 1; i >= 0; i--) {
        const bullet = bullets[i];

        bullet.x += bullet.vx;
        bullet.y += bullet.vy;
        bullet.life -= deltaTime;

        // Remove if out of bounds or life expired
        if (bullet.x < 0 || bullet.x > canvas.width ||
            bullet.y < 0 || bullet.y > canvas.height ||
            bullet.life <= 0) {
            bullets.splice(i, 1);
            continue;
        }

        // Check collision with enemies
        for (let j = enemies.length - 1; j >= 0; j--) {
            const enemy = enemies[j];
            const distance = Math.sqrt(
                Math.pow(bullet.x - enemy.x, 2) +
                Math.pow(bullet.y - enemy.y, 2)
            );

            if (distance < bullet.size + enemy.width / 2) {
                // Hit enemy
                enemy.health--;
                bullets.splice(i, 1);

                // Create hit particles
                createHitParticles(enemy.x, enemy.y);

                if (enemy.health <= 0) {
                    // Enemy killed
                    gameStats.score += enemyTypes[enemy.type].score;
                    gameStats.enemiesKilled++;

                    // Create explosion
                    createExplosion(enemy.x, enemy.y);

                    enemies.splice(j, 1);
                }
                break;
            }
        }
    }
}

function updateEnemies(deltaTime) {
    for (let i = enemies.length - 1; i >= 0; i--) {
        const enemy = enemies[i];

        // Move towards player
        const dx = player.x - enemy.x;
        const dy = player.y - enemy.y;
        const distance = Math.sqrt(dx * dx + dy * dy);

        if (distance > 0) {
            enemy.x += (dx / distance) * enemy.speed;
            enemy.y += (dy / distance) * enemy.speed;
        }

        // Check collision with player
        const playerDistance = Math.sqrt(
            Math.pow(enemy.x - player.x, 2) +
            Math.pow(enemy.y - player.y, 2)
        );

        if (playerDistance < enemy.width / 2 + player.width / 2) {
            if (!player.shieldActive) {
                // Damage player
                player.health -= 10;
                player.hitAnimation = 1;
                player.shieldTimer = 500; // Brief invincibility

                // Screen shake
                addScreenShake(5, 200);

                if (player.health <= 0) {
                    gameOver();
                    return;
                }
            }

            // Remove enemy
            enemies.splice(i, 1);
        }
    }
}

function updateParticles(deltaTime) {
    for (let i = particles.length - 1; i >= 0; i--) {
        const particle = particles[i];

        particle.x += particle.vx;
        particle.y += particle.vy;
        particle.life -= deltaTime;
        particle.size *= 0.98;

        if (particle.life <= 0 || particle.size <= 0.1) {
            particles.splice(i, 1);
        }
    }
}

function updateEnemySpawning(deltaTime) {
    enemySpawnTimer += deltaTime;

    const spawnRate = Math.max(500, 2000 - gameStats.level * 100);

    if (enemySpawnTimer >= spawnRate) {
        enemySpawnTimer = 0;
        spawnEnemy();
    }
}

function spawnEnemy() {
    const types = Object.keys(enemyTypes);
    let typeIndex = 0;

    // Difficulty-based enemy selection
    if (gameStats.level >= 3) {
        typeIndex = Math.floor(Math.random() * Math.min(2, types.length));
    }
    if (gameStats.level >= 5) {
        typeIndex = Math.floor(Math.random() * Math.min(3, types.length));
    }
    if (gameStats.level >= 8) {
        typeIndex = Math.floor(Math.random() * types.length);
    }

    const type = types[typeIndex];
    const enemyType = enemyTypes[type];

    // Spawn from random edge
    const edge = Math.floor(Math.random() * 4);
    let x, y;

    switch (edge) {
        case 0: // Top
            x = Math.random() * canvas.width;
            y = -enemyType.height;
            break;
        case 1: // Right
            x = canvas.width + enemyType.width;
            y = Math.random() * canvas.height;
            break;
        case 2: // Bottom
            x = Math.random() * canvas.width;
            y = canvas.height + enemyType.height;
            break;
        case 3: // Left
            x = -enemyType.width;
            y = Math.random() * canvas.height;
            break;
    }

    const enemy = {
        x: x,
        y: y,
        width: enemyType.width,
        height: enemyType.height,
        speed: enemyType.speed,
        health: enemyType.health,
        type: type
    };

    enemies.push(enemy);
}

function updateDifficulty(deltaTime) {
    difficultyTimer += deltaTime;

    if (difficultyTimer >= 10000) { // Every 10 seconds
        difficultyTimer = 0;
        gameStats.level++;
    }
}

function updateScreenShake(deltaTime) {
    if (screenShake.duration > 0) {
        screenShake.duration -= deltaTime;
        screenShake.x = (Math.random() - 0.5) * screenShake.intensity;
        screenShake.y = (Math.random() - 0.5) * screenShake.intensity;

        if (screenShake.duration <= 0) {
            screenShake.x = 0;
            screenShake.y = 0;
        }
    }
}

function addScreenShake(intensity, duration) {
    screenShake.intensity = intensity;
    screenShake.duration = duration;
}

function createHitParticles(x, y) {
    for (let i = 0; i < 5; i++) {
        particles.push({
            x: x,
            y: y,
            vx: (Math.random() - 0.5) * 8,
            vy: (Math.random() - 0.5) * 8,
            size: Math.random() * 4 + 2,
            color: '#ffff00',
            life: 300
        });
    }
}

function createExplosion(x, y) {
    for (let i = 0; i < 15; i++) {
        particles.push({
            x: x,
            y: y,
            vx: (Math.random() - 0.5) * 12,
            vy: (Math.random() - 0.5) * 12,
            size: Math.random() * 8 + 4,
            color: Math.random() > 0.5 ? '#ff6b6b' : '#ffd93d',
            life: 500
        });
    }
}

function updateUI() {
    document.getElementById('scoreValue').textContent = gameStats.score;
    document.getElementById('healthValue').textContent = player.health;
    document.getElementById('levelValue').textContent = gameStats.level;

    // Update health bar
    const healthPercentage = (player.health / player.maxHealth) * 100;
    const healthFill = document.getElementById('healthFill');
    healthFill.style.width = healthPercentage + '%';

    if (healthPercentage > 60) {
        healthFill.style.backgroundColor = '#2ecc71';
    } else if (healthPercentage > 30) {
        healthFill.style.backgroundColor = '#f39c12';
    } else {
        healthFill.style.backgroundColor = '#e74c3c';
    }
}

// Render functions
function render() {
    if (gameState === 'trailer') {
        renderTrailer();
        return;
    }

    if (gameState !== 'playing') return;

    // Clear canvas
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Apply screen shake
    ctx.save();
    ctx.translate(screenShake.x, screenShake.y);

    // Draw background
    drawBackground();

    // Draw game objects
    drawBullets();
    drawEnemies();
    drawPlayer();
    drawParticles();

    ctx.restore();
}

function drawBackground() {
    // Grid pattern
    ctx.strokeStyle = 'rgba(255, 255, 255, 0.1)';
    ctx.lineWidth = 1;

    const gridSize = 50;
    for (let x = 0; x < canvas.width; x += gridSize) {
        ctx.beginPath();
        ctx.moveTo(x, 0);
        ctx.lineTo(x, canvas.height);
        ctx.stroke();
    }

    for (let y = 0; y < canvas.height; y += gridSize) {
        ctx.beginPath();
        ctx.moveTo(0, y);
        ctx.lineTo(canvas.width, y);
        ctx.stroke();
    }
}

function drawPlayer() {
    // Shield effect
    if (player.shieldActive) {
        ctx.shadowColor = '#00ffff';
        ctx.shadowBlur = 20;
        ctx.strokeStyle = '#00ffff';
        ctx.lineWidth = 3;
        ctx.beginPath();
        ctx.arc(player.x, player.y, player.width / 2 + 10, 0, Math.PI * 2);
        ctx.stroke();
        ctx.shadowBlur = 0;
    }

    // Hit effect
    if (player.hitAnimation > 0) {
        ctx.globalAlpha = 0.5 + Math.sin(Date.now() * 0.02) * 0.5;
    }

    drawEnhancedPlayer(ctx, player.x, player.y, player.width, player.angle);

    ctx.globalAlpha = 1;
}

function drawBullets() {
    bullets.forEach(bullet => {
        ctx.shadowColor = bullet.glowColor;
        ctx.shadowBlur = 10;
        ctx.fillStyle = bullet.color;
        ctx.beginPath();
        ctx.arc(bullet.x, bullet.y, bullet.size, 0, Math.PI * 2);
        ctx.fill();
        ctx.shadowBlur = 0;
    });
}

function drawEnemies() {
    enemies.forEach(enemy => {
        drawEnhancedEnemy(ctx, enemy.x, enemy.y, enemy);
    });
}

function drawParticles() {
    particles.forEach(particle => {
        const alpha = particle.life / 500;
        ctx.globalAlpha = alpha;
        ctx.fillStyle = particle.color;
        ctx.beginPath();
        ctx.arc(particle.x, particle.y, particle.size, 0, Math.PI * 2);
        ctx.fill();
    });
    ctx.globalAlpha = 1;
}

// Game loop
function gameLoop() {
    const currentTime = Date.now();
    const deltaTime = currentTime - lastTime;
    lastTime = currentTime;

    update(deltaTime);
    render();

    requestAnimationFrame(gameLoop);
}

// Start the game
window.addEventListener('load', init);
