using System.Collections.Generic;
namespace IntegrationTest.Models.InputModels
{
    public class HomeworkUpdateInputModel
    {
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string DeadlineDate { get; set; }
        public List<int> TagIds { get; set; }
        public List<int> ThemeIds { get; set; }
        public bool IsOptional { get; set; }
    }
}
