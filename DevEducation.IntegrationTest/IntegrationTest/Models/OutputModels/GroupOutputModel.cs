namespace IntegrationTest.Models.OutputModels
{
    public class GroupOutputModel
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public CourseOutputModel Course { get; set; }                     
        public string GroupStatus { get; set; } 
        public int GroupStatusId { get; set; }
    }
}