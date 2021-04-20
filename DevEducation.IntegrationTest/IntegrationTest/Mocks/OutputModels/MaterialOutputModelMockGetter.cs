using System;
using System.Collections.Generic;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Models.OutputModels;

namespace IntegrationTest.Mocks
{
    public static class MaterialOutputModelMockGetter
    {
        public static MaterialOutputModel GetMaterialOutputModelMock(int caseId)
        {
            return caseId switch
            {
                0 => new MaterialOutputModel(),
                1 => new MaterialOutputModel
                {
                    Description = "Description test",
                    Link = "Link test1"
                },

                2 => new MaterialOutputModel
                {
                    Description = "Description test2",
                    Link = "Link test2"
                },

                3 => new MaterialOutputModel
                {
                    Description = "Description test3",
                    Link = "Link test3"
                },
                4 => new MaterialOutputModel
                {
                    Description = "Description test2"
                },

                5 => new MaterialOutputModel
                {
                    Link = "Link test3"
                },
                _ => null,
            };
        }
    }
}