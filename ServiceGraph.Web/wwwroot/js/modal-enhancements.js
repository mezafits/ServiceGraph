/**
 * Enhanced Modal Accessibility and Interaction Utilities
 * Provides comprehensive modal management with improved UX and accessibility
 */

class ModalEnhancer {
    constructor() {
        this.activeModal = null;
        this.focusableElements = [
            'button', 'input', 'select', 'textarea', 'a[href]', 
            '[tabindex]:not([tabindex="-1"])', '[role="button"]',
            'fluent-button', 'fluent-text-field', 'fluent-select', 
            'fluent-checkbox', '.btn', '.form-control'
        ].join(', ');
        this.init();
    }

    init() {
        this.setupEventListeners();
        this.enhanceExistingModals();
        this.observeNewModals();
    }

    setupEventListeners() {
        // Enhanced keyboard navigation
        document.addEventListener('keydown', (e) => this.handleKeyboard(e));
        
        // Modal show/hide events
        document.addEventListener('show.bs.modal', (e) => this.onModalShow(e));
        document.addEventListener('hide.bs.modal', (e) => this.onModalHide(e));
        document.addEventListener('shown.bs.modal', (e) => this.onModalShown(e));
        document.addEventListener('hidden.bs.modal', (e) => this.onModalHidden(e));

        // Enhanced backdrop handling
        document.addEventListener('click', (e) => this.handleBackdropClick(e), true);
    }

    handleKeyboard(e) {
        if (!this.activeModal) return;

        switch (e.key) {
            case 'Escape':
                this.handleEscapeKey(e);
                break;
            case 'Tab':
                this.handleTabKey(e);
                break;
            case 'Enter':
                this.handleEnterKey(e);
                break;
            case ' ':
                this.handleSpaceKey(e);
                break;
        }
    }

    handleEscapeKey(e) {
        const modal = this.activeModal;
        if (modal && !modal.hasAttribute('data-keyboard-disabled')) {
            e.preventDefault();
            this.closeModal(modal);
        }
    }

    handleTabKey(e) {
        const modal = this.activeModal;
        if (!modal) return;

        const focusableElements = Array.from(modal.querySelectorAll(this.focusableElements))
            .filter(el => this.isElementVisible(el) && !el.disabled);

        if (focusableElements.length === 0) return;

        const firstElement = focusableElements[0];
        const lastElement = focusableElements[focusableElements.length - 1];

        if (e.shiftKey) {
            if (document.activeElement === firstElement) {
                e.preventDefault();
                lastElement.focus();
            }
        } else {
            if (document.activeElement === lastElement) {
                e.preventDefault();
                firstElement.focus();
            }
        }
    }

    handleEnterKey(e) {
        const target = e.target;
        if (target.matches('button, [role="button"], .btn') && !target.disabled) {
            if (!target.matches('input[type="submit"], input[type="button"]')) {
                e.preventDefault();
                target.click();
            }
        }
    }

    handleSpaceKey(e) {
        const target = e.target;
        if (target.matches('[role="button"], .btn') && !target.matches('input, textarea, select')) {
            e.preventDefault();
            target.click();
        }
    }

    onModalShow(e) {
        const modal = e.target;
        this.activeModal = modal;
        this.enhanceModal(modal);
        this.storeLastFocus();
    }

    onModalShown(e) {
        const modal = e.target;
        this.setInitialFocus(modal);
        this.announceModal(modal);
    }

    onModalHide(e) {
        const modal = e.target;
        this.addClosingAnimation(modal);
    }

    onModalHidden(e) {
        const modal = e.target;
        this.activeModal = null;
        this.restoreLastFocus();
        this.cleanupModal(modal);
    }

    enhanceModal(modal) {
        // Set proper ARIA attributes
        if (!modal.getAttribute('role')) {
            modal.setAttribute('role', 'dialog');
        }
        if (!modal.getAttribute('aria-modal')) {
            modal.setAttribute('aria-modal', 'true');
        }

        // Enhance modal styling and interactions
        modal.style.zIndex = '10000';
        modal.style.pointerEvents = 'auto';

        const modalDialog = modal.querySelector('.modal-dialog');
        if (modalDialog) {
            modalDialog.style.pointerEvents = 'auto';
            modalDialog.style.zIndex = '10001';
        }

        const modalContent = modal.querySelector('.modal-content');
        if (modalContent) {
            modalContent.style.pointerEvents = 'auto';
            modalContent.style.zIndex = '10002';
            modalContent.setAttribute('tabindex', '-1');
        }

        // Fix backdrop
        this.fixBackdrop(modal);

        // Enhance interactive elements
        this.enhanceInteractiveElements(modal);

        // Add modal enhancement class
        modal.classList.add('modal-enhanced');
    }

