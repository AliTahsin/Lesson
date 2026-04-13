using AutoMapper;
using Staff.API.Models;
using Staff.API.DTOs;
using Staff.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Staff.API.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<IssueService> _logger;

        public IssueService(
            IIssueRepository issueRepository,
            IStaffRepository staffRepository,
            INotificationService notificationService,
            IMapper mapper,
            IHubContext<SignalRHub> hubContext,
            ILogger<IssueService> logger)
        {
            _issueRepository = issueRepository;
            _staffRepository = staffRepository;
            _notificationService = notificationService;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<IssueDto> GetIssueByIdAsync(int id)
        {
            var issue = await _issueRepository.GetByIdAsync(id);
            return issue != null ? _mapper.Map<IssueDto>(issue) : null;
        }

        public async Task<List<IssueDto>> GetIssuesByStaffAsync(int staffId)
        {
            var issues = await _issueRepository.GetByStaffAsync(staffId);
            return _mapper.Map<List<IssueDto>>(issues);
        }

        public async Task<List<IssueDto>> GetCriticalIssuesAsync(int hotelId)
        {
            var issues = await _issueRepository.GetCriticalIssuesAsync(hotelId);
            return _mapper.Map<List<IssueDto>>(issues);
        }

        public async Task<IssueDto> ReportIssueAsync(CreateIssueDto dto)
        {
            var issue = new MaintenanceIssue
            {
                IssueNumber = $"ISS-{DateTime.Now.Ticks}",
                HotelId = dto.HotelId,
                RoomId = dto.RoomId,
                RoomNumber = dto.RoomNumber,
                Category = dto.Category,
                Description = dto.Description,
                Priority = dto.Priority,
                ReportedByStaffId = dto.ReportedByStaffId,
                ReportedByName = dto.ReportedByName,
                Status = "Reported",
                Images = dto.Images ?? new List<string>()
            };

            await _issueRepository.AddAsync(issue);

            // Send notification for critical issues
            if (dto.Priority == "Critical")
            {
                await _notificationService.SendCriticalIssueNotification(dto.HotelId, issue);
                await _hubContext.Clients.Group($"hotel-{dto.HotelId}-maintenance").SendAsync("CriticalIssue", issue);
            }

            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto> AssignIssueAsync(int issueId, int staffId)
        {
            var issue = await _issueRepository.GetByIdAsync(issueId);
            if (issue == null)
                throw new Exception("Issue not found");

            var staff = await _staffRepository.GetByIdAsync(staffId);
            if (staff == null)
                throw new Exception("Staff not found");

            issue.AssignedToStaffId = staffId;
            issue.AssignedToStaffName = staff.FullName;
            issue.AssignedAt = DateTime.Now;
            issue.Status = "Assigned";
            await _issueRepository.UpdateAsync(issue);

            await _notificationService.SendIssueNotification(staffId, issue.Id, issue.Category);
            await _hubContext.Clients.User(staffId.ToString()).SendAsync("IssueAssigned", issue);

            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto> StartIssueAsync(int issueId)
        {
            var issue = await _issueRepository.UpdateStatusAsync(issueId, "InProgress");
            if (issue == null)
                throw new Exception("Issue not found");

            await _hubContext.Clients.Group($"hotel-{issue.HotelId}").SendAsync("IssueStarted", issue);

            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto> ResolveIssueAsync(int issueId, ResolveIssueDto dto)
        {
            var issue = await _issueRepository.UpdateStatusAsync(issueId, "Resolved");
            if (issue == null)
                throw new Exception("Issue not found");

            issue.ResolutionNotes = dto.ResolutionNotes;
            issue.ActualCost = dto.ActualCost;
            await _issueRepository.UpdateAsync(issue);

            await _hubContext.Clients.Group($"hotel-{issue.HotelId}").SendAsync("IssueResolved", issue);

            return _mapper.Map<IssueDto>(issue);
        }
    }
}