using System;
using System.Collections;
using System.Collections.Generic;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Models.OutputModels;

namespace IntegrationTest.Mocks
{
    public class ValidCourseWithoutThemesAndMaterialsMockCollection : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[]
            {
            new CourseOutputModel
            {
                Description = "Course Test 1 mock",
                Duration = 8,
                Name = "Test 1 C#",
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
            yield return new object[] {
             new CourseOutputModel
             {
                 Description = "Course Test 2 mock",
                 Duration = 16,
                 Name = "Test 2 C#",
                 Materials = new List<MaterialOutputModel>()
             },
            new CourseInputModel
             {
                 Description = "Course Test 2 mock",
                 Duration = 16,
                 Name = "Test 2 C#",
                 Themes = new List<OrderedThemeInputModel>(),
                 MaterialIds = new List<int>()
             } }
            ;
            yield return new object[]
            {
            new CourseOutputModel
            {
                Description = "Course Test 3 mock",
                Duration = 32,
                Name = "Test 3 C#",

                Materials = new List<MaterialOutputModel>()
            }, new CourseInputModel
            {
                Description = "Course Test 3 mock",
                Duration = 32,
                Name = "Test 3 C#",
                Themes = new List<OrderedThemeInputModel>(),
                MaterialIds = new List<int>()
            }
            };

            yield return new object[]
            {
             new CourseOutputModel
            {
                Description = "Course Test 4 mock",
                Duration = 4,
                Name = "Test 4 C#",
                Materials = new List<MaterialOutputModel>()
            },
                new CourseInputModel
            {
                Description = "Course Test 4 mock",
                Duration = 4,
                Name = "Test 4 C#",
                Themes = new List<OrderedThemeInputModel>(),
                MaterialIds = new List<int>()
            }
            };
            yield return new object[]
            {
            new CourseOutputModel
            {
                Description = "Course Test 5 mock",
                Duration = 2,
                Name = "Test 5 C#",
                Materials = new List<MaterialOutputModel>()
            }, new CourseInputModel
            {
                Description = "Course Test 5 mock",
                Duration = 2,
                Name = "Test 5 C#",
                Themes = new List<OrderedThemeInputModel>(),
                MaterialIds = new List<int>()
            }
            };

        }
    }
}
