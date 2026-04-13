using Reporting.API.Models;

namespace Reporting.API.Data
{
    public static class MockData
    {
        public static List<Report> GetReports()
        {
            var reports = new List<Report>();
            var random = new Random();
            
            for (int i = 1; i <= 50; i++)
            {
                reports.Add(new Report
                {
                    Id = i,
                    Name = $"Report_{i}_{DateTime.Now:yyyyMMdd}",
                    Description = $"Sample report {i}",
                    ReportType = new[] { "Revenue", "Occupancy", "Reservation", "Customer", "Channel" }[random.Next(5)],
                    Format = random.Next(0, 10) > 5 ? "Excel" : "PDF",
                    HotelId = random.Next(1, 4),
                    StartDate = DateTime.Now.AddDays(-random.Next(1, 90)),
                    EndDate = DateTime.Now,
                    GeneratedAt = DateTime.Now.AddHours(-random.Next(1, 48)),
                    FileSize = random.Next(100000, 5000000),
                    Status = "Completed",
                    GeneratedByUserId = 1,
                    GeneratedByUserName = "Admin User",
                    CreatedAt = DateTime.Now.AddHours(-random.Next(1, 48))
                });
            }
            
            return reports.OrderByDescending(r => r.GeneratedAt).ToList();
        }

        public static List<Dashboard> GetDashboards()
        {
            var dashboards = new List<Dashboard>
            {
                new Dashboard
                {
                    Id = 1,
                    Name = "Executive Dashboard",
                    HotelId = 1,
                    DashboardType = "Executive",
                    IsDefault = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6),
                    Widgets = new List<DashboardWidget>
                    {
                        new DashboardWidget
                        {
                            Id = 1,
                            DashboardId = 1,
                            Title = "Total Revenue",
                            WidgetType = "KPI",
                            Width = 3,
                            Height = 2,
                            PositionX = 0,
                            PositionY = 0,
                            RefreshInterval = 300
                        },
                        new DashboardWidget
                        {
                            Id = 2,
                            DashboardId = 1,
                            Title = "Occupancy Rate",
                            WidgetType = "KPI",
                            Width = 3,
                            Height = 2,
                            PositionX = 3,
                            PositionY = 0,
                            RefreshInterval = 300
                        },
                        new DashboardWidget
                        {
                            Id = 3,
                            DashboardId = 1,
                            Title = "Revenue Trend",
                            WidgetType = "Chart",
                            ChartType = "Line",
                            Width = 6,
                            Height = 4,
                            PositionX = 0,
                            PositionY = 2,
                            RefreshInterval = 600
                        }
                    }
                },
                new Dashboard
                {
                    Id = 2,
                    Name = "Operations Dashboard",
                    HotelId = 1,
                    DashboardType = "Operations",
                    IsDefault = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-5),
                    Widgets = new List<DashboardWidget>()
                }
            };
            
            return dashboards;
        }

        public static List<KPI> GetKPIs()
        {
            var random = new Random();
            return new List<KPI>
            {
                new KPI
                {
                    Id = 1,
                    Name = "Revenue Per Available Room",
                    Code = "REVPAR",
                    Description = "Total room revenue divided by total available rooms",
                    Category = "Revenue",
                    HotelId = 1,
                    CurrentValue = random.Next(80, 150),
                    PreviousValue = random.Next(70, 140),
                    TargetValue = 120,
                    ChangePercent = random.Next(-10, 20),
                    Trend = random.Next(0, 10) > 7 ? "Up" : "Stable",
                    Status = "OnTrack",
                    LastUpdated = DateTime.Now,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new KPI
                {
                    Id = 2,
                    Name = "Average Daily Rate",
                    Code = "ADR",
                    Description = "Average room rate per occupied room",
                    Category = "Revenue",
                    HotelId = 1,
                    CurrentValue = random.Next(100, 200),
                    PreviousValue = random.Next(90, 180),
                    TargetValue = 160,
                    ChangePercent = random.Next(-10, 20),
                    Trend = random.Next(0, 10) > 7 ? "Up" : "Stable",
                    Status = "OnTrack",
                    LastUpdated = DateTime.Now,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new KPI
                {
                    Id = 3,
                    Name = "Occupancy Rate",
                    Code = "OCCUPANCY",
                    Description = "Percentage of occupied rooms",
                    Category = "Operations",
                    HotelId = 1,
                    CurrentValue = random.Next(60, 90),
                    PreviousValue = random.Next(55, 85),
                    TargetValue = 80,
                    ChangePercent = random.Next(-10, 15),
                    Trend = random.Next(0, 10) > 8 ? "Up" : "Stable",
                    Status = "OnTrack",
                    LastUpdated = DateTime.Now,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new KPI
                {
                    Id = 4,
                    Name = "Customer Satisfaction",
                    Code = "CSAT",
                    Description = "Customer satisfaction score",
                    Category = "Customer",
                    HotelId = 1,
                    CurrentValue = random.Next(75, 98),
                    PreviousValue = random.Next(70, 95),
                    TargetValue = 90,
                    ChangePercent = random.Next(-5, 10),
                    Trend = random.Next(0, 10) > 6 ? "Up" : "Stable",
                    Status = "OnTrack",
                    LastUpdated = DateTime.Now,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                }
            };
        }
    }
}