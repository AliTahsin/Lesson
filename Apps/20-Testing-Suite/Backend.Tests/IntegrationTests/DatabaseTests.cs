using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests.IntegrationTests
{
    public class DatabaseTests : TestBase, IAsyncLifetime
    {
        private DbContextOptions<ApplicationDbContext> _options;

        public async Task InitializeAsync()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(_options);
            await context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            using var context = new ApplicationDbContext(_options);
            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task AddHotel_ShouldSaveToDatabase()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                City = "Test City",
                Country = "Test Country",
                StarRating = 4
            };

            // Act
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            // Assert
            var savedHotel = await context.Hotels.FirstOrDefaultAsync(h => h.Name == "Test Hotel");
            savedHotel.Should().NotBeNull();
            savedHotel.Name.Should().Be("Test Hotel");
        }

        [Fact]
        public async Task AddReservation_ShouldSaveToDatabase()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var reservation = new Reservation
            {
                ReservationNumber = "TEST-001",
                GuestName = "Test Guest",
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(3)
            };

            // Act
            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();

            // Assert
            var savedReservation = await context.Reservations.FirstOrDefaultAsync(r => r.ReservationNumber == "TEST-001");
            savedReservation.Should().NotBeNull();
            savedReservation.GuestName.Should().Be("Test Guest");
        }
    }
}