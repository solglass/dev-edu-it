using System.Diagnostics.CodeAnalysis;

namespace IntegrationTest.Models.InputModels
{
    public static class AttendanceMockGetter
    {
        public static AttendanceInputModel GetAttendanceInputModel(int Id)
        {
            return Id switch
            {
                0 => new AttendanceInputModel(),
                1 => new AttendanceInputModel
                {
                    UserId = 1,
                    IsAbsent = true,
                    ReasonOfAbsence = null
                },
                2 => new AttendanceInputModel
                {
                    UserId = 2,
                    IsAbsent = false,
                    ReasonOfAbsence = "Important"
                },
                _ => null,
            };
        }
    }
}
