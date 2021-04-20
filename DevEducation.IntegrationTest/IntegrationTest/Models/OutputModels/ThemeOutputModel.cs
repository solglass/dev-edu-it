using System;
using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class ThemeOutputModel : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TagOutputModel> Tags { get; set; }

        public virtual object Clone()
        {
            return new ThemeOutputModel()
            {
                Id = Id,
                Name = Name,
                Tags = Tags
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ThemeOutputModel))
                return false;

            var ThemeObj = (ThemeOutputModel)obj;

            return (Id == ThemeObj.Id &&
                    ((string.IsNullOrEmpty(ThemeObj.Name) && string.IsNullOrEmpty(Name)) ||
                    Name.Equals(ThemeObj.Name)));
        }

        public override string ToString()
        {
            return $"{Id} {Name} ";
        }
    }
}
