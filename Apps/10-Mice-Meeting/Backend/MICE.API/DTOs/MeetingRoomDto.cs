namespace MICE.API.DTOs
{
    public class MeetingRoomDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string RoomCode { get; set; }
        public int Capacity { get; set; }
        public int TheaterCapacity { get; set; }
        public int ClassroomCapacity { get; set; }
        public int UShapeCapacity { get; set; }
        public int BoardroomCapacity { get; set; }
        public decimal Area { get; set; }
        public decimal HalfDayPrice { get; set; }
        public decimal FullDayPrice { get; set; }
        public bool HasProjector { get; set; }
        public bool HasSoundSystem { get; set; }
        public bool HasWhiteboard { get; set; }
        public bool HasWiFi { get; set; }
        public bool HasNaturalLight { get; set; }
        public List<string> Images { get; set; }
        public string Floor { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateMeetingRoomDto
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string RoomCode { get; set; }
        public int Capacity { get; set; }
        public int TheaterCapacity { get; set; }
        public int ClassroomCapacity { get; set; }
        public int UShapeCapacity { get; set; }
        public int BoardroomCapacity { get; set; }
        public decimal Area { get; set; }
        public decimal HalfDayPrice { get; set; }
        public decimal FullDayPrice { get; set; }
        public bool HasProjector { get; set; }
        public bool HasSoundSystem { get; set; }
        public bool HasWhiteboard { get; set; }
        public bool HasWiFi { get; set; }
        public List<string> Images { get; set; }
        public string Floor { get; set; }
    }

    public class UpdateMeetingRoomDto
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal HalfDayPrice { get; set; }
        public decimal FullDayPrice { get; set; }
        public bool HasProjector { get; set; }
        public bool HasSoundSystem { get; set; }
        public bool IsActive { get; set; }
    }
}