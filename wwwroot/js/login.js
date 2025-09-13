// TixWave Authentication JavaScript
document.addEventListener('DOMContentLoaded', function () {
    initializeAuthForm();
});

function initializeAuthForm() {
    // Initialize password toggles
    initializePasswordToggles();

    // Initialize form validation
    initializeFormValidation();

    // Initialize input animations
    initializeInputAnimations();

    // Initialize form submissions
    initializeFormSubmissions();

    // Initialize real-time validation
    initializeRealTimeValidation();
}

// Password visibility toggle functionality
function initializePasswordToggles() {
    const passwordToggles = document.querySelectorAll('.password-toggle');

    passwordToggles.forEach(toggle => {
        toggle.addEventListener('click', function () {
            const input = this.previousElementSibling;
            const icon = this.querySelector('i');

            if (input.type === 'password') {
                input.type = 'text';
                icon.classList.remove('bx-hide');
                icon.classList.add('bx-show');
                this.setAttribute('title', 'Hide password');
            } else {
                input.type = 'password';
                icon.classList.remove('bx-show');
                icon.classList.add('bx-hide');
                this.setAttribute('title', 'Show password');
            }
        });
    });
}

// Form validation initialization
function initializeFormValidation() {
    const forms = document.querySelectorAll('.auth-form');

    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            if (!validateForm(this)) {
                e.preventDefault();
            }
        });
    });
}

// Input focus animations
function initializeInputAnimations() {
    const formInputs = document.querySelectorAll('.form-control');

    formInputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.parentElement.classList.add('focused');
            clearFieldError(this);
        });

        input.addEventListener('blur', function () {
            this.parentElement.classList.remove('focused');
            if (this.value.trim() !== '') {
                this.parentElement.classList.add('has-value');
            } else {
                this.parentElement.classList.remove('has-value');
            }
        });

        // Check if field has value on page load
        if (input.value.trim() !== '') {
            input.parentElement.classList.add('has-value');
        }
    });
}

// Form submission handling
function initializeFormSubmissions() {
    const submitButtons = document.querySelectorAll('.submit-btn');

    submitButtons.forEach(button => {
        const form = button.closest('form');
        if (form) {
            form.addEventListener('submit', function (e) {
                showLoadingState(button);
            });
        }
    });
}

// Real-time validation
function initializeRealTimeValidation() {
    // Username validation
    const usernameInput = document.getElementById('Username');
    if (usernameInput) {
        usernameInput.addEventListener('input', debounce(function () {
            validateUsername(this);
        }, 500));
    }

    // Email validation
    const emailInput = document.getElementById('Email');
    if (emailInput) {
        emailInput.addEventListener('input', debounce(function () {
            validateEmail(this);
        }, 500));
    }

    // Password validation
    const passwordInput = document.getElementById('Password');
    if (passwordInput) {
        passwordInput.addEventListener('input', function () {
            validatePassword(this);
        });
    }

    // Confirm password validation
    const confirmPasswordInput = document.getElementById('ConfirmPassword');
    if (confirmPasswordInput && passwordInput) {
        confirmPasswordInput.addEventListener('input', function () {
            validateConfirmPassword(this, passwordInput);
        });
    }

    // Contact number validation
    const contactInput = document.getElementById('ContactNo');
    if (contactInput) {
        contactInput.addEventListener('input', function () {
            // Only allow numbers
            this.value = this.value.replace(/[^0-9]/g, '');
            if (this.value.length <= 10) {
                validateContactNumber(this);
            }
        });
    }

    // NIC validation
    const nicInput = document.getElementById('NIC');
    if (nicInput) {
        nicInput.addEventListener('input', function () {
            validateNIC(this);
        });
    }
}

// Form validation function
function validateForm(form) {
    let isValid = true;
    clearAllErrors(form);

    const requiredFields = form.querySelectorAll('[required]');

    requiredFields.forEach(field => {
        if (!validateRequiredField(field)) {
            isValid = false;
        }
    });

    // Additional specific validations
    const emailField = form.querySelector('#Email');
    if (emailField && emailField.value.trim() !== '') {
        if (!validateEmail(emailField)) {
            isValid = false;
        }
    }

    const passwordField = form.querySelector('#Password');
    if (passwordField) {
        if (!validatePassword(passwordField)) {
            isValid = false;
        }
    }

    const confirmPasswordField = form.querySelector('#ConfirmPassword');
    if (confirmPasswordField && passwordField) {
        if (!validateConfirmPassword(confirmPasswordField, passwordField)) {
            isValid = false;
        }
    }

    const contactField = form.querySelector('#ContactNo');
    if (contactField) {
        if (!validateContactNumber(contactField)) {
            isValid = false;
        }
    }

    const nicField = form.querySelector('#NIC');
    if (nicField) {
        if (!validateNIC(nicField)) {
            isValid = false;
        }
    }

    return isValid;
}

