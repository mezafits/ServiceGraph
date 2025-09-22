/* ===================================
   🌌 NEON EFFECTS & PARTICLE SYSTEM
   ===================================
   Dynamic background effects for futuristic UI
*/

class NeonEffectsManager {
    constructor() {
        this.particles = [];
        this.circuitLines = [];
        this.isInitialized = false;
        this.rafId = null;
        this.isReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
        
        this.init();
    }

    init() {
        if (this.isReducedMotion) {
            console.log('Reduced motion preference detected - skipping animations');
            return;
        }

        this.createBackgroundElements();
        this.createParticleSystem();
        this.createCircuitLines();
        this.setupScrollAnimations();
        this.setupIntersectionObserver();
        this.startAnimationLoop();
        
        this.isInitialized = true;
        console.log('Neon Effects System initialized');
    }

    createBackgroundElements() {
        // Create dynamic gradient background
        const gradientBg = document.createElement('div');
        gradientBg.className = 'dynamic-gradient-bg';
        document.body.appendChild(gradientBg);

        // Create hex pattern overlay
        const hexPattern = document.createElement('div');
        hexPattern.className = 'hex-pattern-overlay';
        document.body.appendChild(hexPattern);

        // Create circuit overlay container
        const circuitOverlay = document.createElement('div');
        circuitOverlay.className = 'circuit-overlay';
        circuitOverlay.id = 'circuit-overlay';
        document.body.appendChild(circuitOverlay);
    }

    createParticleSystem() {
        const particleContainer = document.createElement('div');
        particleContainer.className = 'particle-background';
        particleContainer.id = 'particle-background';
        document.body.appendChild(particleContainer);

        // Create 50 particles
        for (let i = 0; i < 50; i++) {
            this.createParticle(particleContainer);
        }
    }

    createParticle(container) {
        const particle = document.createElement('div');
        particle.className = 'particle';
        
        // Random positioning
        particle.style.left = Math.random() * 100 + '%';
        particle.style.top = Math.random() * 100 + '%';
        
        // Random animation delay and duration
        particle.style.animationDelay = Math.random() * 20 + 's';
        particle.style.animationDuration = (15 + Math.random() * 20) + 's';
        
        // Random size variation
        const size = 1 + Math.random() * 3;
        particle.style.width = size + 'px';
        particle.style.height = size + 'px';
        
        container.appendChild(particle);
        this.particles.push(particle);
    }

    createCircuitLines() {
        const circuitContainer = document.getElementById('circuit-overlay');
        if (!circuitContainer) return;

        // Create horizontal circuit lines
        for (let i = 0; i < 8; i++) {
            const line = document.createElement('div');
            line.className = 'circuit-line';
            line.style.top = Math.random() * 100 + '%';
            line.style.width = (20 + Math.random() * 60) + '%';
            line.style.left = Math.random() * 100 + '%';
            line.style.animationDelay = Math.random() * 15 + 's';
            line.style.animationDuration = (10 + Math.random() * 10) + 's';
            
            circuitContainer.appendChild(line);
            this.circuitLines.push(line);
        }

        // Create vertical circuit lines
        for (let i = 0; i < 5; i++) {
            const line = document.createElement('div');
            line.className = 'circuit-line';
            line.style.left = Math.random() * 100 + '%';
            line.style.height = (20 + Math.random() * 60) + '%';
            line.style.top = Math.random() * 100 + '%';
            line.style.width = '1px';
            line.style.transform = 'rotate(90deg)';
            line.style.animationDelay = Math.random() * 15 + 's';
            line.style.animationDuration = (12 + Math.random() * 8) + 's';
            
            circuitContainer.appendChild(line);
            this.circuitLines.push(line);
        }
    }

    setupScrollAnimations() {
        // Add scroll event listener for parallax effects
        let ticking = false;
        
        const updateParallax = () => {
            const scrolled = window.pageYOffset;
            const parallaxElements = document.querySelectorAll('.parallax-layer');
            
            parallaxElements.forEach((element, index) => {
                const speed = 0.5 + (index * 0.2);
                const yPos = -(scrolled * speed);
                element.style.transform = `translateY(${yPos}px)`;
            });
            
            ticking = false;
        };

        window.addEventListener('scroll', () => {
            if (!ticking) {
                requestAnimationFrame(updateParallax);
                ticking = true;
            }
        }, { passive: true });
    }

