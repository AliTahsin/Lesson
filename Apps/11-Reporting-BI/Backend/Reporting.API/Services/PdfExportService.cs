using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;

namespace Reporting.API.Services
{
    public class PdfExportService : IPdfExportService
    {
        private readonly IConverter _converter;
        private readonly IReportService _reportService;

        public PdfExportService(IConverter converter, IReportService reportService)
        {
            _converter = converter;
            _reportService = reportService;
        }

        public async Task<byte[]> ExportReportAsync(ReportRequestDto request)
        {
            var html = await GenerateHtmlReportAsync(request);
            
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
                },
                Objects = {
                    new ObjectSettings {
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                        FooterSettings = { FontSize = 9, Right = "Generated: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") }
                    }
                }
            };
            
            return _converter.Convert(doc);
        }

        private async Task<string> GenerateHtmlReportAsync(ReportRequestDto request)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("<html><head><meta charset='utf-8'><style>");
            sb.AppendLine(@"
                body { font-family: Arial, sans-serif; margin: 20px; }
                h1 { color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 10px; }
                h2 { color: #34495e; margin-top: 20px; }
                table { width: 100%; border-collapse: collapse; margin-top: 10px; }
                th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
                th { background-color: #3498db; color: white; }
                .summary { background-color: #ecf0f1; padding: 15px; border-radius: 5px; margin: 20px 0; }
                .footer { text-align: center; margin-top: 30px; font-size: 10px; color: #7f8c8d; }
            ");
            sb.AppendLine("</style></head><body>");
            
            switch (request.ReportType)
            {
                case "Revenue":
                    await AppendRevenueReportHtml(sb, request);
                    break;
                case "Occupancy":
                    await AppendOccupancyReportHtml(sb, request);
                    break;
                case "Reservation":
                    await AppendReservationReportHtml(sb, request);
                    break;
                case "Customer":
                    await AppendCustomerReportHtml(sb, request);
                    break;
                case "Channel":
                    await AppendChannelReportHtml(sb, request);
                    break;
            }
            
            sb.AppendLine("<div class='footer'>This report was generated automatically by Hotel Management System</div>");
            sb.AppendLine("</body></html>");
            
            return sb.ToString();
        }

        private async Task AppendRevenueReportHtml(StringBuilder sb, ReportRequestDto request)
        {
            var data = await _reportService.GetRevenueReportAsync(request.HotelId, request.StartDate, request.EndDate);
            
            sb.AppendLine($"<h1>Revenue Report - {data.HotelName}</h1>");
            sb.AppendLine($"<p>Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}</p>");
            sb.AppendLine("<div class='summary'>");
            sb.AppendLine("<h2>Summary</h2>");
            sb.AppendLine($"<p><strong>Total Revenue:</strong> €{data.TotalRevenue:N2}</p>");
            sb.AppendLine($"<p><strong>Room Revenue:</strong> €{data.TotalRoomRevenue:N2}</p>");
            sb.AppendLine($"<p><strong>F&B Revenue:</strong> €{data.TotalFBRevenue:N2}</p>");
            sb.AppendLine($"<p><strong>ADR:</strong> €{data.AverageDailyRate:N2}</p>");
            sb.AppendLine($"<p><strong>RevPAR:</strong> €{data.RevPAR:N2}</p>");
            sb.AppendLine("</div>");
            
            sb.AppendLine("<h2>Daily Breakdown</h2>");
            sb.AppendLine("<table><tr><th>Date</th><th>Revenue</th><th>Room Revenue</th><th>F&B Revenue</th><th>Other Revenue</th></tr>");
            foreach (var daily in data.DailyData)
            {
                sb.AppendLine($"<tr><td>{daily.Date:dd/MM/yyyy}</td><td>€{daily.Revenue:N2}</td><td>€{daily.RoomRevenue:N2}</td><td>€{daily.FBRevenue:N2}</td><td>€{daily.OtherRevenue:N2}</td></tr>");
            }
            sb.AppendLine("</table>");
        }

        private async Task AppendOccupancyReportHtml(StringBuilder sb, ReportRequestDto request)
        {
            var data = await _reportService.GetOccupancyReportAsync(request.HotelId, request.StartDate, request.EndDate);
            
            sb.AppendLine($"<h1>Occupancy Report - {data.HotelName}</h1>");
            sb.AppendLine($"<p>Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}</p>");
            sb.AppendLine("<div class='summary'>");
            sb.AppendLine("<h2>Summary</h2>");
            sb.AppendLine($"<p><strong>Average Occupancy Rate:</strong> {data.AverageOccupancyRate:F2}%</p>");
            sb.AppendLine($"<p><strong>Total Available Rooms:</strong> {data.TotalAvailableRooms}</p>");
            sb.AppendLine($"<p><strong>Total Sold Rooms:</strong> {data.TotalSoldRooms}</p>");
            sb.AppendLine("</div>");
            
            sb.AppendLine("<h2>Daily Breakdown</h2>");
            sb.AppendLine("<table><tr><th>Date</th><th>Occupancy Rate</th><th>Available Rooms</th><th>Sold Rooms</th><th>Average Price</th></tr>");
            foreach (var daily in data.OccupancyData)
            {
                sb.AppendLine($"<tr><td>{daily.Date:dd/MM/yyyy}</td><td>{daily.OccupancyRate:F1}%</td><td>{daily.AvailableRooms}</td><td>{daily.SoldRooms}</td><td>€{daily.AveragePrice:N2}</td></tr>");
            }
            sb.AppendLine("</table>");
        }

        private async Task AppendReservationReportHtml(StringBuilder sb, ReportRequestDto request)
        {
            var data = await _reportService.GetReservationReportAsync(request.HotelId, request.StartDate, request.EndDate);
            
            sb.AppendLine($"<h1>Reservation Report - {data.HotelName}</h1>");
            sb.AppendLine($"<p>Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}</p>");
            sb.AppendLine("<div class='summary'>");
            sb.AppendLine("<h2>Summary</h2>");
            sb.AppendLine($"<p><strong>Total Reservations:</strong> {data.TotalReservations}</p>");
            sb.AppendLine($"<p><strong>Confirmed Reservations:</strong> {data.ConfirmedReservations}</p>");
            sb.AppendLine($"<p><strong>Cancelled Reservations:</strong> {data.CancelledReservations}</p>");
            sb.AppendLine($"<p><strong>Cancellation Rate:</strong> {data.CancellationRate:F2}%</p>");
            sb.AppendLine("</div>");
            
            sb.AppendLine("<h2>Daily Breakdown</h2>");
            sb.AppendLine("<table><tr><th>Date</th><th>Total</th><th>Confirmed</th><th>Cancelled</th><th>No Show</th><th>Avg Lead Days</th></tr>");
            foreach (var daily in data.ReservationData)
            {
                sb.AppendLine($"<tr><td>{daily.Date:dd/MM/yyyy}</td><td>{daily.TotalReservations}</td><td>{daily.ConfirmedReservations}</td><td>{daily.CancelledReservations}</td><td>{daily.NoShowReservations}</td><td>{daily.AverageLeadDays}</td></tr>");
            }
            sb.AppendLine("</table>");
        }

        private async Task AppendCustomerReportHtml(StringBuilder sb, ReportRequestDto request)
        {
            var data = await _reportService.GetCustomerReportAsync(request.HotelId, request.StartDate, request.EndDate);
            
            sb.AppendLine($"<h1>Customer Report - {data.HotelName}</h1>");
            sb.AppendLine($"<p>Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}</p>");
            sb.AppendLine("<div class='summary'>");
            sb.AppendLine("<h2>Summary</h2>");
            sb.AppendLine($"<p><strong>Total Customers:</strong> {data.TotalCustomers}</p>");
            sb.AppendLine($"<p><strong>New Customers:</strong> {data.NewCustomers}</p>");
            sb.AppendLine($"<p><strong>Repeat Customers:</strong> {data.RepeatCustomers}</p>");
            sb.AppendLine($"<p><strong>Customer Satisfaction Score:</strong> {data.CustomerSatisfactionScore:F1}%</p>");
            sb.AppendLine("</div>");
            
            sb.AppendLine("<h2>Top 10 Customers</h2>");
            sb.AppendLine("<table><tr><th>Customer Name</th><th>Total Stays</th><th>Total Spent</th><th>Average per Stay</th></tr>");
            foreach (var customer in data.TopCustomers)
            {
                sb.AppendLine($"<tr><td>{customer.CustomerName}</td><td>{customer.TotalStays}</td><td>€{customer.TotalSpent:N2}</td><td>€{customer.AverageSpentPerStay:N2}</td></tr>");
            }
            sb.AppendLine("</table>");
        }

        private async Task AppendChannelReportHtml(StringBuilder sb, ReportRequestDto request)
        {
            var data = await _reportService.GetChannelReportAsync(request.HotelId, request.StartDate, request.EndDate);
            
            sb.AppendLine($"<h1>Channel Report - {data.HotelName}</h1>");
            sb.AppendLine($"<p>Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}</p>");
            sb.AppendLine("<div class='summary'>");
            sb.AppendLine("<h2>Summary</h2>");
            sb.AppendLine($"<p><strong>Total Bookings:</strong> {data.TotalBookings}</p>");
            sb.AppendLine($"<p><strong>Total Revenue:</strong> €{data.TotalRevenue:N2}</p>");
            sb.AppendLine($"<p><strong>Total Commission:</strong> €{data.TotalCommission:N2}</p>");
            sb.AppendLine($"<p><strong>Net Revenue:</strong> €{data.NetRevenue:N2}</p>");
            sb.AppendLine("</div>");
            
            sb.AppendLine("<h2>Channel Performance</h2>");
            sb.AppendLine("<table><tr><th>Channel</th><th>Bookings</th><th>Revenue</th><th>Commission</th><th>Net Revenue</th></tr>");
            foreach (var channel in data.ChannelData)
            {
                sb.AppendLine($"<tr><td>{channel.ChannelName}</td><td>{channel.Bookings}</td><td>€{channel.Revenue:N2}</td><td>€{channel.Commission:N2}</td><td>€{channel.Revenue - channel.Commission:N2}</td></tr>");
            }
            sb.AppendLine("</table>");
        }
    }
}