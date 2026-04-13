using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Reporting.API.Services
{
    public class ExcelExportService : IExcelExportService
    {
        private readonly IReportService _reportService;

        public ExcelExportService(IReportService reportService)
        {
            _reportService = reportService;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<byte[]> ExportReportAsync(ReportRequestDto request)
        {
            using var package = new ExcelPackage();
            
            switch (request.ReportType)
            {
                case "Revenue":
                    await CreateRevenueReport(package, request);
                    break;
                case "Occupancy":
                    await CreateOccupancyReport(package, request);
                    break;
                case "Reservation":
                    await CreateReservationReport(package, request);
                    break;
                case "Customer":
                    await CreateCustomerReport(package, request);
                    break;
                case "Channel":
                    await CreateChannelReport(package, request);
                    break;
            }
            
            return package.GetAsByteArray();
        }

        private async Task CreateRevenueReport(ExcelPackage package, ReportRequestDto request)
        {
            var data = await _reportService.GetRevenueReportAsync(request.HotelId, request.StartDate, request.EndDate);
            var worksheet = package.Workbook.Worksheets.Add("Revenue Report");
            
            // Title
            worksheet.Cells["A1"].Value = $"Revenue Report - {data.HotelName}";
            worksheet.Cells["A1:A2"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            
            // Period
            worksheet.Cells["A3"].Value = $"Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}";
            worksheet.Cells["A3"].Style.Font.Bold = true;
            
            // Summary
            worksheet.Cells["A5"].Value = "SUMMARY";
            worksheet.Cells["A5"].Style.Font.Bold = true;
            
            worksheet.Cells["A6"].Value = "Total Revenue";
            worksheet.Cells["B6"].Value = data.TotalRevenue;
            worksheet.Cells["B6"].Style.Numberformat.Format = "#,##0.00";
            
            worksheet.Cells["A7"].Value = "Room Revenue";
            worksheet.Cells["B7"].Value = data.TotalRoomRevenue;
            worksheet.Cells["B7"].Style.Numberformat.Format = "#,##0.00";
            
            worksheet.Cells["A8"].Value = "F&B Revenue";
            worksheet.Cells["B8"].Value = data.TotalFBRevenue;
            worksheet.Cells["B8"].Style.Numberformat.Format = "#,##0.00";
            
            worksheet.Cells["A9"].Value = "ADR (Average Daily Rate)";
            worksheet.Cells["B9"].Value = data.AverageDailyRate;
            worksheet.Cells["B9"].Style.Numberformat.Format = "#,##0.00";
            
            worksheet.Cells["A10"].Value = "RevPAR";
            worksheet.Cells["B10"].Value = data.RevPAR;
            worksheet.Cells["B10"].Style.Numberformat.Format = "#,##0.00";
            
            // Daily Data Table
            worksheet.Cells["A12"].Value = "DAILY BREAKDOWN";
            worksheet.Cells["A12"].Style.Font.Bold = true;
            
            var headers = new[] { "Date", "Revenue", "Room Revenue", "F&B Revenue", "Other Revenue" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[13, i + 1].Value = headers[i];
                worksheet.Cells[13, i + 1].Style.Font.Bold = true;
                worksheet.Cells[13, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[13, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }
            
            int row = 14;
            foreach (var daily in data.DailyData)
            {
                worksheet.Cells[row, 1].Value = daily.Date.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 2].Value = daily.Revenue;
                worksheet.Cells[row, 3].Value = daily.RoomRevenue;
                worksheet.Cells[row, 4].Value = daily.FBRevenue;
                worksheet.Cells[row, 5].Value = daily.OtherRevenue;
                worksheet.Cells[row, 2, row, 5].Style.Numberformat.Format = "#,##0.00";
                row++;
            }
            
            worksheet.Cells.AutoFitColumns();
        }

        private async Task CreateOccupancyReport(ExcelPackage package, ReportRequestDto request)
        {
            var data = await _reportService.GetOccupancyReportAsync(request.HotelId, request.StartDate, request.EndDate);
            var worksheet = package.Workbook.Worksheets.Add("Occupancy Report");
            
            worksheet.Cells["A1"].Value = $"Occupancy Report - {data.HotelName}";
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            
            worksheet.Cells["A3"].Value = $"Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}";
            
            worksheet.Cells["A5"].Value = "SUMMARY";
            worksheet.Cells["A5"].Style.Font.Bold = true;
            
            worksheet.Cells["A6"].Value = "Average Occupancy Rate";
            worksheet.Cells["B6"].Value = data.AverageOccupancyRate;
            worksheet.Cells["B6"].Style.Numberformat.Format = "#0.00%";
            
            worksheet.Cells["A7"].Value = "Total Available Rooms";
            worksheet.Cells["B7"].Value = data.TotalAvailableRooms;
            
            worksheet.Cells["A8"].Value = "Total Sold Rooms";
            worksheet.Cells["B8"].Value = data.TotalSoldRooms;
            
            var headers = new[] { "Date", "Occupancy Rate", "Available Rooms", "Sold Rooms", "Average Price" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[10, i + 1].Value = headers[i];
                worksheet.Cells[10, i + 1].Style.Font.Bold = true;
            }
            
            int row = 11;
            foreach (var daily in data.OccupancyData)
            {
                worksheet.Cells[row, 1].Value = daily.Date.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 2].Value = daily.OccupancyRate / 100;
                worksheet.Cells[row, 3].Value = daily.AvailableRooms;
                worksheet.Cells[row, 4].Value = daily.SoldRooms;
                worksheet.Cells[row, 5].Value = daily.AveragePrice;
                worksheet.Cells[row, 2].Style.Numberformat.Format = "0.00%";
                worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                row++;
            }
            
            worksheet.Cells.AutoFitColumns();
        }

        private async Task CreateReservationReport(ExcelPackage package, ReportRequestDto request)
        {
            var data = await _reportService.GetReservationReportAsync(request.HotelId, request.StartDate, request.EndDate);
            var worksheet = package.Workbook.Worksheets.Add("Reservation Report");
            
            worksheet.Cells["A1"].Value = $"Reservation Report - {data.HotelName}";
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            
            worksheet.Cells["A3"].Value = $"Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}";
            
            worksheet.Cells["A5"].Value = "SUMMARY";
            worksheet.Cells["A5"].Style.Font.Bold = true;
            
            worksheet.Cells["A6"].Value = "Total Reservations";
            worksheet.Cells["B6"].Value = data.TotalReservations;
            
            worksheet.Cells["A7"].Value = "Confirmed Reservations";
            worksheet.Cells["B7"].Value = data.ConfirmedReservations;
            
            worksheet.Cells["A8"].Value = "Cancelled Reservations";
            worksheet.Cells["B8"].Value = data.CancelledReservations;
            
            worksheet.Cells["A9"].Value = "Cancellation Rate";
            worksheet.Cells["B9"].Value = data.CancellationRate / 100;
            worksheet.Cells["B9"].Style.Numberformat.Format = "0.00%";
            
            var headers = new[] { "Date", "Total", "Confirmed", "Cancelled", "No Show", "Avg Lead Days" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[11, i + 1].Value = headers[i];
                worksheet.Cells[11, i + 1].Style.Font.Bold = true;
            }
            
            int row = 12;
            foreach (var daily in data.ReservationData)
            {
                worksheet.Cells[row, 1].Value = daily.Date.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 2].Value = daily.TotalReservations;
                worksheet.Cells[row, 3].Value = daily.ConfirmedReservations;
                worksheet.Cells[row, 4].Value = daily.CancelledReservations;
                worksheet.Cells[row, 5].Value = daily.NoShowReservations;
                worksheet.Cells[row, 6].Value = daily.AverageLeadDays;
                row++;
            }
            
            worksheet.Cells.AutoFitColumns();
        }

        private async Task CreateCustomerReport(ExcelPackage package, ReportRequestDto request)
        {
            var data = await _reportService.GetCustomerReportAsync(request.HotelId, request.StartDate, request.EndDate);
            var worksheet = package.Workbook.Worksheets.Add("Customer Report");
            
            worksheet.Cells["A1"].Value = $"Customer Report - {data.HotelName}";
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            
            worksheet.Cells["A3"].Value = $"Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}";
            
            worksheet.Cells["A5"].Value = "SUMMARY";
            worksheet.Cells["A5"].Style.Font.Bold = true;
            
            worksheet.Cells["A6"].Value = "Total Customers";
            worksheet.Cells["B6"].Value = data.TotalCustomers;
            
            worksheet.Cells["A7"].Value = "New Customers";
            worksheet.Cells["B7"].Value = data.NewCustomers;
            
            worksheet.Cells["A8"].Value = "Repeat Customers";
            worksheet.Cells["B8"].Value = data.RepeatCustomers;
            
            worksheet.Cells["A9"].Value = "Customer Satisfaction Score";
            worksheet.Cells["B9"].Value = data.CustomerSatisfactionScore;
            
            worksheet.Cells["A11"].Value = "TOP 10 CUSTOMERS";
            worksheet.Cells["A11"].Style.Font.Bold = true;
            
            var headers = new[] { "Customer Name", "Total Stays", "Total Spent", "Average per Stay" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[12, i + 1].Value = headers[i];
                worksheet.Cells[12, i + 1].Style.Font.Bold = true;
            }
            
            int row = 13;
            foreach (var customer in data.TopCustomers)
            {
                worksheet.Cells[row, 1].Value = customer.CustomerName;
                worksheet.Cells[row, 2].Value = customer.TotalStays;
                worksheet.Cells[row, 3].Value = customer.TotalSpent;
                worksheet.Cells[row, 4].Value = customer.AverageSpentPerStay;
                worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                row++;
            }
            
            worksheet.Cells.AutoFitColumns();
        }

        private async Task CreateChannelReport(ExcelPackage package, ReportRequestDto request)
        {
            var data = await _reportService.GetChannelReportAsync(request.HotelId, request.StartDate, request.EndDate);
            var worksheet = package.Workbook.Worksheets.Add("Channel Report");
            
            worksheet.Cells["A1"].Value = $"Channel Report - {data.HotelName}";
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            
            worksheet.Cells["A3"].Value = $"Period: {data.StartDate:dd/MM/yyyy} - {data.EndDate:dd/MM/yyyy}";
            
            worksheet.Cells["A5"].Value = "SUMMARY";
            worksheet.Cells["A5"].Style.Font.Bold = true;
            
            worksheet.Cells["A6"].Value = "Total Bookings";
            worksheet.Cells["B6"].Value = data.TotalBookings;
            
            worksheet.Cells["A7"].Value = "Total Revenue";
            worksheet.Cells["B7"].Value = data.TotalRevenue;
            worksheet.Cells["B7"].Style.Numberformat.Format = "#,##0.00";
            
            worksheet.Cells["A8"].Value = "Total Commission";
            worksheet.Cells["B8"].Value = data.TotalCommission;
            worksheet.Cells["B8"].Style.Numberformat.Format = "#,##0.00";
            
            worksheet.Cells["A9"].Value = "Net Revenue";
            worksheet.Cells["B9"].Value = data.NetRevenue;
            worksheet.Cells["B9"].Style.Numberformat.Format = "#,##0.00";
            
            var headers = new[] { "Channel", "Bookings", "Revenue", "Commission", "Net Revenue" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[11, i + 1].Value = headers[i];
                worksheet.Cells[11, i + 1].Style.Font.Bold = true;
            }
            
            int row = 12;
            foreach (var channel in data.ChannelData)
            {
                worksheet.Cells[row, 1].Value = channel.ChannelName;
                worksheet.Cells[row, 2].Value = channel.Bookings;
                worksheet.Cells[row, 3].Value = channel.Revenue;
                worksheet.Cells[row, 4].Value = channel.Commission;
                worksheet.Cells[row, 5].Value = channel.Revenue - channel.Commission;
                worksheet.Cells[row, 3, row, 5].Style.Numberformat.Format = "#,##0.00";
                row++;
            }
            
            worksheet.Cells.AutoFitColumns();
        }
    }
}