    setupIntersectionObserver() {
        // Set up intersection observer for scroll animations
        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('in-view');
                    
                    // Add random animation delay for staggered effect
                    const delay = Math.random() * 0.3;
                    entry.target.style.animationDelay = delay + 's';
                }
            });
        }, observerOptions);

        // Observe all elements with animation classes
        const animatedElements = document.querySelectorAll('.animate-on-scroll');
        animatedElements.forEach(el => observer.observe(el));
    }

    startAnimationLoop() {
        const animate = () => {
            if (this.isReducedMotion) return;
            
            // Update particle positions occasionally for dynamic movement
            if (Math.random() < 0.001) {
                this.updateRandomParticle();
            }
            
            // Update circuit lines occasionally
            if (Math.random() < 0.0005) {
                this.updateRandomCircuitLine();
            }
            
            this.rafId = requestAnimationFrame(animate);
        };
        
        animate();
    }

    updateRandomParticle() {
        if (this.particles.length === 0) return;
        
        const particle = this.particles[Math.floor(Math.random() * this.particles.length)];
        particle.style.left = Math.random() * 100 + '%';
        particle.style.top = Math.random() * 100 + '%';
    }

    updateRandomCircuitLine() {
        if (this.circuitLines.length === 0) return;
        
        const line = this.circuitLines[Math.floor(Math.random() * this.circuitLines.length)];
        line.style.opacity = '0';
        
        setTimeout(() => {
            line.style.left = Math.random() * 100 + '%';
            line.style.top = Math.random() * 100 + '%';
            line.style.opacity = '0.1';
        }, 1000);
    }

    // Enhanced button interactions
    static setupAdvancedButtons() {
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('btn-neon') || 
                e.target.classList.contains('btn-neon-advanced')) {
                NeonEffectsManager.createRippleEffect(e.target, e);
            }
        });
    }

    static createRippleEffect(button, event) {
        const ripple = document.createElement('span');
        const rect = button.getBoundingClientRect();
        const size = Math.max(rect.width, rect.height);
        const x = event.clientX - rect.left - size / 2;
        const y = event.clientY - rect.top - size / 2;
        
        ripple.style.width = ripple.style.height = size + 'px';
        ripple.style.left = x + 'px';
        ripple.style.top = y + 'px';
        ripple.classList.add('ripple-effect');
        
        button.appendChild(ripple);
        
        setTimeout(() => {
            ripple.remove();
        }, 600);
    }

    // Modal animations
    static enhanceModals() {
        // Fix modal interactivity issues
        document.addEventListener('DOMContentLoaded', () => {
            NeonEffectsManager.fixModalInteractivity();
        });
        
        // Watch for modal show events
        document.addEventListener('show.bs.modal', (event) => {
            NeonEffectsManager.onModalShow(event.target);
        });
        
        // Watch for modal hide events
        document.addEventListener('hide.bs.modal', (event) => {
            NeonEffectsManager.onModalHide(event.target);
        });
        
        // Override default modal behavior for enhanced animations
        const originalShowModal = window.showModal;
        const originalHideModal = window.hideModal;
        
        window.showModal = function(modalId) {
            const modal = document.getElementById(modalId);
            if (modal) {
                NeonEffectsManager.onModalShow(modal);
            }
            if (originalShowModal) originalShowModal.call(this, modalId);
        };
        
        window.hideModal = function(modalId) {
            const modal = document.getElementById(modalId);
            if (modal) {
                modal.classList.add('closing');
                setTimeout(() => {
                    modal.classList.remove('modal-advanced', 'closing');
                    const backdrop = modal.querySelector('.modal-backdrop') || modal.parentElement;
                    if (backdrop) {
                        backdrop.classList.remove('modal-backdrop-advanced');
                    }
                    if (originalHideModal) originalHideModal.call(this, modalId);
                }, 300);
                return;
            }
            if (originalHideModal) originalHideModal.call(this, modalId);
        };
    }

    static onModalShow(modal) {
        if (!modal) return;
        
        // Ensure proper z-index and pointer events
        modal.style.zIndex = '10000';
        modal.style.pointerEvents = 'auto';
        
        // Add advanced modal class
        modal.classList.add('modal-advanced');
        
        // Fix modal dialog
        const modalDialog = modal.querySelector('.modal-dialog');
        if (modalDialog) {
            modalDialog.style.pointerEvents = 'auto';
            modalDialog.style.zIndex = '10001';
        }
        
        // Fix modal content
        const modalContent = modal.querySelector('.modal-content');
        if (modalContent) {
            modalContent.style.pointerEvents = 'auto';
            modalContent.style.zIndex = '10002';
        }
        
        // Fix backdrop
        const backdrop = modal.querySelector('.modal-backdrop') || document.querySelector('.modal-backdrop');
        if (backdrop) {
            backdrop.classList.add('modal-backdrop-advanced');
            backdrop.style.pointerEvents = 'none';
            backdrop.style.zIndex = '9999';
        }
        
        // Fix all interactive elements inside modal
        const interactiveElements = modal.querySelectorAll('button, input, select, textarea, a, [tabindex]');
        interactiveElements.forEach(element => {
            element.style.pointerEvents = 'auto';
        });
    }

    static onModalHide(modal) {
        if (!modal) return;
        
        modal.classList.add('closing');
        
        // Clean up after animation
        setTimeout(() => {
            modal.classList.remove('modal-advanced', 'closing');
            const backdrop = modal.querySelector('.modal-backdrop') || document.querySelector('.modal-backdrop');
            if (backdrop) {
                backdrop.classList.remove('modal-backdrop-advanced');
            }
        }, 300);
    }

    static fixModalInteractivity() {
        // Fix any existing modals
        fixModalInteractivityNow();
        
        // Add enhanced keyboard navigation support
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape') {
                const visibleModal = document.querySelector('.modal.show');
                if (visibleModal) {
                    const closeBtn = visibleModal.querySelector('.close, [data-dismiss="modal"]');
                    if (closeBtn) {
                        closeBtn.click();
                    }
                }
            }
            
            // Tab trap for better accessibility
            if (e.key === 'Tab') {
                const visibleModal = document.querySelector('.modal.show');
                if (visibleModal) {
                    const focusableElements = visibleModal.querySelectorAll(
                        'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
                    );
                    const firstElement = focusableElements[0];
                    const lastElement = focusableElements[focusableElements.length - 1];
                    
                    if (e.shiftKey && document.activeElement === firstElement) {
                        e.preventDefault();
                        lastElement.focus();
                    } else if (!e.shiftKey && document.activeElement === lastElement) {
                        e.preventDefault();
                        firstElement.focus();
                    }
                }
            }
        });
        
        // Watch for dynamically added modals
        const observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                mutation.addedNodes.forEach((node) => {
                    if (node.nodeType === 1) { // Element node
                        if (node.classList && node.classList.contains('modal')) {
                            setTimeout(fixModalInteractivityNow, 50);
                        }
                        // Check for modals within added nodes
                        const modals = node.querySelectorAll && node.querySelectorAll('.modal');
                        if (modals && modals.length > 0) {
                            setTimeout(fixModalInteractivityNow, 50);
                        }
                    }
                });
            });
        });
        
        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    }

    // Loading state animations
    static showLoadingState(element) {
        if (!element) return;
        
        element.classList.add('loading-neon');
        element.setAttribute('data-original-content', element.innerHTML);
        
        const loadingSpinner = document.createElement('div');
        loadingSpinner.className = 'loading-spinner-neon';
        loadingSpinner.innerHTML = `
            <div class="spinner-ring"></div>
            <span>Processing...</span>
        `;
        
        element.appendChild(loadingSpinner);
    }

    static hideLoadingState(element) {
        if (!element) return;
        
        element.classList.remove('loading-neon');
        const spinner = element.querySelector('.loading-spinner-neon');
        if (spinner) {
            spinner.remove();
        }
        
        const originalContent = element.getAttribute('data-original-content');
        if (originalContent) {
            element.innerHTML = originalContent;
            element.removeAttribute('data-original-content');
        }
    }

    // Page transitions
    static transitionToPage(callback) {
        document.body.classList.add('page-transition-exit');
        
        setTimeout(() => {
            if (callback) callback();
            document.body.classList.remove('page-transition-exit');
            document.body.classList.add('page-transition-enter');
            
            setTimeout(() => {
                document.body.classList.remove('page-transition-enter');
            }, 500);
        }, 300);
    }

    // Cleanup method
    destroy() {
        if (this.rafId) {
            cancelAnimationFrame(this.rafId);
        }
        
        // Remove created elements
        const elementsToRemove = [
            '.dynamic-gradient-bg',
            '.hex-pattern-overlay',
            '.circuit-overlay',
            '.particle-background'
        ];
        
        elementsToRemove.forEach(selector => {
            const element = document.querySelector(selector);
            if (element) {
                element.remove();
            }
        });
        
        this.particles = [];
        this.circuitLines = [];
        this.isInitialized = false;
    }
}

