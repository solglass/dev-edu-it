using System;
using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class CourseOutputModel: ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public List<MaterialOutputModel> Materials { get; set; }

        public virtual object Clone()
        {
            return new CourseOutputModel()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Duration = Duration,
                Materials = Materials
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is CourseOutputModel))
                return false;

            var courseObj = (CourseOutputModel)obj;

            return (Id == courseObj.Id &&
                    ( (string.IsNullOrEmpty(courseObj.Name) && string.IsNullOrEmpty(Name)) ||
                    Name.Equals(courseObj.Name) )  && 
                    (string.IsNullOrEmpty(courseObj.Description) &&
                    string.IsNullOrEmpty(Description)) ||
                    Description.Equals(courseObj.Description) &&
                    Duration == courseObj.Duration);
        }

        public override string ToString()
        {
            return $"{Id} {Name} {Duration}";
        }
    }
}
