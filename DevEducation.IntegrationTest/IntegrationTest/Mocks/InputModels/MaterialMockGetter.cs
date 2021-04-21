using System;
using System.Collections.Generic;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Models.OutputModels;

namespace IntegrationTest.Mocks
{
    public static class MaterialMockGetter
    {
        public static MaterialInputModel GetMaterialInputModelMock(int caseId)
        {
            return caseId switch
            {
                0 => new MaterialInputModel(),
                1 => new MaterialInputModel
                {
                    Description = "Description test",
                    Link = "Link test1"
                },

                2 => new MaterialInputModel
                {
                    Description = "Description test2",
                    Link = "Link test2"
                },

                3 => new MaterialInputModel
                {
                    Description = "Description test3",
                    Link = "Link test3"
                },
                4 => new MaterialInputModel
                {
                    Description = "Description test2"
                },

                5 => new MaterialInputModel
                {
                    Link = "Link test3"
                },
                6 => new MaterialInputModel(),
                _ => null,
            };
        }
    }
}