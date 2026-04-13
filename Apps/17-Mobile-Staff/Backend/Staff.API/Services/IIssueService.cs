using Staff.API.DTOs;

namespace Staff.API.Services
{
    public interface IIssueService
    {
        Task<IssueDto> GetIssueByIdAsync(int id);
        Task<List<IssueDto>> GetIssuesByStaffAsync(int staffId);
        Task<List<IssueDto>> GetCriticalIssuesAsync(int hotelId);
        Task<IssueDto> ReportIssueAsync(CreateIssueDto dto);
        Task<IssueDto> AssignIssueAsync(int issueId, int staffId);
        Task<IssueDto> StartIssueAsync(int issueId);
        Task<IssueDto> ResolveIssueAsync(int issueId, ResolveIssueDto dto);
    }
}