// Individual field validations
function validateRequiredField(field) {
    if (field.value.trim() === '') {
        showFieldError(field, `${getFieldLabel(field)} is required`);
        return false;
    }
    return true;
}

function validateUsername(field) {
    const value = field.value.trim();
    if (value === '') return true; // Let required validation handle empty

    if (value.length < 3) {
        showFieldError(field, 'Username must be at least 3 characters long');
        return false;
    }

    if (value.length > 30) {
        showFieldError(field, 'Username must not exceed 30 characters');
        return false;
    }

    if (!/^[a-zA-Z0-9_]+$/.test(value)) {
        showFieldError(field, 'Username can only contain letters, numbers, and underscores');
        return false;
    }

    showFieldSuccess(field);
    return true;
}

function validateEmail(field) {
    const value = field.value.trim();
    if (value === '') return true; // Let required validation handle empty

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(value)) {
        showFieldError(field, 'Please enter a valid email address');
        return false;
    }

    showFieldSuccess(field);
    return true;
}

function validatePassword(field) {
    const value = field.value;
    if (value === '') return true; // Let required validation handle empty

    if (value.length < 6) {
        showFieldError(field, 'Password must be at least 6 characters long');
        return false;
    }

    if (value.length > 100) {
        showFieldError(field, 'Password must not exceed 100 characters');
        return false;
    }

    // Check for at least one uppercase, lowercase, number, and special character
    const hasUpper = /[A-Z]/.test(value);
    const hasLower = /[a-z]/.test(value);
    const hasNumber = /\d/.test(value);
    const hasSpecial = /[@$!%*?&]/.test(value);

    if (!hasUpper || !hasLower || !hasNumber || !hasSpecial) {
        showFieldError(field, 'Password must contain uppercase, lowercase, number, and special character');
        return false;
    }

    showFieldSuccess(field);
    return true;
}

function validateConfirmPassword(field, passwordField) {
    const value = field.value;
    const passwordValue = passwordField.value;

    if (value === '') return true; // Let required validation handle empty

    if (value !== passwordValue) {
        showFieldError(field, 'Passwords do not match');
        return false;
    }

    showFieldSuccess(field);
    return true;
}

function validateContactNumber(field) {
    const value = field.value.trim();
    if (value === '') return true; // Let required validation handle empty

    if (!/^[0-9]{10}$/.test(value)) {
        showFieldError(field, 'Contact number must be exactly 10 digits');
        return false;
    }

    showFieldSuccess(field);
    return true;
}

function validateNIC(field) {
    const value = field.value.trim().toUpperCase();
    if (value === '') return true; // Let required validation handle empty

    // Old NIC format: 9 digits + V/X
    const oldNicRegex = /^[0-9]{9}[VX]$/;
    // New NIC format: 12 digits
    const newNicRegex = /^[0-9]{12}$/;

    if (!oldNicRegex.test(value) && !newNicRegex.test(value)) {
        showFieldError(field, 'Invalid NIC format. Use 9 digits + V/X or 12 digits');
        return false;
    }

    showFieldSuccess(field);
    return true;
}

function validateUserRole(field) {
    const value = field.value.trim();
    if (value === '' || value === 'Select User Type') {
        showFieldError(field, 'Please select a user role');
        return false;
    }

    showFieldSuccess(field);
    return true;
}

// Helper functions
function showFieldError(field, message) {
    field.classList.remove('is-valid');
    field.classList.add('is-invalid');

    // Remove existing error message
    const existingError = field.parentElement.querySelector('.text-danger');
    if (existingError) {
        existingError.remove();
    }

    // Add new error message
    const errorDiv = document.createElement('div');
    errorDiv.className = 'text-danger';
    errorDiv.textContent = message;
    field.parentElement.appendChild(errorDiv);
}

function showFieldSuccess(field) {
    field.classList.remove('is-invalid');
    field.classList.add('is-valid');
    clearFieldError(field);
}

function clearFieldError(field) {
    field.classList.remove('is-invalid');
    const errorMessage = field.parentElement.querySelector('.text-danger');
    if (errorMessage) {
        errorMessage.remove();
    }
}

function clearAllErrors(form) {
    const errorMessages = form.querySelectorAll('.text-danger');
    const invalidFields = form.querySelectorAll('.is-invalid');

    errorMessages.forEach(error => error.remove());
    invalidFields.forEach(field => field.classList.remove('is-invalid'));
}

