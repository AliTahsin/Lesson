using AutoMapper;
using Housekeeping.API.Models;
using Housekeeping.API.DTOs;
using Housekeeping.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Housekeeping.API.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ILogger<IssueService> _logger;

        public IssueService(
            IIssueRepository issueRepository,
            IMapper mapper,
            IHubContext<SignalRHub> hubContext,
            ILogger<IssueService> logger)
        {
            _issueRepository = issueRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<IssueDto> GetIssueByIdAsync(int id)
        {
            var issue = await _issueRepository.GetByIdAsync(id);
            return issue != null ? _mapper.Map<IssueDto>(issue) : null;
        }

        public async Task<List<IssueDto>> GetAllIssuesAsync()
        {
            var issues = await _issueRepository.GetAllAsync();
            return _mapper.Map<List<IssueDto>>(issues);
        }

        public async Task<List<IssueDto>> GetIssuesByHotelAsync(int hotelId)
        {
            var issues = await _issueRepository.GetByHotelAsync(hotelId);
            return _mapper.Map<List<IssueDto>>(issues);
        }

        public async Task<List<IssueDto>> GetCriticalIssuesAsync()
        {
            var issues = await _issueRepository.GetCriticalIssuesAsync();
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
                ReportedAt = DateTime.Now,
                Status = "Reported",
                Images = dto.Images ?? new List<string>()
            };

            await _issueRepository.AddAsync(issue);
            
            // Send real-time notification for critical issues
            if (dto.Priority == "Critical")
            {
                await _hubContext.Clients.Group($"hotel-{dto.HotelId}-maintenance").SendAsync("CriticalIssue", issue);
            }
            
            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto> AssignIssueAsync(int issueId, int staffId)
        {
            var issue = await _issueRepository.GetByIdAsync(issueId);
            if (issue == null)
                throw new Exception("Issue not found");

            issue.AssignedToStaffId = staffId;
            issue.AssignedToStaffName = $"Staff {staffId}";
            issue.AssignedAt = DateTime.Now;
            issue.Status = "Assigned";
            await _issueRepository.UpdateAsync(issue);
            
            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto> ResolveIssueAsync(int issueId, ResolveIssueDto dto)
        {
            var issue = await _issueRepository.GetByIdAsync(issueId);
            if (issue == null)
                throw new Exception("Issue not found");

            issue.Status = "Resolved";
            issue.ResolvedAt = DateTime.Now;
            issue.ResolutionNotes = dto.ResolutionNotes;
            issue.ActualCost = dto.ActualCost;
            await _issueRepository.UpdateAsync(issue);
            
            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto> CloseIssueAsync(int issueId, string notes)
        {
            var issue = await _issueRepository.GetByIdAsync(issueId);
            if (issue == null)
                throw new Exception("Issue not found");

            issue.Status = "Closed";
            issue.ResolutionNotes += $"\nClosed: {notes}";
            await _issueRepository.UpdateAsync(issue);
            
            return _mapper.Map<IssueDto>(issue);
        }
    }
}