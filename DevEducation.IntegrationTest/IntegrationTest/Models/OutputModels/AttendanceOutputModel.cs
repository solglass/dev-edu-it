namespace IntegrationTest.Models.OutputModels
{
    public class AttendanceOutputModel
    {
        public int Id { get; set; }
        public AuthorOutputModel User { get; set; }
        public bool IsAbsent { get; set; }
        public string ReasonOfAbsence { get; set; }
    }
}
