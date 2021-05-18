using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest.Models.OutputModels
{
    public class CourseWithProgramOutputModel: CourseOutputModel
    {
        public List<ThemeOrderedOutputModel> Themes { get; set; }
    }
}
