document.addEventListener('DOMContentLoaded', function () {
    console.log('Profile page loaded');

    // Initialize form handlers
    initializeProfileForm();
    initializePasswordForm();
    initializePasswordStrength();
});

function initializeProfileForm() {
    const profileForm = document.getElementById('profileForm');
    if (!profileForm) return;

    // Store original values for reset functionality
    const originalValues = {
        firstName: document.getElementById('firstName').value,
        lastName: document.getElementById('lastName').value,
        email: document.getElementById('email').value,
        phoneNumber: document.getElementById('phoneNumber').value
    };

    profileForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        const submitBtn = this.querySelector('button[type="submit"]');
        const originalText = submitBtn.innerHTML;

        // Show loading state
        submitBtn.classList.add('loading');
        submitBtn.innerHTML = '<i class="bx bx-loader-alt bx-spin"></i> Updating...';

        try {
            const formData = new FormData(this);
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            formData.append('__RequestVerificationToken', token);

            const response = await fetch('/Home/UpdateProfile', {
                method: 'POST',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: formData
            });

            const result = await response.json();

            if (result.success) {
                showAlert('success', result.message);
                // Update original values after successful update
                Object.assign(originalValues, {
                    firstName: formData.get('FirstName'),
                    lastName: formData.get('LastName'),
                    email: formData.get('Email'),
                    phoneNumber: formData.get('PhoneNumber')
                });
            } else {
                showAlert('danger', result.message);
            }
        } catch (error) {
            console.error('Error updating profile:', error);
            showAlert('danger', 'An error occurred while updating your profile. Please try again.');
        } finally {
            // Reset button state
            submitBtn.classList.remove('loading');
            submitBtn.innerHTML = originalText;
        }
    });

    // Reset function
    window.resetProfileForm = function () {
        document.getElementById('firstName').value = originalValues.firstName;
        document.getElementById('lastName').value = originalValues.lastName;
        document.getElementById('email').value = originalValues.email;
        document.getElementById('phoneNumber').value = originalValues.phoneNumber;
        clearAlerts();
    };
}

function initializePasswordForm() {
    const passwordForm = document.getElementById('passwordForm');
    if (!passwordForm) return;

    passwordForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        const newPassword = document.getElementById('newPassword').value;
        const confirmPassword = document.getElementById('confirmPassword').value;

        // Validate password confirmation
        if (newPassword !== confirmPassword) {
            showAlert('danger', 'New password and confirm password do not match.');
            return;
        }

        const submitBtn = this.querySelector('button[type="submit"]');
        const originalText = submitBtn.innerHTML;

        // Show loading state
        submitBtn.classList.add('loading');
        submitBtn.innerHTML = '<i class="bx bx-loader-alt bx-spin"></i> Changing...';

        try {
            const formData = new FormData(this);
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            formData.append('__RequestVerificationToken', token);

            const response = await fetch('/Home/ChangePassword', {
                method: 'POST',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: formData
            });

            const result = await response.json();

            if (result.success) {
                showAlert('success', result.message);
                resetPasswordForm();
            } else {
                showAlert('danger', result.message);
            }
        } catch (error) {
            console.error('Error changing password:', error);
            showAlert('danger', 'An error occurred while changing your password. Please try again.');
        } finally {
            // Reset button state
            submitBtn.classList.remove('loading');
            submitBtn.innerHTML = originalText;
        }
    });

    // Reset function
    window.resetPasswordForm = function () {
        document.getElementById('currentPassword').value = '';
        document.getElementById('newPassword').value = '';
        document.getElementById('confirmPassword').value = '';
        updatePasswordStrength('');
        clearAlerts();
    };
}

function initializePasswordStrength() {
    const newPasswordInput = document.getElementById('newPassword');
    if (!newPasswordInput) return;

    newPasswordInput.addEventListener('input', function () {
        updatePasswordStrength(this.value);
    });
}

function updatePasswordStrength(password) {
    const strengthFill = document.getElementById('strengthFill');
    const strengthText = document.getElementById('strengthText');

    if (!strengthFill || !strengthText) return;

    // Reset classes
    strengthFill.className = 'strength-fill';

    if (!password) {
        strengthText.textContent = 'Enter a password';
        return;
    }

    let strength = 0;
    let feedback = '';

    // Length check
    if (password.length >= 8) strength++;
    else feedback = 'At least 8 characters';

    // Lowercase check
    if (/[a-z]/.test(password)) strength++;
    else if (!feedback) feedback = 'Add lowercase letters';

    // Uppercase check
    if (/[A-Z]/.test(password)) strength++;
    else if (!feedback) feedback = 'Add uppercase letters';

    // Number check
    if (/\d/.test(password)) strength++;
    else if (!feedback) feedback = 'Add numbers';

    // Special character check
    if (/[!@#$%^&*(),.?":{}|<>]/.test(password)) strength++;
    else if (!feedback) feedback = 'Add special characters';

    // Update UI based on strength
    switch (strength) {
        case 0:
        case 1:
            strengthFill.classList.add('weak');
            strengthText.textContent = 'Weak password';
            break;
        case 2:
            strengthFill.classList.add('fair');
            strengthText.textContent = 'Fair password';
            break;
        case 3:
        case 4:
            strengthFill.classList.add('good');
            strengthText.textContent = 'Good password';
            break;
        case 5:
            strengthFill.classList.add('strong');
            strengthText.textContent = 'Strong password';
            break;
    }

    // Show feedback if password is weak
    if (strength < 3 && feedback) {
        strengthText.textContent = feedback;
    }
}

function showAlert(type, message) {
    // Remove existing alerts
    clearAlerts();

    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type}`;

    const icon = type === 'success' ? 'bx-check-circle' : 'bx-error-circle';
    alertDiv.innerHTML = `
        <i class='bx ${icon}'></i>
        <span>${message}</span>
    `;

    // Insert at the top of the active tab content
    const activeTab = document.querySelector('.tab-pane.active');
    if (activeTab) {
        activeTab.insertBefore(alertDiv, activeTab.firstChild);

        // Auto-remove after 5 seconds
        setTimeout(() => {
            if (alertDiv.parentNode) {
                alertDiv.remove();
            }
        }, 5000);
    }
}

function clearAlerts() {
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => alert.remove());
}

// Tab switching with smooth transitions
document.addEventListener('DOMContentLoaded', function () {
    const tabButtons = document.querySelectorAll('[data-bs-toggle="tab"]');

    tabButtons.forEach(button => {
        button.addEventListener('click', function () {
            // Clear any existing alerts when switching tabs
            clearAlerts();
        });
    });
});

// Form validation
function validateForm(form) {
    const requiredFields = form.querySelectorAll('[required]');
    let isValid = true;

    requiredFields.forEach(field => {
        if (!field.value.trim()) {
            field.classList.add('is-invalid');
            isValid = false;
        } else {
            field.classList.remove('is-invalid');
        }
    });

    return isValid;
}

// Add CSS for invalid fields
const style = document.createElement('style');
style.textContent = `
    .form-control.is-invalid {
        border-color: #F44336;
        box-shadow: 0 0 0 0.2rem rgba(244, 67, 54, 0.25);
    }
`;
document.head.appendChild(style);
