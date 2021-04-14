using System.Collections.Generic;
namespace IntegrationTest.Models.InputModels
{
    public class ThemeInputModel
    {
       public string Name { get; set; }
       public List<int> TagIds { get; set; }
    }
}
