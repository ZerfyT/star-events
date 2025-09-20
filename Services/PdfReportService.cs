using iTextSharp.text;
using iTextSharp.text.pdf;
using star_events.Models;

namespace star_events.Services;

public class PdfReportService : IPdfReportService
{
    private readonly Font _boldFont = new(Font.FontFamily.HELVETICA, 10, Font.BOLD);
    private readonly Font _headerFont = new(Font.FontFamily.HELVETICA, 18, Font.BOLD);
    private readonly Font _normalFont = new(Font.FontFamily.HELVETICA, 10, Font.NORMAL);
    private readonly Font _smallFont = new(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.GRAY);
    private readonly Font _subtitleFont = new(Font.FontFamily.HELVETICA, 14, Font.NORMAL, BaseColor.GRAY);
    private readonly Font _titleFont = new(Font.FontFamily.HELVETICA, 24, Font.BOLD, BaseColor.BLUE);

    public byte[] GenerateRevenueReportPdf(List<Payment> payments, decimal totalRevenue,
        List<PaymentMethodSummary> paymentMethods, DateTime startDate, DateTime endDate,
        string? eventTitle = null)
    {
        using var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4, 50, 50, 50, 50);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        // header
        AddHeader(document, "Revenue Report", startDate, endDate, eventTitle);

        // summary section
        AddSummarySection(document, "Revenue Summary", new[]
        {
            ("Total Revenue", $"${totalRevenue:N2}"),
            ("Total Payments", payments.Count.ToString()),
            ("Average Payment", payments.Count > 0 ? $"${totalRevenue / payments.Count:N2}" : "$0.00"),
            ("Report Period", $"{startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}")
        });

        // payment methods breakdown
        if (paymentMethods.Any()) AddPaymentMethodsSection(document, paymentMethods, totalRevenue);

        // payments table
        AddPaymentsTable(document, payments);

        // footer
        AddFooter(document);

