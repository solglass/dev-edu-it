using System;
using System.Collections;
using System.Collections.Generic;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Models.OutputModels;

namespace IntegrationTest.Mocks
{
    public class ValidCourseExtendedModelMockCollection : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[]
            {
            new CourseExtendedOutputModel
            {
                Description = "Course Test 1 mock",
                Duration = 8,
                Name = "Test 1 C#",
                IsDeleted = false,
                Materials = new List<MaterialOutputModel>()
            },
            new CourseInputModel
            {
                Description = "Course Test 1 mock",
                Duration = 8,
                Name = "Test 1 C#",
                Themes = new List<OrderedThemeInputModel>(),
                MaterialIds = new List<int>()
            }
            };

        }
    }
}
