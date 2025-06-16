// Search Suggestions JavaScript
class SearchSuggestions {
    constructor(inputSelector, suggestionsSelector) {
        // Handle both selector strings and DOM elements
        this.input = typeof inputSelector === 'string' 
            ? document.querySelector(inputSelector) 
            : inputSelector;
            
        this.suggestionsContainer = typeof suggestionsSelector === 'string' 
            ? document.querySelector(suggestionsSelector) 
            : suggestionsSelector;
            
        this.currentIndex = -1;
        this.suggestions = [];
        this.debounceTimeout = null;
        this.isLoading = false;
        
        this.init();
    }
    
    init() {
        if (!this.input) return;
        
        // Create suggestions container if it doesn't exist
        if (!this.suggestionsContainer) {
            this.suggestionsContainer = this.createSuggestionsContainer();
        }
        
        this.bindEvents();
    }
    
    createSuggestionsContainer() {
        const container = document.createElement('div');
        container.className = 'search-suggestions';
        container.id = 'searchSuggestions';
        
        // Insert after the input
        this.input.parentNode.style.position = 'relative';
        this.input.parentNode.appendChild(container);
        
        return container;
    }
    
    bindEvents() {
        // Input events
        this.input.addEventListener('input', (e) => this.handleInput(e));
        this.input.addEventListener('keydown', (e) => this.handleKeydown(e));
        this.input.addEventListener('focus', () => this.handleFocus());
        this.input.addEventListener('blur', (e) => this.handleBlur(e));
        
        // Click outside to close
        document.addEventListener('click', (e) => {
            if (!this.input.contains(e.target) && !this.suggestionsContainer.contains(e.target)) {
                this.hideSuggestions();
            }
        });
    }
    
    handleInput(e) {
        const query = e.target.value.trim();
        
        // Clear previous timeout
        if (this.debounceTimeout) {
            clearTimeout(this.debounceTimeout);
        }
        
        // Debounce the search
        this.debounceTimeout = setTimeout(() => {
            if (query.length >= 2) {
                this.fetchSuggestions(query);
            } else {
                this.hideSuggestions();
            }
        }, 300);
    }
    
    handleKeydown(e) {
        if (!this.suggestionsContainer.classList.contains('show')) return;
        
        switch (e.key) {
            case 'ArrowDown':
                e.preventDefault();
                this.navigateDown();
                break;
            case 'ArrowUp':
                e.preventDefault();
                this.navigateUp();
                break;
            case 'Enter':
                e.preventDefault();
                this.selectCurrent();
                break;
            case 'Escape':
                this.hideSuggestions();
                this.input.blur();
                break;
        }
    }
    
    handleFocus() {
        if (this.suggestions.length > 0 && this.input.value.length >= 2) {
            this.showSuggestions();
        }
    }
    
    handleBlur(e) {
        // Delay hiding to allow clicking on suggestions
        setTimeout(() => {
            if (!this.suggestionsContainer.contains(document.activeElement)) {
                this.hideSuggestions();
            }
        }, 150);
    }
    
    async fetchSuggestions(query) {
        if (this.isLoading) return;
        
        this.isLoading = true;
        this.showLoading();
        
        try {
            const response = await fetch(`/Game/SearchSuggestions?query=${encodeURIComponent(query)}`);
            
            if (!response.ok) {
                throw new Error('Failed to fetch suggestions');
            }
            
            const suggestions = await response.json();
            this.suggestions = suggestions;
            this.currentIndex = -1;
            this.renderSuggestions(suggestions);
            
        } catch (error) {
            console.error('Error fetching suggestions:', error);
            this.showError();
        } finally {
            this.isLoading = false;
        }
    }
    
    showLoading() {
        this.suggestionsContainer.innerHTML = `
            <div class="suggestion-loading">
                <i class="fas fa-spinner"></i>
                Đang tìm kiếm...
            </div>
        `;
        this.showSuggestions();
    }
    
    showError() {
        this.suggestionsContainer.innerHTML = `
            <div class="suggestion-no-results">
                Có lỗi xảy ra khi tìm kiếm
            </div>
        `;
        this.showSuggestions();
    }
    