    fixBackdrop(modal) {
        setTimeout(() => {
            const backdrops = document.querySelectorAll('.modal-backdrop');
            backdrops.forEach(backdrop => {
                backdrop.style.pointerEvents = 'none !important';
                backdrop.style.zIndex = '9999';
                backdrop.style.backdropFilter = 'blur(8px)';
                backdrop.style.webkitBackdropFilter = 'blur(8px)';
                backdrop.classList.add('backdrop-enhanced');
            });
        }, 50);
    }

    enhanceInteractiveElements(modal) {
        const elements = modal.querySelectorAll(this.focusableElements);
        elements.forEach(element => {
            element.style.pointerEvents = 'auto';
            element.style.position = 'relative';
            element.style.zIndex = '2';

            // Add enhanced hover effects
            if (!element.classList.contains('hover-enhanced')) {
                element.classList.add('hover-enhanced');
                this.addHoverEffects(element);
            }

            // Ensure proper focus indicators
            if (!element.style.outline) {
                element.addEventListener('focus', () => {
                    element.style.outline = '2px solid var(--neon-blue-start)';
                    element.style.outlineOffset = '2px';
                });
                element.addEventListener('blur', () => {
                    element.style.outline = '';
                    element.style.outlineOffset = '';
                });
            }
        });
    }

    addHoverEffects(element) {
        element.addEventListener('mouseenter', () => {
            if (!element.disabled) {
                element.style.transform = 'scale(1.02)';
                element.style.transition = 'all 0.2s ease';
                element.style.filter = 'brightness(1.1)';
                element.style.boxShadow = '0 0 10px rgba(255, 255, 255, 0.2)';
            }
        });

        element.addEventListener('mouseleave', () => {
            element.style.transform = 'scale(1)';
            element.style.filter = '';
            element.style.boxShadow = '';
        });
    }

    setInitialFocus(modal) {
        // Focus priority: error elements, primary button, first focusable
        const focusTargets = [
            '.alert-danger input, .alert-danger button',
            '.btn-primary:not(:disabled)',
            '.modal-footer .btn:not(:disabled)',
            this.focusableElements
        ];

        for (const selector of focusTargets) {
            const element = modal.querySelector(selector);
            if (element && this.isElementVisible(element) && !element.disabled) {
                setTimeout(() => {
                    element.focus();
                    if (element.select && element.type === 'text') {
                        element.select();
                    }
                }, 100);
                break;
            }
        }
    }

    storeLastFocus() {
        this.lastFocusedElement = document.activeElement;
    }

    restoreLastFocus() {
        if (this.lastFocusedElement && this.isElementVisible(this.lastFocusedElement)) {
            setTimeout(() => {
                this.lastFocusedElement.focus();
            }, 100);
        }
    }

    announceModal(modal) {
        const title = modal.querySelector('.modal-title');
        if (title) {
            const announcement = document.createElement('div');
            announcement.setAttribute('aria-live', 'polite');
            announcement.setAttribute('aria-atomic', 'true');
            announcement.className = 'sr-only';
            announcement.textContent = `Dialog opened: ${title.textContent}`;
            document.body.appendChild(announcement);
            setTimeout(() => document.body.removeChild(announcement), 1000);
        }
    }

    closeModal(modal) {
        const closeBtn = modal.querySelector('.close, [data-dismiss="modal"], [data-bs-dismiss="modal"]');
        if (closeBtn) {
            closeBtn.click();
        } else {
            // Fallback: trigger Bootstrap modal hide
            if (window.bootstrap?.Modal) {
                const modalInstance = window.bootstrap.Modal.getInstance(modal);
                if (modalInstance) {
                    modalInstance.hide();
                }
            }
        }
    }

    addClosingAnimation(modal) {
        modal.classList.add('closing');
        const modalContent = modal.querySelector('.modal-content');
        if (modalContent) {
            modalContent.style.animation = 'modalSlideOut 0.3s ease-out';
        }
    }

    cleanupModal(modal) {
        modal.classList.remove('modal-enhanced', 'closing');
        const modalContent = modal.querySelector('.modal-content');
        if (modalContent) {
            modalContent.style.animation = '';
        }
    }

    handleBackdropClick(e) {
        if (e.target.classList.contains('modal-backdrop')) {
            e.stopPropagation();
            e.preventDefault();
            return false;
        }

        // Enhanced outside click handling
        if (e.target.classList.contains('modal') && this.activeModal) {
            const modalContent = this.activeModal.querySelector('.modal-content');
            if (modalContent && !modalContent.contains(e.target)) {
                if (!this.activeModal.hasAttribute('data-backdrop-static')) {
                    this.shakeAndClose(this.activeModal);
                } else {
                    this.shakeModal(this.activeModal);
                }
            }
        }
    }