// Immediate modal fix function
function fixModalInteractivityNow() {
    const modals = document.querySelectorAll('.modal');
    modals.forEach(modal => {
        modal.style.zIndex = '10000';
        modal.style.pointerEvents = 'auto';
        
        // Ensure modal is accessible
        if (!modal.getAttribute('role')) {
            modal.setAttribute('role', 'dialog');
        }
        if (!modal.getAttribute('aria-modal')) {
            modal.setAttribute('aria-modal', 'true');
        }
        
        const modalDialog = modal.querySelector('.modal-dialog');
        if (modalDialog) {
            modalDialog.style.pointerEvents = 'auto';
            modalDialog.style.zIndex = '10001';
        }
        
        const modalContent = modal.querySelector('.modal-content');
        if (modalContent) {
            modalContent.style.pointerEvents = 'auto';
            modalContent.style.zIndex = '10002';
            
            // Improve visual feedback for modal content
            modalContent.style.outline = 'none';
            modalContent.style.boxShadow = '0 10px 30px rgba(0, 0, 0, 0.5), 0 0 20px var(--neon-purple-glow)';
        }
        
        const interactiveElements = modal.querySelectorAll('button, input, select, textarea, a, [role="button"], [tabindex], .btn, .form-control, .close, fluent-button, fluent-text-field, fluent-select, fluent-checkbox');
        interactiveElements.forEach(element => {
            element.style.pointerEvents = 'auto';
            element.style.position = 'relative';
            element.style.zIndex = '2';
            
            // Add hover effects for better UX
            if (!element.classList.contains('hover-enhanced')) {
                element.classList.add('hover-enhanced');
                element.addEventListener('mouseenter', () => {
                    element.style.transform = 'scale(1.02)';
                    element.style.transition = 'all 0.2s ease';
                });
                element.addEventListener('mouseleave', () => {
                    element.style.transform = 'scale(1)';
                });
            }
        });
        
        // Auto-focus first interactive element when modal shows
        if (modal.classList.contains('show')) {
            const firstFocusable = modal.querySelector('button, input, select, textarea, [tabindex]:not([tabindex="-1"])');
            if (firstFocusable) {
                setTimeout(() => firstFocusable.focus(), 100);
            }
        }
    });
    
    // Fix all modal backdrops to not block interactions
    const backdrops = document.querySelectorAll('.modal-backdrop');
    backdrops.forEach(backdrop => {
        backdrop.style.pointerEvents = 'none';
        backdrop.style.zIndex = '9999';
        
        // Enhance backdrop blur effect
        backdrop.style.backdropFilter = 'blur(8px)';
        backdrop.style.webkitBackdropFilter = 'blur(8px)';
    });
    
    // Additional backdrop fix for Bootstrap modals
    fixBootstrapBackdrops();
}

