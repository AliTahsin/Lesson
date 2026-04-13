using AutoMapper;
using Restaurant.API.Models;
using Restaurant.API.DTOs;
using Restaurant.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Restaurant.API.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(
            IReservationRepository reservationRepository,
            IRestaurantRepository restaurantRepository,
            IMapper mapper,
            IHubContext<SignalRHub> hubContext,
            ILogger<ReservationService> logger)
        {
            _reservationRepository = reservationRepository;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<ReservationDto> GetReservationByIdAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            return reservation != null ? _mapper.Map<ReservationDto>(reservation) : null;
        }

        public async Task<List<ReservationDto>> GetReservationsByRestaurantAsync(int restaurantId)
        {
            var reservations = await _reservationRepository.GetByRestaurantAsync(restaurantId);
            return _mapper.Map<List<ReservationDto>>(reservations);
        }

        public async Task<List<ReservationDto>> GetReservationsByDateAsync(int restaurantId, DateTime date)
        {
            var reservations = await _reservationRepository.GetByDateAsync(restaurantId, date);
            return _mapper.Map<List<ReservationDto>>(reservations);
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto dto)
        {
            // Check availability
            var availableTables = await _reservationRepository.GetAvailableTablesAsync(
                dto.RestaurantId, dto.ReservationDate, dto.ReservationTime, dto.GuestCount);
            
            if (!availableTables.Any())
                throw new Exception("No available tables for the selected time");
            
            var reservation = _mapper.Map<TableReservation>(dto);
            reservation.TableId = availableTables.First().Id;
            reservation.TableNumber = availableTables.First().TableNumber;
            
            var createdReservation = await _reservationRepository.CreateAsync(reservation);
            
            // Send notification
            await _hubContext.Clients.Group($"restaurant-{dto.RestaurantId}").SendAsync("NewReservation", createdReservation);
            
            return _mapper.Map<ReservationDto>(createdReservation);
        }

        public async Task<ReservationDto> ConfirmReservationAsync(int id)
        {
            var reservation = await _reservationRepository.UpdateStatusAsync(id, "Confirmed");
            return _mapper.Map<ReservationDto>(reservation);
        }

        public async Task<ReservationDto> CancelReservationAsync(int id, string reason)
        {
            var reservation = await _reservationRepository.UpdateStatusAsync(id, "Cancelled");
            
            // Send notification
            await _hubContext.Clients.Group($"restaurant-{reservation.RestaurantId}").SendAsync("ReservationCancelled", id, reason);
            
            return _mapper.Map<ReservationDto>(reservation);
        }

        public async Task<List<TableDto>> GetAvailableTablesAsync(int restaurantId, DateTime date, string time, int guestCount)
        {
            var tables = await _reservationRepository.GetAvailableTablesAsync(restaurantId, date, time, guestCount);
            return _mapper.Map<List<TableDto>>(tables);
        }
    }
}