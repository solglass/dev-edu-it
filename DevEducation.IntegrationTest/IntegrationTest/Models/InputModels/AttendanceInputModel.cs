namespace IntegrationTest.Models.InputModels
{
    public class AttendanceInputModel
    {
        public int UserId { get; set; }
        public bool IsAbsent { get; set; }
        public string ReasonOfAbsence { get; set; }
    }
}
