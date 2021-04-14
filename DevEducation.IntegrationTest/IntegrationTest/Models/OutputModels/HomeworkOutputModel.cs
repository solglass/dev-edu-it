using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class HomeworkOutputModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string DeadlineDate { get; set; }
        public bool IsOptional { get; set; }
        public GroupOutputModel Group { get; set; }
        public List<TagOutputModel> Tags { get; set; }
        public List<HomeworkAttemptOutputModel> HomeworkAttempts { get; set; }
        public List<ThemeOutputModel> Themes { get; set; }
    }
}