    shakeAndClose(modal) {
        const modalContent = modal.querySelector('.modal-content');
        if (modalContent) {
            modalContent.style.animation = 'modalShake 0.3s ease-in-out';
            setTimeout(() => {
                modalContent.style.animation = '';
                this.closeModal(modal);
            }, 300);
        } else {
            this.closeModal(modal);
        }
    }

    shakeModal(modal) {
        const modalContent = modal.querySelector('.modal-content');
        if (modalContent) {
            modalContent.style.animation = 'modalShake 0.3s ease-in-out';
            setTimeout(() => {
                modalContent.style.animation = '';
            }, 300);
        }
    }

    isElementVisible(element) {
        return !!(element.offsetWidth || element.offsetHeight || element.getClientRects().length);
    }

    enhanceExistingModals() {
        const modals = document.querySelectorAll('.modal');
        modals.forEach(modal => {
            if (modal.classList.contains('show')) {
                this.enhanceModal(modal);
                this.activeModal = modal;
            }
        });
    }

    observeNewModals() {
        const observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                mutation.addedNodes.forEach((node) => {
                    if (node.nodeType === 1) {
                        if (node.classList && node.classList.contains('modal')) {
                            setTimeout(() => this.enhanceExistingModals(), 50);
                        }
                        const modals = node.querySelectorAll && node.querySelectorAll('.modal');
                        if (modals && modals.length > 0) {
                            setTimeout(() => this.enhanceExistingModals(), 50);
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
}

// Add enhanced CSS animations
if (!document.getElementById('modal-enhancement-styles')) {
    const style = document.createElement('style');
    style.id = 'modal-enhancement-styles';
    style.textContent = `
        /* Enhanced modal animations */
        @keyframes modalSlideOut {
            0% {
                transform: scale(1) translateY(0);
                opacity: 1;
            }
            100% {
                transform: scale(0.8) translateY(-20px);
                opacity: 0;
            }
        }

        @keyframes modalShake {
            0%, 100% { transform: translateX(0); }
            25% { transform: translateX(-8px); }
            75% { transform: translateX(8px); }
        }

        /* Enhanced focus styles */
        .modal-enhanced .hover-enhanced:focus {
            outline: 2px solid var(--neon-blue-start) !important;
            outline-offset: 2px !important;
            box-shadow: 0 0 15px var(--neon-blue-glow) !important;
        }

        /* Improved backdrop */
        .backdrop-enhanced {
            transition: all 0.3s ease-out !important;
        }

        .backdrop-enhanced.show {
            backdrop-filter: blur(12px) !important;
            -webkit-backdrop-filter: blur(12px) !important;
        }

        /* Loading state for modals */
        .modal-content.loading {
            position: relative;
            pointer-events: none;
        }

        .modal-content.loading::after {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.5);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 1000;
        }

        /* Screen reader only content */
        .sr-only {
            position: absolute !important;
            width: 1px !important;
            height: 1px !important;
            padding: 0 !important;
            margin: -1px !important;
            overflow: hidden !important;
            clip: rect(0, 0, 0, 0) !important;
            white-space: nowrap !important;
            border: 0 !important;
        }

        /* Enhanced button states */
        .modal-enhanced .btn:disabled {
            opacity: 0.6;
            cursor: not-allowed;
            transform: none !important;
        }

        .modal-enhanced .btn:not(:disabled):active {
            transform: scale(0.98) !important;
        }
    `;
    document.head.appendChild(style);
}

// Utility functions for external use
window.ModalEnhancer = {
    showModal: function(modalId, options = {}) {
        const modal = document.getElementById(modalId);
        if (modal) {
            if (options.backdrop === 'static') {
                modal.setAttribute('data-backdrop-static', 'true');
            }
            if (options.keyboard === false) {
                modal.setAttribute('data-keyboard-disabled', 'true');
            }
            
            // Use Bootstrap if available, otherwise fallback
            if (window.bootstrap?.Modal) {
                const modalInstance = new window.bootstrap.Modal(modal, options);
                modalInstance.show();
            } else {
                modal.classList.add('show');
                modal.style.display = 'block';
                document.body.classList.add('modal-open');
            }
        }
    },

    hideModal: function(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            if (window.bootstrap?.Modal) {
                const modalInstance = window.bootstrap.Modal.getInstance(modal);
                if (modalInstance) {
                    modalInstance.hide();
                }
            } else {
                modal.classList.remove('show');
                modal.style.display = 'none';
                document.body.classList.remove('modal-open');
            }
        }
    },

    setModalLoading: function(modalId, loading = true) {
        const modal = document.getElementById(modalId);
        if (modal) {
            const modalContent = modal.querySelector('.modal-content');
            if (modalContent) {
                if (loading) {
                    modalContent.classList.add('loading');
                } else {
                    modalContent.classList.remove('loading');
                }
            }
        }
    }
};

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        new ModalEnhancer();
    });
} else {
    new ModalEnhancer();
}

// Export for module systems
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ModalEnhancer;
}