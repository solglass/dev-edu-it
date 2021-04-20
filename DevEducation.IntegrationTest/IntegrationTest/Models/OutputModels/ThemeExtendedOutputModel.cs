using System;

namespace IntegrationTest.Models.OutputModels
{
    public class ThemeExtendedOutputModel:ThemeOutputModel
    {
        public bool IsDeleted { get; set; }

        public override object Clone()
        {
            return new ThemeExtendedOutputModel()
            {
                Id = Id,
                Name = Name,
                IsDeleted = IsDeleted
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ThemeExtendedOutputModel))
                return false;

            var ThemeObj = (ThemeExtendedOutputModel)obj;

            return (Id == ThemeObj.Id &&
                    ((string.IsNullOrEmpty(ThemeObj.Name) && string.IsNullOrEmpty(Name)) ||
                    Name.Equals(ThemeObj.Name)) && IsDeleted == ThemeObj.IsDeleted);
        }

        public override string ToString()
        {
            return $"{Id} {Name} is deleted: {IsDeleted}";
        }
    }
}
