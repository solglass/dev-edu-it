using System.Collections.Generic;

namespace IntegrationTest.Models.InputModels
{
    public class CourseInputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public List<int> ThemeIds { get; set; }
        public List<int> MaterialIds { get; set; }
    }
}