    renderSuggestions(suggestions) {
        if (suggestions.length === 0) {
            this.suggestionsContainer.innerHTML = `
                <div class="suggestion-no-results">
                    Không tìm thấy kết quả nào
                </div>
            `;
        } else {
            this.suggestionsContainer.innerHTML = suggestions.map((suggestion, index) => `
                <a href="/Game/Details/${suggestion.id}" class="suggestion-item" data-index="${index}">
                    <img src="${suggestion.coverImage || '/img/no-image.jpg'}" 
                         alt="${suggestion.title}" 
                         class="suggestion-image"
                         onerror="this.src='/img/no-image.jpg'">
                    <div class="suggestion-content">
                        <div class="suggestion-title">${this.highlightMatch(suggestion.title)}</div>
                        <div class="suggestion-meta">
                            <span class="suggestion-category">${suggestion.category}</span>
                            ${suggestion.developer ? `<span>by ${suggestion.developer}</span>` : ''}
                        </div>
                    </div>
                </a>
            `).join('');
            
            // Add click events to suggestion items
            this.addSuggestionClickEvents();
        }
        
        this.showSuggestions();
    }
    
    addSuggestionClickEvents() {
        const items = this.suggestionsContainer.querySelectorAll('.suggestion-item');
        items.forEach((item, index) => {
            item.addEventListener('mouseenter', () => {
                this.setActiveIndex(index);
            });
            
            item.addEventListener('click', (e) => {
                e.preventDefault();
                const href = item.getAttribute('href');
                window.location.href = href;
            });
        });
    }
    
    highlightMatch(text) {
        const query = this.input.value.trim();
        if (!query) return text;
        
        const regex = new RegExp(`(${query})`, 'gi');
        return text.replace(regex, '<strong>$1</strong>');
    }
    
    navigateDown() {
        if (this.currentIndex < this.suggestions.length - 1) {
            this.setActiveIndex(this.currentIndex + 1);
        }
    }
    
    navigateUp() {
        if (this.currentIndex > 0) {
            this.setActiveIndex(this.currentIndex - 1);
        } else {
            this.setActiveIndex(-1);
        }
    }
    
    setActiveIndex(index) {
        // Remove active class from all items
        const items = this.suggestionsContainer.querySelectorAll('.suggestion-item');
        items.forEach(item => item.classList.remove('active'));
        
        this.currentIndex = index;
        
        // Add active class to current item
        if (index >= 0 && index < items.length) {
            items[index].classList.add('active');
            items[index].scrollIntoView({ block: 'nearest' });
        }
    }
    
    selectCurrent() {
        if (this.currentIndex >= 0 && this.currentIndex < this.suggestions.length) {
            const suggestion = this.suggestions[this.currentIndex];
            window.location.href = `/Game/Details/${suggestion.id}`;
        } else {
            // If no suggestion is selected, trigger search
            this.triggerSearch();
        }
    }
    
    triggerSearch() {
        const query = this.input.value.trim();
        if (query) {
            window.location.href = `/Game/Search?query=${encodeURIComponent(query)}`;
        }
    }
    
    showSuggestions() {
        this.suggestionsContainer.classList.add('show');
    }
    
    hideSuggestions() {
        this.suggestionsContainer.classList.remove('show');
        this.currentIndex = -1;
    }
}

// Initialize search suggestions when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    // Initialize for header search
    const headerInput = document.querySelector('.navbar .search-input');
    if (headerInput) {
        new SearchSuggestions('.navbar .search-input', '#headerSearchSuggestions');
    }
    
    // Initialize for hero section search  
    const heroInput = document.querySelector('.hero-section .form-control[name="query"]');
    if (heroInput) {
        new SearchSuggestions('.hero-section .form-control[name="query"]', '#heroSearchSuggestions');
    }
    
    // Initialize for game index search
    const gameIndexInput = document.querySelector('.game-index .form-control[name="query"]');
    if (gameIndexInput) {
        new SearchSuggestions('.game-index .form-control[name="query"]', '#gameIndexSearchSuggestions');
    }
    
    // Generic initialization for any search input with data-search-suggestions attribute
    const searchInputs = document.querySelectorAll('input[name="query"]');
    searchInputs.forEach(input => {
        const container = input.closest('.search-container');
        if (container && !input.dataset.initialized) {
            const suggestionsId = 'search-suggestions-' + Math.random().toString(36).substr(2, 9);
            let suggestionsContainer = container.querySelector('.search-suggestions');
            if (!suggestionsContainer) {
                suggestionsContainer = document.createElement('div');
                suggestionsContainer.className = 'search-suggestions';
                suggestionsContainer.id = suggestionsId;
                container.appendChild(suggestionsContainer);
            }
            new SearchSuggestions(input, suggestionsContainer);
            input.dataset.initialized = 'true';
        }
    });
});

// Export for manual initialization
window.SearchSuggestions = SearchSuggestions;
