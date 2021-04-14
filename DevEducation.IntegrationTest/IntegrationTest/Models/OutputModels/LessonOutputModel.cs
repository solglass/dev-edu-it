using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class LessonOutputModel
    {
        public int Id { get; set; }
        public GroupOutputModel Group { get; set; }
        public string Comment { get; set; }
        public string LessonDate { get; set; }
        public List<ThemeOutputModel> Themes { get; set; }
    }
}
