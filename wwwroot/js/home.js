document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM Content Loaded - Starting initialization');

    // 1. NAVIGATION CODE
    const navLinks = document.querySelectorAll('.nav-pills .nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            navLinks.forEach(nav => nav.classList.remove('active'));
            this.classList.add('active');
            const page = this.getAttribute('data-page');
            console.log('Navigating to:', page);
        });
    });

    // 2. CAROUSEL CODE (keeping your existing code)
    const carouselSlides = [
        {
            id: 1,
            videoSrc: "/videos/weeknd-concert.mp4",
            title: "Feel the Music Live",
            subtitle: "Book your tickets to the hottest concerts in town.",
            buttonText: "Explore Now",
            buttonIcon: "bx bx-right-arrow-alt",
            isActive: true
        },
        {
            id: 2,
            videoSrc: "/videos/taylor-swift.mp4",
            title: "On the Spotlight",
            subtitle: "Events everyone's raving about — don't miss out.",
            buttonText: "Explore Now",
            buttonIcon: "bx bx-right-arrow-alt",
            isActive: false
        }
    ];

    function generateCarouselHTML() {
        const carouselInner = document.querySelector('.carousel-inner');
        if (!carouselInner) return;

        carouselInner.innerHTML = '';
        carouselSlides.forEach((slide, index) => {
            const slideHTML = `
                <div class="carousel-item ${slide.isActive ? 'active' : ''}" data-slide-id="${slide.id}">
                    <video class="d-block w-100" autoplay muted loop playsinline preload="metadata">
                        <source src="${slide.videoSrc}" type="video/mp4" />
                        Your browser does not support the video tag.
                    </video>
                    <div class="carousel-caption d-flex flex-column justify-content-center align-items-start">
                        <h1 class="slide-title">${slide.title}</h1>
                        <p class="slide-subtitle">${slide.subtitle}</p>
                        <a href="#" class="btn btn-explore">
                            ${slide.buttonText}
                            <i class='${slide.buttonIcon}'></i>
                        </a>
                    </div>
                </div>
            `;
            carouselInner.innerHTML += slideHTML;
        });
    }
    generateCarouselHTML();

    // 3. COMBINED SEARCH, SORT, AND VIEW ALL FUNCTIONALITY
    const searchInput = document.getElementById('searchInput');
    const clearSearch = document.getElementById('clearSearch');
    const sortSelect = document.getElementById('sortSelect');
    const statusFilter = document.getElementById('statusFilter');
    const resultsCount = document.getElementById('resultsCount');
    const resetFilters = document.getElementById('resetFilters');
    const noResults = document.getElementById('noResults');
    const viewAllBtn = document.querySelector('.btn-view-all-fixed');

    const INITIAL_DISPLAY_COUNT = 6;
    let isShowingAll = false;

    // Get all event cards - IMPROVED STATUS DETECTION
    let allEvents = Array.from(document.querySelectorAll('.event-card')).map(card => {
        const parentCol = card.closest('.col-lg-4');
        const statusElement = card.querySelector('.event-status');
        const statusText = statusElement ? statusElement.textContent.trim() : '';

        console.log('Event status found:', statusText); // Debug log

        return {
            element: parentCol,
            name: card.querySelector('.event-name')?.textContent?.toLowerCase() || '',
            type: card.querySelector('.event-type')?.textContent?.toLowerCase() || '',
            organizer: card.querySelector('.event-organizer')?.textContent?.toLowerCase() || '',
            status: statusText.toLowerCase(), // Store lowercase for comparison
            statusOriginal: statusText, // Store original for debugging
            date: card.querySelector('.event-date span')?.textContent || '',
            originalOrder: Array.from(parentCol?.parentNode?.children || []).indexOf(parentCol)
        };
    });

    let currentSearchTerm = '';
    let currentSortValue = 'default';
    let currentStatusFilter = 'all';

    console.log('All events with status:', allEvents.map(e => ({ name: e.name, status: e.status, statusOriginal: e.statusOriginal })));

    // Initialize View All Button
    if (viewAllBtn) {
        if (allEvents.length <= INITIAL_DISPLAY_COUNT) {
            viewAllBtn.style.display = 'none';
        } else {
            updateViewAllButton();
        }
    }

    // Search functionality
    if (searchInput) {
        let searchTimeout;
        searchInput.addEventListener('input', function() {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                currentSearchTerm = this.value.toLowerCase().trim();
                updateClearButton();
                applyFiltersAndDisplay();
            }, 300);
        });
    }

    // Clear search
    if (clearSearch) {
        clearSearch.addEventListener('click', function() {
            searchInput.value = '';
            currentSearchTerm = '';
            updateClearButton();
            applyFiltersAndDisplay();
            searchInput.focus();
        });
    }

    // Sort functionality
    if (sortSelect) {
        sortSelect.addEventListener('change', function() {
            currentSortValue = this.value;
            console.log('Sort changed to:', currentSortValue);
            applyFiltersAndDisplay();
        });
    }

    // Status filter - FIXED
    if (statusFilter) {
        statusFilter.addEventListener('change', function() {
            currentStatusFilter = this.value; // Keep original case
            console.log('Status filter changed to:', currentStatusFilter);
            applyFiltersAndDisplay();
        });
    }

    // Reset filters
    if (resetFilters) {
        resetFilters.addEventListener('click', function() {
            searchInput.value = '';
            sortSelect.value = 'default';
            statusFilter.value = 'all';
            currentSearchTerm = '';
            currentSortValue = 'default';
            currentStatusFilter = 'all';
            isShowingAll = false;
            updateClearButton();
            applyFiltersAndDisplay();
        });
    }

    // View All functionality
    if (viewAllBtn) {
        viewAllBtn.addEventListener('click', function(e) {
            e.preventDefault();
            console.log(`View All clicked. Currently showing all: ${isShowingAll}`);

            isShowingAll = !isShowingAll;
            applyFiltersAndDisplay();
        });
    }

    function updateClearButton() {
        if (clearSearch) {
            clearSearch.style.display = currentSearchTerm ? 'block' : 'none';
        }
    }

    function updateViewAllButton() {
        if (!viewAllBtn) return;

        if (!isShowingAll) {
            viewAllBtn.innerHTML = `
                <span>View All (${allEvents.length})</span>
                <i class='bx bx-chevron-down'></i>
            `;
        } else {
            viewAllBtn.innerHTML = `
                <span>Show Less</span>
                <i class='bx bx-chevron-up'></i>
            `;
        }
    }

    function applyFiltersAndDisplay() {
        console.log('Applying filters:', {
            search: currentSearchTerm,
            status: currentStatusFilter,
            sort: currentSortValue
        });

        // Step 1: Filter events based on search and status
        let filteredEvents = allEvents.filter(event => {
            const matchesSearch = !currentSearchTerm ||
                event.name.includes(currentSearchTerm) ||
                event.type.includes(currentSearchTerm) ||
                event.organizer.includes(currentSearchTerm);

            // FIXED STATUS MATCHING - case insensitive comparison
            let matchesStatus = currentStatusFilter === 'all';
            if (!matchesStatus) {
                matchesStatus = event.status.toLowerCase() === currentStatusFilter.toLowerCase();
            }

            console.log(`Event: ${event.name}, Status: "${event.status}", Filter: "${currentStatusFilter}", Match: ${matchesStatus}`);

            return matchesSearch && matchesStatus;
        });

        console.log(`Filtered ${filteredEvents.length} events from ${allEvents.length} total`);

        // Step 2: Sort the filtered events
        if (currentSortValue !== 'default') {
            filteredEvents.sort((a, b) => {
                switch (currentSortValue) {
                    case 'name-asc':
                        return a.name.localeCompare(b.name);
                    case 'name-desc':
                        return b.name.localeCompare(a.name);
                    case 'type-asc':
                        return a.type.localeCompare(b.type);
                    case 'status-asc':
                        return a.status.localeCompare(b.status);
                    case 'date-asc':
                        return a.date.localeCompare(b.date);
                    case 'date-desc':
                        return b.date.localeCompare(a.date);
                    default:
                        return a.originalOrder - b.originalOrder;
                }
            });
        } else {
            // Maintain original order when no sort is applied
            filteredEvents.sort((a, b) => a.originalOrder - b.originalOrder);
        }

        // Step 3: Apply View All logic (only if no active search/filter)
        let eventsToShow = filteredEvents;
        const hasActiveFilters = currentSearchTerm || currentStatusFilter !== 'all';

        if (!hasActiveFilters && !isShowingAll) {
            // Show only first 6 events when no filters and not showing all
            eventsToShow = filteredEvents.slice(0, INITIAL_DISPLAY_COUNT);
        }

        // Step 4: Update display
        updateDisplay(eventsToShow, filteredEvents);

        // Step 5: Update UI elements
        updateResultsInfo(eventsToShow.length, filteredEvents.length);
        updateViewAllButton();

        // Show/hide View All button based on conditions
        if (viewAllBtn) {
            if (hasActiveFilters || filteredEvents.length <= INITIAL_DISPLAY_COUNT) {
                viewAllBtn.style.display = 'none';
            } else {
                viewAllBtn.style.display = 'inline-flex';
            }
        }
    }

    function updateDisplay(eventsToShow, allFilteredEvents) {
        // Hide all events first
        allEvents.forEach(event => {
            if (event.element) {
                event.element.style.display = 'none';
            }
        });

        if (eventsToShow.length > 0) {
            // Show the events that should be displayed
            eventsToShow.forEach(event => {
                if (event.element) {
                    event.element.style.display = 'block';
                }
            });

            if (noResults) {
                noResults.style.display = 'none';
            }
        } else {
            if (noResults) {
                noResults.style.display = 'flex';
            }
        }
    }

    function updateResultsInfo(showingCount, totalFilteredCount) {
        const totalEvents = allEvents.length;
        let message = '';

        const hasActiveFilters = currentSearchTerm || currentStatusFilter !== 'all' || currentSortValue !== 'default';

        if (hasActiveFilters) {
            message = `Showing ${showingCount} of ${totalEvents} events`;
            if (resetFilters) {
                resetFilters.style.display = 'inline-flex';
            }
        } else {
            if (isShowingAll) {
                message = `Showing all ${totalEvents} events`;
            } else {
                message = `Showing ${showingCount} of ${totalEvents} events`;
            }
            if (resetFilters) {
                resetFilters.style.display = 'none';
            }
        }

        if (resultsCount) {
            resultsCount.textContent = message;
        }
    }

    // Initialize the display
    applyFiltersAndDisplay();

    // Keyboard shortcuts
    document.addEventListener('keydown', function(e) {
        if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
            e.preventDefault();
            searchInput?.focus();
        }

        if (e.key === 'Escape' && document.activeElement === searchInput) {
            if (currentSearchTerm) {
                searchInput.value = '';
                currentSearchTerm = '';
                updateClearButton();
                applyFiltersAndDisplay();
            } else {
                searchInput.blur();
            }
        }
    });
});

