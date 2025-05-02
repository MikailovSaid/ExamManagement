namespace ExamManagement.Entities
{
    public class Log
    {
        public int LogId { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTime RunAt { get; set; }
        public string Status { get; set; } = null!;
        public string? Message { get; set; }
    }
}
