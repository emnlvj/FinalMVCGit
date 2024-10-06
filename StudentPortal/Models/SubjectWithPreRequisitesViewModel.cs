namespace StudentPortal.Models
{
    public class SubjectWithPreRequisitesViewModel
    {
        public Subject Subject { get; set; }
        public List<PreRequisite> PreRequisites { get; set; }
    }
}