// Keep your existing delete function
function deleteEvent(eventId) {
    Swal.fire({
        title: 'Delete Event?',
        text: "This action cannot be undone. The event will be permanently deleted.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#FD4BC7',
        cancelButtonColor: '#005AFF',
        confirmButtonText: '<i class="bx bx-trash"></i> Yes, Delete',
        cancelButtonText: '<i class="bx bx-x"></i> Cancel',
        background: '#0B1426',
        color: '#FFFFFF',
        customClass: {
            popup: 'alpine-swal-popup',
            title: 'alpine-swal-title',
            content: 'alpine-swal-content',
            confirmButton: 'alpine-swal-confirm',
            cancelButton: 'alpine-swal-cancel'
        },
        backdrop: 'rgba(11, 20, 38, 0.8)'
    }).then((result) => {
        if (result.isConfirmed) {
            const deleteButton = document.querySelector(`[data-event-id="${eventId}"] .alpine-delete-btn`);
            deleteButton.classList.add('loading');
            deleteButton.innerHTML = '<i class="bx bx-loader-alt bx-spin"></i>';

            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            const formData = new FormData();
            formData.append('__RequestVerificationToken', token);

            fetch(`/Events/Delete/${eventId}`, {
                method: 'POST',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: formData
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    Swal.fire({
                        title: 'Deleted!',
                        text: data.message,
                        icon: 'success',
                        timer: 2000,
                        showConfirmButton: false,
                        background: '#0B1426',
                        color: '#FFFFFF',
                        iconColor: '#FD4BC7'
                    });

                    const eventCard = document.querySelector(`[data-event-id="${eventId}"]`).closest('.col-lg-4');
                    eventCard.style.transition = 'all 0.5s ease';
                    eventCard.style.opacity = '0';
                    eventCard.style.transform = 'scale(0.8) translateY(-20px)';

                    setTimeout(() => {
                        eventCard.remove();
                        const remainingEvents = document.querySelectorAll('.event-card');
                        if (remainingEvents.length === 0) {
                            showNoEventsMessage();
                        }
                    }, 500);
                }
            });
        }
    });
}

