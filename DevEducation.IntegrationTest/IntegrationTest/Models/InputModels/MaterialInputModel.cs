namespace IntegrationTest.Models.InputModels
{
    public class MaterialInputModel
    {
        public string Link { get; set; }
        public string Description { get; set; }

        public object Clone()
        {
            return new MaterialInputModel()
            {
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
            if (obj == null || !(obj is MaterialInputModel))
                return false;

            var MaterialObj = (MaterialInputModel)obj;

            return (
                    Link.Equals(MaterialObj.Link) && 
                    Description.Equals(MaterialObj.Description));
        }

        public override string ToString()
        {
            return $" {Link} {Description}";
        }
    }
}
