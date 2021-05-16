using System;

namespace IntegrationTest.Models.OutputModels
{
    public class CourseExtendedOutputModel: CourseOutputModel
    {
        public bool IsDeleted { get; set; }

        public override object Clone()
        {
            return new CourseExtendedOutputModel()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Duration = Duration,
                Materials = Materials,
                IsDeleted = IsDeleted
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is CourseExtendedOutputModel))
                return false;

            var courseObj = (CourseExtendedOutputModel)obj;

            return (Id == courseObj.Id &&
                    ((string.IsNullOrEmpty(courseObj.Name) && string.IsNullOrEmpty(Name)) ||
                    Name.Equals(courseObj.Name)) &&
                    (string.IsNullOrEmpty(courseObj.Description) &&
                    string.IsNullOrEmpty(Description)) ||
                    Description.Equals(courseObj.Description) &&
                    Duration == courseObj.Duration)&&IsDeleted == courseObj.IsDeleted;
        }

        public override string ToString()
        {
            return $"{Id} {Name} {Duration} is deleted: {IsDeleted}";
        }
    }
}