function showNoEventsMessage() {
    const eventsContainer = document.querySelector('.events-container');
    eventsContainer.innerHTML = `
        <div class="no-events">
            <div class="no-events-content">
                <i class='bx bx-calendar-x'></i>
                <h3>No Events Available</h3>
                <p>There are currently no events to display. Check back later for exciting events!</p>
                <a href="/Home/AddEvent" class="btn-add-first-event">Add First Event<i class='bx bx-plus'></i></a>
            </div>
        </div>
    `;
}

// User Dropdown Functionality
document.addEventListener('DOMContentLoaded', function () {
    const userDropdownBtn = document.getElementById('userDropdownBtn');
    const userDropdownMenu = document.getElementById('userDropdownMenu');
    const userAvatar = document.querySelector('.user-avatar');

    if (userDropdownBtn && userDropdownMenu) {
        // Toggle dropdown on button click
        userDropdownBtn.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            toggleDropdown();
        });

        // Toggle dropdown on avatar click
        if (userAvatar) {
            userAvatar.addEventListener('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                toggleDropdown();
            });
        }

        // Close dropdown when clicking outside
        document.addEventListener('click', function(e) {
            if (!userDropdownMenu.contains(e.target) &&
                !userDropdownBtn.contains(e.target) &&
                (!userAvatar || !userAvatar.contains(e.target))) {
                closeDropdown();
            }
        });

        // Close dropdown on escape key
        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape') {
                closeDropdown();
            }
        });

        // Prevent dropdown from closing when clicking inside it
        userDropdownMenu.addEventListener('click', function(e) {
            e.stopPropagation();
        });

        function toggleDropdown() {
            if (userDropdownMenu.classList.contains('show')) {
                closeDropdown();
            } else {
                openDropdown();
            }
        }

        function openDropdown() {
            userDropdownMenu.classList.add('show');
            userDropdownBtn.setAttribute('aria-expanded', 'true');

            // Rotate chevron icon
            const chevron = userDropdownBtn.querySelector('.bx-chevron-down');
            if (chevron) {
                chevron.style.transform = 'rotate(180deg)';
            }
        }

        function closeDropdown() {
            userDropdownMenu.classList.remove('show');
            userDropdownBtn.setAttribute('aria-expanded', 'false');

            // Reset chevron icon
            const chevron = userDropdownBtn.querySelector('.bx-chevron-down');
            if (chevron) {
                chevron.style.transform = 'rotate(0deg)';
            }
        }

        // Add smooth transition to chevron
        const chevron = userDropdownBtn.querySelector('.bx-chevron-down');
        if (chevron) {
            chevron.style.transition = 'transform 0.3s ease';
        }
    }

    // Handle logout form submission with confirmation
    const logoutForm = document.querySelector('.logout-form');
    if (logoutForm) {
        logoutForm.addEventListener('submit', function(e) {
            e.preventDefault();

            // Optional: Add confirmation dialog
            if (confirm('Are you sure you want to sign out?')) {
                this.submit();
            }
        });
    }
});

