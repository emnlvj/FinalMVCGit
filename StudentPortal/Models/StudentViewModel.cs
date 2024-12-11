namespace StudentPortal.Models
{
    public class StudentViewModel
    {
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string Course { get; set; }
        public string Year { get; set; }
        public List<ScheduleViewModel> ManySchedules { get; set; }
    }
}
