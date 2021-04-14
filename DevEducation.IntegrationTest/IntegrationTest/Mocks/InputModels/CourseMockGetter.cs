using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IntegrationTest.Models.InputModels
{
    [ExcludeFromCodeCoverage]
    public static class CourseInputModelMockGetter
    {
        public static CourseInputModel GetCourseInputModelMock(int id)
        {
            return id switch
            {
                1 => new CourseInputModel
                {
                    Description = "Course Test 1 mock",
                    Duration = 8,
                    Name = "Test 1 C#",
                },
                2 => new CourseInputModel
                {
                    Description = "Course Test 2 mock",
                    Duration = 16,
                    Name = "Test 2 C#",
                },
                3 => new CourseInputModel
                {
                    Description = "Course Test 3 mock",
                    Duration = 32,
                    Name = "Test 3 C#",
                },
                4 => new CourseInputModel
                {
                    Description = "Course Test 4 mock",
                    Duration = 4,
                    Name = "Test 4 C#",
                },
                5 => new CourseInputModel
                {
                    Description = "Course Test 5 mock",
                    Duration = 2,
                    Name = "Test 5 C#",
                },
                6=> new CourseInputModel(),
                _ => null,
            };
        }
    }
}
