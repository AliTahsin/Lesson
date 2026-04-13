namespace Housekeeping.API.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalTasksToday { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedToday { get; set; }
        public decimal CompletionRate { get; set; }
        public double AverageTaskTime { get; set; }
        public int CriticalIssues { get; set; }
        public int OpenIssues { get; set; }
        public int AverageResolutionTime { get; set; }
        public Dictionary<string, int> TasksByType { get; set; }
        public Dictionary<string, int> IssuesByCategory { get; set; }
    }
}