using System;
using System.Collections.Generic;
using IntegrationTest.Models.OutputModels;

namespace IntegrationTest.Mocks
{
    public static class CourseExtendedOutputModelMockGetter
    {
        public static CourseExtendedOutputModel GetCourseExtendedOutputModelMock(int id)
        {
            return id switch
            {
                1 => new CourseExtendedOutputModel
                {
                    Description = "Course Test 1 mock",
                    Duration = 8,
                    Name = "Test 1 C#",
                    IsDeleted = false,
                    
                    Materials = new List<MaterialOutputModel>()
                },
                2 => new CourseExtendedOutputModel
                {
                    Description = "Course Test 2 mock",
                    Duration = 16,
                    Name = "Test 2 C#",
                    
                    Materials = new List<MaterialOutputModel>()
                },
                3 => new CourseExtendedOutputModel
                {
                    Description = "Course Test 3 mock",
                    Duration = 32,
                    Name = "Test 3 C#",
                    
                    Materials = new List<MaterialOutputModel>()
                },
                4 => new CourseExtendedOutputModel
                {
                    Description = "Course Test 4 mock",
                    Duration = 4,
                    Name = "Test 4 C#",
                    
                    Materials = new List<MaterialOutputModel>()
                },
                5 => new CourseExtendedOutputModel
                {
                    Description = "Course Test 5 mock",
                    Duration = 2,
                    Name = "Test 5 C#",
                    
                    Materials = new List<MaterialOutputModel>()
                },
                6=> new CourseExtendedOutputModel(),
                _ => null,
            };
        }
    }
}
