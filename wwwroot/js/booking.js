// Booking Page JavaScript

document.addEventListener('DOMContentLoaded', function () {
    initializeSeatSelection();
    updateBookingSummary();
});

let selectedSeats = [];
let totalAmount = 0;

function initializeSeatSelection() {
    const seats = document.querySelectorAll('.seat');

    seats.forEach(seat => {
        seat.addEventListener('click', function () {
            toggleSeatSelection(this);
        });
    });
}

function toggleSeatSelection(seatElement) {
    const seatId = seatElement.getAttribute('data-seat');
    const seatPrice = parseInt(seatElement.getAttribute('data-price'));

    if (seatElement.classList.contains('occupied')) {
        return; // Can't select occupied seats
    }

    if (seatElement.classList.contains('selected')) {
        // Deselect seat
        seatElement.classList.remove('selected');
        selectedSeats = selectedSeats.filter(seat => seat.id !== seatId);
    } else {
        // Select seat
        seatElement.classList.add('selected');
        selectedSeats.push({
            id: seatId,
            price: seatPrice
        });
    }

    updateBookingSummary();
}

function updateBookingSummary() {
    const selectedSeatsContainer = document.getElementById('selectedSeatsContainer');
    const selectedSeatsList = document.getElementById('selectedSeatsList');
    const bookingSummary = document.getElementById('bookingSummary');
    const summaryItems = document.getElementById('summaryItems');
    const totalPrice = document.getElementById('totalPrice');
    const proceedBtn = document.getElementById('proceedBtn');

    // Clear previous content
    selectedSeatsList.innerHTML = '';
    summaryItems.innerHTML = '';

    if (selectedSeats.length > 0) {
        selectedSeatsContainer.style.display = 'block';
        bookingSummary.style.display = 'block';

        totalAmount = 0;

        selectedSeats.forEach(seat => {
            // Add to selected seats list
            const seatItem = document.createElement('div');
            seatItem.className = 'selected-seat-item';
            seatItem.innerHTML = `
                <span class="seat-id">${seat.id}</span>
                <span class="seat-price">LKR ${seat.price.toLocaleString()}</span>
            `;
            selectedSeatsList.appendChild(seatItem);

            // Add to summary
            const summaryItem = document.createElement('div');
            summaryItem.className = 'summary-item';
            summaryItem.innerHTML = `
                <span>Seat ${seat.id}</span>
                <span>LKR ${seat.price.toLocaleString()}</span>
            `;
            summaryItems.appendChild(summaryItem);

            totalAmount += seat.price;
        });

        totalPrice.textContent = `LKR ${totalAmount.toLocaleString()}`;
        proceedBtn.disabled = false;

        // Update hidden inputs
        document.getElementById('selectedSeatsInput').value = selectedSeats.map(s => s.id).join(',');
        document.getElementById('totalAmountInput').value = totalAmount;
    } else {
        selectedSeatsContainer.style.display = 'none';
        bookingSummary.style.display = 'none';
        proceedBtn.disabled = true;
    }
}

// Form submission
document.getElementById('bookingForm').addEventListener('submit', function (e) {
    if (selectedSeats.length === 0) {
        e.preventDefault();
        alert('Please select at least one seat before proceeding.');
        return;
    }

    // Add loading state
    const proceedBtn = document.getElementById('proceedBtn');
    const originalText = proceedBtn.innerHTML;
    proceedBtn.innerHTML = '<i class="bi bi-hourglass-split"></i> Processing...';
    proceedBtn.disabled = true;

    // Store booking data in session storage
    sessionStorage.setItem('selectedSeats', JSON.stringify(selectedSeats));
    sessionStorage.setItem('totalAmount', totalAmount);

    // Allow form to submit normally to navigate to payment page
});
