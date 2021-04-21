using System;
using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class MaterialOutputModel
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

        public virtual object Clone()
        {
            return new MaterialOutputModel()
            {
                Id = Id,
                Link = Link,
                Description = Description
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is MaterialOutputModel))
                return false;

            var MaterialObj = (MaterialOutputModel)obj;

            return (Id == MaterialObj.Id &&
                    ((string.IsNullOrEmpty(MaterialObj.Link) && string.IsNullOrEmpty(Link)) ||
                    Link.Equals(MaterialObj.Link))&&
                    ((string.IsNullOrEmpty(MaterialObj.Description) && string.IsNullOrEmpty(Description)) ||
                    Description.Equals(MaterialObj.Description)));
        }

        public override string ToString()
        {
            return $"{Id} {Link}";
        }
    }
}