// Add click handlers for dropdown menu items (customize as needed)
document.addEventListener('DOMContentLoaded', function() {
    const dropdownItems = document.querySelectorAll('.dropdown-item:not(.logout-item)');

    dropdownItems.forEach(item => {
        item.addEventListener('click', function(e) {
            e.preventDefault();
            const text = this.querySelector('span').textContent;

            // Handle different menu items
            switch(text) {
                case 'My Profile':
                    // Redirect to profile page or open modal
                    console.log('Navigate to profile');
                    // window.location.href = '/Account/Profile';
                    break;

                case 'My Tickets':
                    // Redirect to tickets page
                    console.log('Navigate to tickets');
                    // window.location.href = '/Tickets/MyTickets';
                    break;

                case 'My Events':
                    // Redirect to user's events
                    console.log('Navigate to my events');
                    // window.location.href = '/Events/MyEvents';
                    break;

                case 'Settings':
                    // Redirect to settings page
                    console.log('Navigate to settings');
                    // window.location.href = '/Account/Settings';
                    break;

                default:
                    console.log('Menu item clicked:', text);
            }

            // Close dropdown after clicking
            const dropdownMenu = document.getElementById('userDropdownMenu');
            if (dropdownMenu) {
                dropdownMenu.classList.remove('show');
            }
        });
    });
});