        document.Close();
        return memoryStream.ToArray();
    }

    public byte[] GenerateEventsReportPdf(List<Event> events, int totalEvents, int activeEvents,
        int completedEvents, int cancelledEvents, DateTime startDate, DateTime endDate)
    {
        using var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4, 50, 50, 50, 50);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        // header
        AddHeader(document, "Events Report", startDate, endDate);

        // summary section
        AddSummarySection(document, "Events Summary", new[]
        {
            ("Total Events", totalEvents.ToString()),
            ("Active Events", activeEvents.ToString()),
            ("Completed Events", completedEvents.ToString()),
            ("Cancelled Events", cancelledEvents.ToString()),
            ("Report Period", $"{startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}")
        });

        // events table
        AddEventsTable(document, events);

        // footer
        AddFooter(document);

        document.Close();
        return memoryStream.ToArray();
    }

    public byte[] GenerateUsersReportPdf(List<ApplicationUser> users, int totalUsers,
        int adminUsers, int eventOrganizerUsers, int customerUsers,
        DateTime startDate, DateTime endDate, string? selectedRole = null)
    {
        using var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4, 50, 50, 50, 50);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        // header
        var title = selectedRole != null ? $"Users Report - {selectedRole}" : "Users Report";
        AddHeader(document, title, startDate, endDate);

        // summary section
        AddSummarySection(document, "Users Summary", new[]
        {
            ("Total Users", totalUsers.ToString()),
            ("Admin Users", adminUsers.ToString()),
            ("Event Organizers", eventOrganizerUsers.ToString()),
            ("Customers", customerUsers.ToString()),
            ("Report Period", $"{startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}")
        });

        // users table
        AddUsersTable(document, users);

        // footer
        AddFooter(document);

        document.Close();
        return memoryStream.ToArray();
    }

    public byte[] GenerateTicketSalesReportPdf(List<Ticket> tickets, int totalTickets,
        decimal totalRevenue, int scannedTickets, int unscannedTickets,
        DateTime startDate, DateTime endDate, string? eventTitle = null)
    {
        using var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4, 50, 50, 50, 50);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        // header
        AddHeader(document, "Ticket Sales Report", startDate, endDate, eventTitle);

        // summary section
        AddSummarySection(document, "Ticket Sales Summary", new[]
        {
            ("Total Tickets Sold", totalTickets.ToString()),
            ("Total Revenue", $"${totalRevenue:N2}"),
            ("Scanned Tickets", scannedTickets.ToString()),
            ("Unscanned Tickets", unscannedTickets.ToString()),
            ("Scan Rate", totalTickets > 0 ? $"{scannedTickets * 100.0 / totalTickets:F1}%" : "0%"),
            ("Report Period", $"{startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}")
        });

        // tickets table
        AddTicketsTable(document, tickets);

        // footer
        AddFooter(document);

        document.Close();
        return memoryStream.ToArray();
    }

    private void AddHeader(Document document, string title, DateTime startDate, DateTime endDate,
        string? subtitle = null)
    {
        // header
        var companyParagraph = new Paragraph("Star Events", _titleFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 10
        };
        document.Add(companyParagraph);

        var titleParagraph = new Paragraph(title, _headerFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 5
        };
        document.Add(titleParagraph);

        if (!string.IsNullOrEmpty(subtitle))
        {
            var subtitleParagraph = new Paragraph(subtitle, _subtitleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 10
            };
            document.Add(subtitleParagraph);
        }

        var dateParagraph = new Paragraph($"Generated on: {DateTime.Now:MMMM dd, yyyy 'at' HH:mm}", _smallFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 20
        };
        document.Add(dateParagraph);
    }

    private void AddSummarySection(Document document, string sectionTitle, (string Label, string Value)[] summaryData)
    {
        var titleParagraph = new Paragraph(sectionTitle, _boldFont)
        {
            SpacingBefore = 20,
            SpacingAfter = 10
        };
        document.Add(titleParagraph);

        var table = new PdfPTable(2) { WidthPercentage = 100 };
        table.SetWidths(new float[] { 1, 1 });

        foreach (var (label, value) in summaryData)
        {
            var labelCell = new PdfPCell(new Phrase(label, _boldFont))
            {
                BackgroundColor = BaseColor.LIGHT_GRAY,
                Padding = 8,
                Border = Rectangle.BOX
            };
            table.AddCell(labelCell);

            var valueCell = new PdfPCell(new Phrase(value, _normalFont))
            {
                Padding = 8,
                Border = Rectangle.BOX
            };
            table.AddCell(valueCell);
        }

        document.Add(table);
    }

    private void AddPaymentMethodsSection(Document document, List<PaymentMethodSummary> paymentMethods,
        decimal totalRevenue)
    {
        var titleParagraph = new Paragraph("Payment Methods Breakdown", _boldFont)
        {
            SpacingBefore = 15,
            SpacingAfter = 10
        };
        document.Add(titleParagraph);

        var table = new PdfPTable(4) { WidthPercentage = 100 };
        table.SetWidths(new float[] { 1, 1, 1, 1 });

        // Header row
        table.AddCell(new PdfPCell(new Phrase("Payment Method", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Count", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Amount", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Percentage", _boldFont)) { Border = Rectangle.BOX });

        foreach (var method in paymentMethods)
        {
            table.AddCell(new PdfPCell(new Phrase(method.Method, _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(method.Count.ToString(), _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase($"${method.TotalAmount:N2}", _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase($"{method.TotalAmount / totalRevenue * 100:F1}%", _normalFont))
                { Border = Rectangle.BOX });
        }

        document.Add(table);
    }

    private void AddPaymentsTable(Document document, List<Payment> payments)
    {
        var titleParagraph = new Paragraph("Payment Details", _boldFont)
        {
            SpacingBefore = 20,
            SpacingAfter = 10
        };
        document.Add(titleParagraph);

        var table = new PdfPTable(6) { WidthPercentage = 100 };
        table.SetWidths(new float[] { 1, 1, 1, 1, 1, 1 });

        // Header row
        table.AddCell(new PdfPCell(new Phrase("Payment ID", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Amount", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Method", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Status", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Date", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Transaction ID", _boldFont)) { Border = Rectangle.BOX });

        foreach (var payment in payments.Take(100))
        {
            table.AddCell(new PdfPCell(new Phrase($"#{payment.PaymentID}", _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase($"${payment.AmountPaid:N2}", _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(payment.PaymentMethod, _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(payment.PaymentStatus, _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(payment.PaymentDateTime.ToString("MMM dd, yyyy"), _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(payment.PaymentGatewayTransactionID, _normalFont))
                { Border = Rectangle.BOX });
        }

        if (payments.Count > 100)
        {
            var cell = new PdfPCell(new Phrase($"... and {payments.Count - 100} more payments", _smallFont))
            {
                Colspan = 6,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = Rectangle.BOX
            };
            table.AddCell(cell);
        }

        document.Add(table);
    }

    private void AddEventsTable(Document document, List<Event> events)
    {
        var titleParagraph = new Paragraph("Event Details", _boldFont)
        {
            SpacingBefore = 20,
            SpacingAfter = 10
        };
        document.Add(titleParagraph);

        var table = new PdfPTable(6) { WidthPercentage = 100 };
        table.SetWidths(new float[] { 1, 2, 1, 1, 1, 1 });

        // Header row
        table.AddCell(new PdfPCell(new Phrase("Event ID", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Title", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Start Date", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("End Date", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Status", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Organizer", _boldFont)) { Border = Rectangle.BOX });

        foreach (var evt in events.Take(50))
        {
            table.AddCell(new PdfPCell(new Phrase($"#{evt.EventID}", _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(evt.Title, _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(evt.StartDateTime.ToString("MMM dd, yyyy"), _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(evt.EndDateTime.ToString("MMM dd, yyyy"), _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(evt.Status, _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(evt.OrganizerName, _normalFont)) { Border = Rectangle.BOX });
        }

        if (events.Count > 50)
        {
            var cell = new PdfPCell(new Phrase($"... and {events.Count - 50} more events", _smallFont))
            {
                Colspan = 6,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = Rectangle.BOX
            };
            table.AddCell(cell);
        }

        document.Add(table);
    }

    private void AddUsersTable(Document document, List<ApplicationUser> users)
    {
        var titleParagraph = new Paragraph("User Details", _boldFont)
        {
            SpacingBefore = 20,
            SpacingAfter = 10
        };
        document.Add(titleParagraph);

        var table = new PdfPTable(4) { WidthPercentage = 100 };
        table.SetWidths(new float[] { 1, 2, 1, 1 });

        // Header row
        table.AddCell(new PdfPCell(new Phrase("User ID", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Email", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Name", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Contact", _boldFont)) { Border = Rectangle.BOX });

        foreach (var user in users.Take(100)) // Limit to 100 rows for performance
        {
            table.AddCell(new PdfPCell(new Phrase(user.Id.Substring(0, 8) + "...", _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(user.Email ?? "N/A", _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase($"{user.FirstName} {user.LastName}", _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(user.ContactNo ?? "N/A", _normalFont)) { Border = Rectangle.BOX });
        }

        if (users.Count > 100)
        {
            var cell = new PdfPCell(new Phrase($"... and {users.Count - 100} more users", _smallFont))
            {
                Colspan = 4,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = Rectangle.BOX
            };
            table.AddCell(cell);
        }

        document.Add(table);
    }

    private void AddTicketsTable(Document document, List<Ticket> tickets)
    {
        var titleParagraph = new Paragraph("Ticket Details", _boldFont)
        {
            SpacingBefore = 20,
            SpacingAfter = 10
        };
        document.Add(titleParagraph);

        var table = new PdfPTable(7) { WidthPercentage = 100 };
        table.SetWidths(new float[] { 1, 1, 1, 2, 1, 1, 2 });

        // Header row
        table.AddCell(new PdfPCell(new Phrase("Ticket ID", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Type", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Price", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Event", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Booking Date", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Status", _boldFont)) { Border = Rectangle.BOX });
        table.AddCell(new PdfPCell(new Phrase("Customer", _boldFont)) { Border = Rectangle.BOX });

        foreach (var ticket in tickets.Take(100))
        {
            table.AddCell(new PdfPCell(new Phrase($"#{ticket.TicketID}", _normalFont)) { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(ticket.TicketType?.Name ?? "N/A", _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase($"${ticket.TicketType?.Price ?? 0:N2}", _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(ticket.TicketType?.Event?.Title ?? "N/A", _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(
                new PdfPCell(new Phrase(ticket.Booking?.BookingDateTime.ToString("MMM dd, yyyy") ?? "N/A", _normalFont))
                    { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(ticket.IsScanned ? "Scanned" : "Unscanned", _normalFont))
                { Border = Rectangle.BOX });
            table.AddCell(new PdfPCell(new Phrase(ticket.Booking?.User?.Email ?? "N/A", _normalFont))
                { Border = Rectangle.BOX });
        }

        if (tickets.Count > 100)
        {
            var cell = new PdfPCell(new Phrase($"... and {tickets.Count - 100} more tickets", _smallFont))
            {
                Colspan = 7,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = Rectangle.BOX
            };
            table.AddCell(cell);
        }

        document.Add(table);
    }

    private void AddFooter(Document document)
    {
        var footerParagraph =
            new Paragraph($"Report generated by Star Events System on {DateTime.Now:MMMM dd, yyyy 'at' HH:mm}",
                _smallFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingBefore = 30
            };
        document.Add(footerParagraph);
    }
}