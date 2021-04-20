using System;
using System.Collections.Generic;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Models.OutputModels;

namespace IntegrationTest.Mocks
{
    public static class ThemeExtendedOutputModelMockGetter
    {
        public static ThemeExtendedOutputModel GetThemeExtendedOutputModelMock(int id)
        {
            return id switch
            {
                1 => new ThemeExtendedOutputModel
                {
                    Name = "Test theme 1"
                },
                2 => new ThemeExtendedOutputModel
                {
                    Name = "Test theme 2"
                },
                3 => new ThemeExtendedOutputModel
                {
                    Name = "Test theme 3"
                },
                4 => new ThemeExtendedOutputModel
                {
                    Name = "Test theme 4"
                },
                5 => new ThemeExtendedOutputModel
                {
                    Name = "Test theme 5"
                },
                6=>new ThemeExtendedOutputModel(),
                _ => null,
            };
        }
    }
}
