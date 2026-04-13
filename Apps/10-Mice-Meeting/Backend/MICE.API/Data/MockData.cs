using MICE.API.Models;

namespace MICE.API.Data
{
    public static class MockData
    {
        private static readonly Random _random = new();

        public static List<MeetingRoom> GetMeetingRooms()
        {
            return new List<MeetingRoom>
            {
                new MeetingRoom
                {
                    Id = 1,
                    HotelId = 1,
                    Name = "Grand Ballroom",
                    RoomCode = "GB-01",
                    Capacity = 500,
                    TheaterCapacity = 500,
                    ClassroomCapacity = 300,
                    UShapeCapacity = 150,
                    BoardroomCapacity = 80,
                    Area = 500,
                    HalfDayPrice = 1500,
                    FullDayPrice = 2500,
                    HasProjector = true,
                    HasSoundSystem = true,
                    HasWhiteboard = true,
                    HasFlipChart = true,
                    HasWiFi = true,
                    HasAirConditioning = true,
                    HasNaturalLight = true,
                    HasBlackout = true,
                    Images = new List<string> { "ballroom1.jpg", "ballroom2.jpg" },
                    Floor = "1st Floor",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-12)
                },
                new MeetingRoom
                {
                    Id = 2,
                    HotelId = 1,
                    Name = "Conference Room A",
                    RoomCode = "CR-A",
                    Capacity = 50,
                    TheaterCapacity = 50,
                    ClassroomCapacity = 30,
                    UShapeCapacity = 25,
                    BoardroomCapacity = 20,
                    Area = 80,
                    HalfDayPrice = 400,
                    FullDayPrice = 700,
                    HasProjector = true,
                    HasSoundSystem = false,
                    HasWhiteboard = true,
                    HasFlipChart = true,
                    HasWiFi = true,
                    HasAirConditioning = true,
                    HasNaturalLight = true,
                    HasBlackout = true,
                    Images = new List<string> { "conferenceA.jpg" },
                    Floor = "2nd Floor",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-10)
                },
                new MeetingRoom
                {
                    Id = 3,
                    HotelId = 1,
                    Name = "Boardroom",
                    RoomCode = "BR-01",
                    Capacity = 20,
                    TheaterCapacity = 20,
                    ClassroomCapacity = 15,
                    UShapeCapacity = 15,
                    BoardroomCapacity = 20,
                    Area = 50,
                    HalfDayPrice = 300,
                    FullDayPrice = 500,
                    HasProjector = true,
                    HasSoundSystem = false,
                    HasWhiteboard = true,
                    HasFlipChart = true,
                    HasWiFi = true,
                    HasAirConditioning = true,
                    HasNaturalLight = true,
                    HasBlackout = false,
                    Images = new List<string> { "boardroom.jpg" },
                    Floor = "3rd Floor",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-8)
                },
                new MeetingRoom
                {
                    Id = 4,
                    HotelId = 2,
                    Name = "Meeting Hall",
                    RoomCode = "MH-01",
                    Capacity = 300,
                    TheaterCapacity = 300,
                    ClassroomCapacity = 200,
                    UShapeCapacity = 100,
                    BoardroomCapacity = 60,
                    Area = 350,
                    HalfDayPrice = 1000,
                    FullDayPrice = 1800,
                    HasProjector = true,
                    HasSoundSystem = true,
                    HasWhiteboard = true,
                    HasFlipChart = true,
                    HasWiFi = true,
                    HasAirConditioning = true,
                    HasNaturalLight = true,
                    HasBlackout = true,
                    Images = new List<string> { "meetinghall.jpg" },
                    Floor = "Ground Floor",
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                }
            };
        }

        public static List<Event> GetEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    Id = 1,
                    EventNumber = "EVT-2024-0001",
                    HotelId = 1,
                    MeetingRoomId = 1,
                    Name = "Annual Tech Conference 2024",
                    Description = "Annual technology conference with industry leaders",
                    EventType = "Conference",
                    StartDate = new DateTime(2024, 6, 10, 9, 0, 0),
                    EndDate = new DateTime(2024, 6, 12, 18, 0, 0),
                    SetupStartTime = new DateTime(2024, 6, 9, 8, 0, 0),
                    SetupEndTime = new DateTime(2024, 6, 9, 20, 0, 0),
                    ExpectedAttendees = 450,
                    ActualAttendees = 420,
                    TotalBudget = 50000,
                    ActualCost = 48000,
                    Status = "Confirmed",
                    CustomerName = "John Smith",
                    CustomerEmail = "john@techcorp.com",
                    CustomerPhone = "+1234567890",
                    CustomerCompany = "Tech Corp",
                    SpecialRequests = "Need extra power outlets",
                    EquipmentIds = new List<int> { 1, 2, 3 },
                    CreatedAt = DateTime.Now.AddMonths(-3),
                    ConfirmedAt = DateTime.Now.AddMonths(-2)
                },
                new Event
                {
                    Id = 2,
                    EventNumber = "EVT-2024-0002",
                    HotelId = 1,
                    MeetingRoomId = 2,
                    Name = "Marketing Summit",
                    Description = "Digital marketing strategies for 2024",
                    EventType = "Seminar",
                    StartDate = new DateTime(2024, 7, 15, 10, 0, 0),
                    EndDate = new DateTime(2024, 7, 15, 17, 0, 0),
                    SetupStartTime = new DateTime(2024, 7, 14, 14, 0, 0),
                    SetupEndTime = new DateTime(2024, 7, 14, 18, 0, 0),
                    ExpectedAttendees = 45,
                    ActualAttendees = 0,
                    TotalBudget = 5000,
                    ActualCost = 0,
                    Status = "Planned",
                    CustomerName = "Jane Doe",
                    CustomerEmail = "jane@marketingpro.com",
                    CustomerPhone = "+1987654321",
                    CustomerCompany = "Marketing Pro",
                    EquipmentIds = new List<int> { 1, 4 },
                    CreatedAt = DateTime.Now.AddMonths(-1)
                }
            };
        }

        public static List<EventSchedule> GetEventSchedules()
        {
            return new List<EventSchedule>
            {
                new EventSchedule
                {
                    Id = 1,
                    EventId = 1,
                    Title = "Opening Keynote",
                    Description = "Welcome and opening address",
                    StartTime = new DateTime(2024, 6, 10, 9, 0, 0),
                    EndTime = new DateTime(2024, 6, 10, 10, 30, 0),
                    Speaker = "John Smith",
                    Location = "Grand Ballroom",
                    Order = 1
                },
                new EventSchedule
                {
                    Id = 2,
                    EventId = 1,
                    Title = "AI Workshop",
                    Description = "Hands-on AI implementation workshop",
                    StartTime = new DateTime(2024, 6, 10, 11, 0, 0),
                    EndTime = new DateTime(2024, 6, 10, 13, 0, 0),
                    Speaker = "Dr. Alan Turing",
                    Location = "Workshop Room A",
                    Order = 2
                }
            };
        }

        public static List<EventAttendee> GetAttendees()
        {
            var attendees = new List<EventAttendee>();
            for (int i = 1; i <= 50; i++)
            {
                attendees.Add(new EventAttendee
                {
                    Id = i,
                    EventId = 1,
                    FirstName = $"Attendee{i}",
                    LastName = "LastName",
                    Email = $"attendee{i}@email.com",
                    Phone = $"+90 5{_random.Next(10, 99)} {_random.Next(100, 999)} {_random.Next(1000, 9999)}",
                    Company = $"Company {_random.Next(1, 10)}",
                    Title = "Manager",
                    HasCheckedIn = _random.Next(0, 10) > 3,
                    CheckInTime = _random.Next(0, 10) > 3 ? DateTime.Now.AddHours(-_random.Next(1, 5)) : null,
                    Status = "Confirmed",
                    RegisteredAt = DateTime.Now.AddDays(-_random.Next(1, 30))
                });
            }
            return attendees;
        }

        public static List<Equipment> GetEquipment()
        {
            return new List<Equipment>
            {
                new Equipment
                {
                    Id = 1,
                    HotelId = 1,
                    Name = "HD Projector",
                    Category = "Visual",
                    Description = "4K Ultra HD Projector",
                    DailyPrice = 150,
                    WeeklyPrice = 500,
                    TotalQuantity = 10,
                    AvailableQuantity = 8,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-12)
                },
                new Equipment
                {
                    Id = 2,
                    HotelId = 1,
                    Name = "Sound System",
                    Category = "Audio",
                    Description = "Professional sound system with microphones",
                    DailyPrice = 200,
                    WeeklyPrice = 700,
                    TotalQuantity = 5,
                    AvailableQuantity = 4,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-10)
                },
                new Equipment
                {
                    Id = 3,
                    HotelId = 1,
                    Name = "Whiteboard",
                    Category = "Furniture",
                    Description = "Magnetic whiteboard with markers",
                    DailyPrice = 50,
                    WeeklyPrice = 150,
                    TotalQuantity = 15,
                    AvailableQuantity = 12,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-8)
                },
                new Equipment
                {
                    Id = 4,
                    HotelId = 1,
                    Name = "Flip Chart",
                    Category = "Furniture",
                    Description = "Flip chart with paper pads",
                    DailyPrice = 30,
                    WeeklyPrice = 100,
                    TotalQuantity = 20,
                    AvailableQuantity = 18,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                }
            };
        }
    }
}