namespace Reporting.API.DTOs
{
    public class DashboardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelId { get; set; }
        public string DashboardType { get; set; }
        public List<DashboardWidgetDto> Widgets { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public class DashboardWidgetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string WidgetType { get; set; }
        public string ChartType { get; set; }
        public string DataSource { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int RefreshInterval { get; set; }
        public object Data { get; set; }
    }

    public class CreateDashboardDto
    {
        public string Name { get; set; }
        public int HotelId { get; set; }
        public string DashboardType { get; set; }
        public List<CreateDashboardWidgetDto> Widgets { get; set; }
    }

    public class CreateDashboardWidgetDto
    {
        public string Title { get; set; }
        public string WidgetType { get; set; }
        public string ChartType { get; set; }
        public string DataSource { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int RefreshInterval { get; set; }
    }
}