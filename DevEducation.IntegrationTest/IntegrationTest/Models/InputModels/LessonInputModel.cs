using System.Collections.Generic;
namespace IntegrationTest.Models.InputModels
{
    public class LessonInputModel
    {
        public int GroupId { get; set; }  
        public string Comment { get; set; }
        public string LessonDate { get; set; }
        public List<int> ThemesId { get; set; }
    }
}
