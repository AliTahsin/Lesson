using Housekeeping.API.DTOs;

namespace Housekeeping.API.Services
{
    public interface IIssueService
    {
        Task<IssueDto> GetIssueByIdAsync(int id);
        Task<List<IssueDto>> GetAllIssuesAsync();
        Task<List<IssueDto>> GetIssuesByHotelAsync(int hotelId);
        Task<List<IssueDto>> GetCriticalIssuesAsync();
        Task<IssueDto> ReportIssueAsync(CreateIssueDto dto);
        Task<IssueDto> AssignIssueAsync(int issueId, int staffId);
        Task<IssueDto> ResolveIssueAsync(int issueId, ResolveIssueDto dto);
        Task<IssueDto> CloseIssueAsync(int issueId, string notes);
    }
}