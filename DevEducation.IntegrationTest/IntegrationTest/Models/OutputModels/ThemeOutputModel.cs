using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class ThemeOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TagOutputModel> Tags { get; set; }
    }
}
