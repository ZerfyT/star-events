// Simple Event.js - No errors
console.log('Event.js loaded successfully');

// Wait for DOM to be fully loaded
document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM loaded, initializing form...');

    // Initialize form validation
    initializeFormValidation();

    // Initialize date validation
    initializeDateValidation();

    console.log('Form initialization complete');
});

// Basic form validation
function initializeFormValidation() {
    const form = document.getElementById('eventForm');
    if (!form) {
        console.log('Form not found');
        return;
    }

    const inputs = form.querySelectorAll('input, select');
    console.log('Found inputs:', inputs.length);

    inputs.forEach(function (input) {
        input.addEventListener('blur', function () {
            validateField(input);
        });

        input.addEventListener('input', function () {
            clearErrors(input);
        });
    });
}

// Validate individual field
function validateField(field) {
    if (!field) return false;

    const value = field.value ? field.value.trim() : '';

    // Remove existing error styling
    field.classList.remove('error');

    // Check required fields
    if (field.hasAttribute('required') && !value) {
        showFieldError(field, 'This field is required');
        return false;
    }

    // Validate specific field types
    if (field.type === 'number' && value) {
        const numValue = parseInt(value);
        if (isNaN(numValue) || numValue <= 0) {
            showFieldError(field, 'Please enter a valid positive number');
            return false;
        }
    }

    return true;
}

// Show field error
function showFieldError(field, message) {
    if (!field) return;

    field.classList.add('error');

    let errorElement = field.parentNode.querySelector('.validation-error');
    if (!errorElement) {
        errorElement = document.createElement('span');
        errorElement.className = 'validation-error';
        field.parentNode.appendChild(errorElement);
    }

    errorElement.textContent = message;
    errorElement.style.display = 'block';
}

// Clear field errors
function clearErrors(field) {
    if (!field) return;

    field.classList.remove('error');

    const errorElement = field.parentNode.querySelector('.validation-error');
    if (errorElement) {
        errorElement.style.display = 'none';
    }
}

// Date validation
function initializeDateValidation() {
    const startDateInput = document.querySelector('input[name="StartDate"]');
    const endDateInput = document.querySelector('input[name="EndDate"]');

    if (!startDateInput || !endDateInput) {
        console.log('Date inputs not found');
        return;
    }

    // Set minimum date to today
    const today = new Date();
    const todayString = today.toISOString().slice(0, 16);
    startDateInput.min = todayString;

    startDateInput.addEventListener('change', function () {
        endDateInput.min = startDateInput.value;
        validateDates();
    });

    endDateInput.addEventListener('change', function () {
        validateDates();
    });
}

// Validate date range
function validateDates() {
    const startDateInput = document.querySelector('input[name="StartDate"]');
    const endDateInput = document.querySelector('input[name="EndDate"]');

    if (!startDateInput || !endDateInput) return true;

    const startDate = new Date(startDateInput.value);
    const endDate = new Date(endDateInput.value);
    const now = new Date();

    // Clear previous errors
    clearErrors(startDateInput);
    clearErrors(endDateInput);

    // Validate start date is not in the past
    if (startDate < now) {
        showFieldError(startDateInput, 'Start date cannot be in the past');
        return false;
    }

    // Validate end date is after start date
    if (endDate <= startDate) {
        showFieldError(endDateInput, 'End date must be after start date');
        return false;
    }

    return true;
}

// File handling (simplified version)
function handleFileSelect(input, posterNumber) {
    console.log('File selected for poster:', posterNumber);

    if (!input || !input.files || input.files.length === 0) {
        resetFileDisplay(posterNumber);
        return;
    }

    const file = input.files[0];

    // Validate file type
    if (!file.type.startsWith('image/')) {
        alert('Please select an image file.');
        input.value = '';
        resetFileDisplay(posterNumber);
        return;
    }

    // Validate file size (5MB limit)
    if (file.size > 5 * 1024 * 1024) {
        alert('File size should not exceed 5MB.');
        input.value = '';
        resetFileDisplay(posterNumber);
        return;
    }

    // Update display
    updateFileDisplay(file, posterNumber);
}

// Update file display
function updateFileDisplay(file, posterNumber) {
    const label = document.getElementById('label' + posterNumber);
    const fileInfo = document.getElementById('fileInfo' + posterNumber);
    const preview = document.getElementById('preview' + posterNumber);

    if (label) {
        label.classList.add('has-file');
        const span = label.querySelector('span');
        const icon = label.querySelector('i');

        if (span) span.textContent = 'Selected: ' + file.name;
        if (icon) icon.className = 'bi bi-check-circle';
    }

    if (fileInfo) {
        fileInfo.textContent = 'File: ' + file.name + ' (' + (file.size / 1024 / 1024).toFixed(2) + ' MB)';
        fileInfo.classList.add('show');
    }

    if (preview) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.innerHTML = '<img src="' + e.target.result + '" alt="Preview" style="max-width: 200px; max-height: 150px; border-radius: 8px;">';
            preview.classList.add('show');
        };
        reader.readAsDataURL(file);
    }
}

// Reset file display
function resetFileDisplay(posterNumber) {
    const label = document.getElementById('label' + posterNumber);
    const fileInfo = document.getElementById('fileInfo' + posterNumber);
    const preview = document.getElementById('preview' + posterNumber);

    if (label) {
        label.classList.remove('has-file');
        const span = label.querySelector('span');
        const icon = label.querySelector('i');

        if (span) span.textContent = 'Click to upload poster ' + posterNumber;
        if (icon) icon.className = 'bi bi-plus-circle';
    }

    if (fileInfo) {
        fileInfo.classList.remove('show');
    }

    if (preview) {
        preview.classList.remove('show');
        preview.innerHTML = '';
    }
}