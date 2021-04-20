using System.Collections.Generic;
namespace IntegrationTest.Models.InputModels
{
    public class ThemeInputModel
    {
       public string Name { get; set; }
       public List<int> TagIds { get; set; }

        public object Clone()
        {
            return new ThemeInputModel()
            {
                Name = Name,
                TagIds = TagIds
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ThemeInputModel))
                return false;

            var ThemeObj = (ThemeInputModel)obj;

            return (
                    Name.Equals(ThemeObj.Name));
        }

        public override string ToString()
        {
            return $" {Name} ";
        }
    }
}

