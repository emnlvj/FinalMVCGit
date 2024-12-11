namespace StudentPortal.Models
{
    public class ScheduleViewModel
    {
        public string EDPCode { get; set; }
        public string SubjectCode { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Days { get; set; }
        public string Room { get; set; }
    }
}
