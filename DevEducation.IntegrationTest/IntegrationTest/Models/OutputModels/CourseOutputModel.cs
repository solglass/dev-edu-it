﻿using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class CourseOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public List<ThemeOutputModel> Themes { get; set; }
        public List<MaterialOutputModel> Materials { get; set; }
    }
}
