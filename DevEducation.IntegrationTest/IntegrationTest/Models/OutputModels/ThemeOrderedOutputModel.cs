using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest.Models.OutputModels
{
    public class ThemeOrderedOutputModel : ThemeOutputModel
    {
        public int Order { get; set; }

        public override bool Equals(object obj)
        {
            var ThemeObj = obj as ThemeOrderedOutputModel;
            if (ThemeObj == null)
                return false;
            else
            return (Id == ThemeObj.Id &&
                    Order == ThemeObj.Order);
        }
    }
}
