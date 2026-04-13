namespace Staff.API.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalTasksToday { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedToday { get; set; }
        public decimal CompletionRate { get; set; }
        public int CriticalIssues { get; set; }
        public int OpenIssues { get; set; }
        public int TodayCheckIns { get; set; }
        public int TodayCheckOuts { get; set; }
        public Dictionary<string, int> TasksByType { get; set; }
        public Dictionary<string, int> IssuesByCategory { get; set; }
    }
}