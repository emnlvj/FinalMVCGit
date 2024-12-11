namespace StudentPortal.Models
{
    public class StudentEnrollmentViewModel
    {
        public int Id { get; set; }
        public List<ScheduleViewModels> Schedules { get; set; }
    }

    public class ScheduleViewModels
    {
        public int SubEdpCode { get; set; }
    }
}
