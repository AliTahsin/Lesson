namespace DynamicPricing.API.DTOs
{
    public class PriceRequestDto
    {
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int GuestCount { get; set; }
        public string PromoCode { get; set; }
    }

    public class PriceResponseDto
    {
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NightCount { get; set; }
        public List<DailyPriceDto> DailyPrices { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal AveragePricePerNight { get; set; }
        public decimal LongStayDiscount { get; set; }
        public decimal PromoDiscount { get; set; }
    }

    public class DailyPriceDto
    {
        public DateTime Date { get; set; }
        public decimal BasePrice { get; set; }
        public decimal FinalPrice { get; set; }
        public List<PriceBreakdownDto> Breakdowns { get; set; }
        public int DemandScore { get; set; }
        public decimal OccupancyRate { get; set; }
    }

    public class PriceBreakdownDto
    {
        public string Factor { get; set; }
        public string Description { get; set; }
        public decimal Multiplier { get; set; }
        public decimal Impact { get; set; }
    }
}