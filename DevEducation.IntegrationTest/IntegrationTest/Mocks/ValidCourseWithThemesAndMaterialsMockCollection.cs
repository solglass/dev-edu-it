using IntegrationTest.Models.InputModels;
using IntegrationTest.Models.OutputModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest.Mocks
{
    public class ValidCourseWithThemesAndMaterialsMockCollection : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] {  new CourseWithProgramOutputModel
            {
                Description = "Course Test 1 mock",
                Duration = 8,
                Name = "Test 1 C#",
                Themes = new List<ThemeOrderedOutputModel>(),
                Materials = new List<MaterialOutputModel>()
            }, 
            new ThemeOutputModel[] { new ThemeOutputModel
            {
                Name = "Test theme 1"
            }, new ThemeOutputModel
            {
                Name = "Test theme 2"
            } 
            },
            new MaterialOutputModel[] { new MaterialOutputModel
            {
                Description = "Description test",
                Link = "Link test1"
            }, new MaterialOutputModel
            {
                Description = "Description test2",
                Link = "Link test2"
            }              
            },
             new CourseInputModel
             {
                 Description = "Course Test 1 mock",
                 Duration = 8,
                 Name = "Test 1 C#",
                 Themes = new List<OrderedThemeInputModel>(),
                 MaterialIds = new List<int>()
             }, 
            new ThemeInputModel[] { new ThemeInputModel
            {
                Name = "Test theme 1"
            }, new ThemeInputModel
            {
                Name = "Test theme 2"
            } 
            },
            new MaterialInputModel[] { new MaterialInputModel
            {
                Description = "Description test",
                Link = "Link test1"
            }, new MaterialInputModel
            {
                Description = "Description test2",
                Link = "Link test2"
            }
            }
            };
        }
    }
}