// Comprehensive Bootstrap backdrop fix
function fixBootstrapBackdrops() {
    // Find all backdrops including ones created by Bootstrap
    const allBackdrops = document.querySelectorAll('.modal-backdrop, .fade.modal-backdrop, .modal-backdrop.fade, .modal-backdrop.show');
    allBackdrops.forEach(backdrop => {
        backdrop.style.pointerEvents = 'none !important';
        backdrop.style.zIndex = '9999';
        backdrop.style.position = 'fixed';
    });
    
    // Override Bootstrap modal backdrop click behavior
    document.removeEventListener('click', handleBackdropClick, true);
    document.addEventListener('click', handleBackdropClick, true);
}

function handleBackdropClick(event) {
    // Allow clicks to pass through backdrop to modal content
    if (event.target.classList.contains('modal-backdrop')) {
        event.stopPropagation();
        event.preventDefault();
        return false;
    }
    
    // Handle clicks outside modal content for better UX
    if (event.target.classList.contains('modal') && !event.target.querySelector('.modal-content:hover')) {
        const modal = event.target;
        const closeBtn = modal.querySelector('.close, [data-dismiss="modal"]');
        if (closeBtn && modal.getAttribute('data-backdrop') !== 'static') {
            // Add subtle shake animation before closing
            const modalContent = modal.querySelector('.modal-content');
            if (modalContent) {
                modalContent.style.animation = 'modalShake 0.3s ease-in-out';
                setTimeout(() => {
                    modalContent.style.animation = '';
                    closeBtn.click();
                }, 300);
            } else {
                closeBtn.click();
            }
        }
    }
}

