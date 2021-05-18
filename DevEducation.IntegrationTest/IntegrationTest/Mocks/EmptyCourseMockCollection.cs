using IntegrationTest.Models.InputModels;
using IntegrationTest.Models.OutputModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest.Mocks
{
    public class EmptyCourseMockCollection: IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { new CourseExtendedOutputModel(), new CourseInputModel() };
        }
    }
}