function getFieldLabel(field) {
    const label = field.parentElement.querySelector('.form-label');
    return label ? label.textContent.replace('*', '').trim() : 'Field';
}

function showLoadingState(button) {
    button.disabled = true;
    const originalContent = button.innerHTML;
    button.setAttribute('data-original-content', originalContent);

    const isRegisterForm = button.textContent.includes('Create Account');
    const loadingText = isRegisterForm ? 'Creating Account...' : 'Signing In...';

    button.innerHTML = `
        <span class="spinner"></span>
        ${loadingText}
    `;
}

function hideLoadingState(button) {
    button.disabled = false;
    const originalContent = button.getAttribute('data-original-content');
    if (originalContent) {
        button.innerHTML = originalContent;
        button.removeAttribute('data-original-content');
    }
}

// Utility function for debouncing
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Handle form submission responses
function handleFormResponse(success, message, redirectUrl = null) {
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => alert.remove());

    const form = document.querySelector('.auth-form');
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert ${success ? 'alert-success' : 'alert-danger'}`;
    alertDiv.innerHTML = `
        <i class="bx ${success ? 'bx-check-circle' : 'bx-error-circle'}"></i>
        ${message}
    `;

    form.insertBefore(alertDiv, form.firstChild);

    // Hide loading state
    const submitBtn = document.querySelector('.submit-btn');
    if (submitBtn) {
        hideLoadingState(submitBtn);
    }

    // Redirect if successful and redirect URL provided
    if (success && redirectUrl) {
        setTimeout(() => {
            window.location.href = redirectUrl;
        }, 1500);
    }

    // Scroll to top to show alert
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

// Auto-hide alerts after 5 seconds
function autoHideAlerts() {
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.opacity = '0';
            alert.style.transform = 'translateY(-20px)';
            setTimeout(() => {
                alert.remove();
            }, 300);
        }, 5000);
    });
}

// Initialize auto-hide for existing alerts
setTimeout(autoHideAlerts, 100);

// Keyboard shortcuts
document.addEventListener('keydown', function (e) {
    // Enter key to submit form
    if (e.key === 'Enter' && !e.shiftKey) {
        const activeElement = document.activeElement;
        if (activeElement.classList.contains('form-control')) {
            const form = activeElement.closest('form');
            if (form) {
                e.preventDefault();
                const submitBtn = form.querySelector('.submit-btn');
                if (submitBtn && !submitBtn.disabled) {
                    submitBtn.click();
                }
            }
        }
    }
});

// Handle browser back button
window.addEventListener('pageshow', function (event) {
    if (event.persisted) {
        // Page was loaded from cache, reset form states
        const forms = document.querySelectorAll('.auth-form');
        forms.forEach(form => {
            clearAllErrors(form);
            const submitBtn = form.querySelector('.submit-btn');
            if (submitBtn) {
                hideLoadingState(submitBtn);
            }
        });
    }
});

// Initialize tooltips for password requirements
function initializePasswordTooltips() {
    const passwordField = document.getElementById('Password');
    if (passwordField) {
        const tooltip = document.createElement('div');
        tooltip.className = 'password-requirements';
        tooltip.innerHTML = `
            <div class="requirements-title">Password Requirements:</div>
            <ul>
                <li>At least 6 characters long</li>
                <li>One uppercase letter (A-Z)</li>
                <li>One lowercase letter (a-z)</li>
                <li>One number (0-9)</li>
                <li>One special character (@$!%*?&)</li>
            </ul>
        `;

        passwordField.addEventListener('focus', function () {
            if (!document.querySelector('.password-requirements')) {
                this.parentElement.appendChild(tooltip);
            }
        });

        passwordField.addEventListener('blur', function () {
            const existingTooltip = document.querySelector('.password-requirements');
            if (existingTooltip) {
                existingTooltip.remove();
            }
        });
    }
}

// Add CSS for password requirements tooltip
const style = document.createElement('style');
style.textContent = `
    .password-requirements {
        position: absolute;
        top: 100%;
        left: 0;
        right: 0;
        background: white;
        border: 1px solid #e1e5e9;
        border-radius: 8px;
        padding: 1rem;
        font-size: 0.8rem;
        z-index: 1000;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        margin-top: 0.25rem;
    }
    
    .requirements-title {
        font-weight: 600;
        margin-bottom: 0.5rem;
        color: var(--Alpine_Dark_Blue);
    }
    
    .password-requirements ul {
        margin: 0;
        padding-left: 1rem;
        list-style-type: disc;
    }
    
    .password-requirements li {
        margin-bottom: 0.25rem;
        color: var(--Gray_Medium);
    }
`;
document.head.appendChild(style);