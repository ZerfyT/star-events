// Event Detail Page JavaScript

document.addEventListener('DOMContentLoaded', function () {
    initializeTicketSelection();
    updateTicketSummary();
});

function initializeTicketSelection() {
    const quantityInputs = document.querySelectorAll('input[name$="Quantity"]');
    quantityInputs.forEach(input => {
        input.addEventListener('change', updateTicketSummary);
    });
}

function increaseQuantity(ticketType) {
    const input = document.getElementById(`${ticketType}-qty`);
    const currentValue = parseInt(input.value) || 0;
    const maxValue = parseInt(input.getAttribute('max')) || 8;

    if (currentValue < maxValue) {
        input.value = currentValue + 1;
        updateTicketSummary();
    }
}

function decreaseQuantity(ticketType) {
    const input = document.getElementById(`${ticketType}-qty`);
    const currentValue = parseInt(input.value) || 0;
    const minValue = parseInt(input.getAttribute('min')) || 0;

    if (currentValue > minValue) {
        input.value = currentValue - 1;
        updateTicketSummary();
    }
}

function updateTicketSummary() {
    const ticketTypes = [
        {type: 'vip', name: 'VIP', price: 5000},
        {type: 'standard', name: 'Standard', price: 4000},
        {type: 'economy', name: 'Economy', price: 3000}
    ];

    let totalAmount = 0;
    let hasSelectedTickets = false;
    const summaryItems = document.getElementById('summaryItems');
    const totalPrice = document.getElementById('totalPrice');
    const ticketSummary = document.getElementById('ticketSummary');
    const selectSeatsBtn = document.getElementById('selectSeatsBtn');

    summaryItems.innerHTML = '';

    ticketTypes.forEach(ticket => {
        const quantity = parseInt(document.getElementById(`${ticket.type}-qty`).value) || 0;

        if (quantity > 0) {
            hasSelectedTickets = true;
            const subtotal = quantity * ticket.price;
            totalAmount += subtotal;

            const summaryItem = document.createElement('div');
            summaryItem.className = 'summary-item';
            summaryItem.innerHTML = `
                <span>${ticket.name} x ${quantity}</span>
                <span>LKR ${subtotal.toLocaleString()}</span>
            `;
            summaryItems.appendChild(summaryItem);
        }
    });

    totalPrice.textContent = `LKR ${totalAmount.toLocaleString()}`;

    if (hasSelectedTickets) {
        ticketSummary.style.display = 'block';
        selectSeatsBtn.disabled = false;
    } else {
        ticketSummary.style.display = 'none';
        selectSeatsBtn.disabled = true;
    }
}