// Add CSS animation for modal shake effect
if (!document.getElementById('modal-animations-style')) {
    const style = document.createElement('style');
    style.id = 'modal-animations-style';
    style.textContent = `
        @keyframes modalShake {
            0%, 100% { transform: translateX(0); }
            25% { transform: translateX(-5px); }
            75% { transform: translateX(5px); }
        }
        
        .hover-enhanced:hover {
            filter: brightness(1.1);
            box-shadow: 0 0 10px rgba(255, 255, 255, 0.2);
        }
        
        .modal-content:focus {
            outline: 2px solid var(--neon-blue-start);
            outline-offset: 2px;
        }
    `;
    document.head.appendChild(style);
}

// Run modal fix immediately
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', fixModalInteractivityNow);
} else {
    fixModalInteractivityNow();
}

// Auto-initialize when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    // Initialize the effects manager
    window.neonEffects = new NeonEffectsManager();
    
    // Setup enhanced interactions
    NeonEffectsManager.setupAdvancedButtons();
    NeonEffectsManager.enhanceModals();
    
    // Fix modals immediately
    fixModalInteractivityNow();
    
    // Periodic modal check as backup
    setInterval(fixModalInteractivityNow, 1000);
    
    // Override Bootstrap modal methods to fix backdrop
    if (window.bootstrap && window.bootstrap.Modal) {
        const originalShow = window.bootstrap.Modal.prototype.show;
        window.bootstrap.Modal.prototype.show = function() {
            const result = originalShow.apply(this, arguments);
            setTimeout(fixBootstrapBackdrops, 100);
            return result;
        };
    }
    
    // Add CSS for ripple effects
    const rippleStyles = document.createElement('style');
    rippleStyles.textContent = `
        .ripple-effect {
            position: absolute;
            border-radius: 50%;
            background: rgba(139, 92, 246, 0.3);
            transform: scale(0);
            animation: ripple-animation 0.6s linear;
            pointer-events: none;
        }
        
        @keyframes ripple-animation {
            to {
                transform: scale(4);
                opacity: 0;
            }
        }
        
        .loading-spinner-neon {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.5rem;
            color: var(--neon-blue-end);
        }
        
        .spinner-ring {
            width: 20px;
            height: 20px;
            border: 2px solid rgba(139, 92, 246, 0.3);
            border-top: 2px solid var(--neon-purple-start);
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }
        
        /* Additional modal fix styles */
        .modal * { pointer-events: auto !important; }
        .modal { z-index: 10000 !important; }
        .modal .modal-dialog { z-index: 10001 !important; }
        .modal .modal-content { z-index: 10002 !important; }
        .modal-backdrop { pointer-events: none !important; z-index: 9999 !important; }
        .modal-backdrop.fade { pointer-events: none !important; }
        .modal-backdrop.show { pointer-events: none !important; }
    `;
    document.head.appendChild(rippleStyles);
    
    // Additional click event delegation for modals
    document.addEventListener('click', (e) => {
        if (e.target.closest('.modal')) {
            const modal = e.target.closest('.modal');
            setTimeout(() => fixModalInteractivityNow(), 10);
        }
    });
});

// Cleanup on page unload
window.addEventListener('beforeunload', () => {
    if (window.neonEffects) {
        window.neonEffects.destroy();
    }
});

// Export for module use
if (typeof module !== 'undefined' && module.exports) {
    module.exports = NeonEffectsManager;
}