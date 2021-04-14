using System;
using System.Collections.Generic;

namespace IntegrationTest.Models.InputModels
{
    public class CourseInputModel :ICloneable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public List<int> ThemeIds { get; set; }
        public List<int> MaterialIds { get; set; }

        public object Clone()
        {
            return new CourseInputModel()
            {
                Name = Name,
                Description = Description,
                Duration = Duration,
                ThemeIds = ThemeIds,
                MaterialIds = MaterialIds
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is CourseInputModel))
                return false;

            var courseObj = (CourseInputModel)obj;

            return (
                    Name.Equals(courseObj.Name) &&
                    Description.Equals(courseObj.Description) &&
                    Duration == courseObj.Duration);
        }

        public override string ToString()
        {
            return $" {Name} {Duration}";
        }
    }
}