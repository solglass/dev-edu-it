using System;
using System.Collections.Generic;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Models.OutputModels;

namespace IntegrationTest.Mocks
{
    public static class ThemeOutputModelMockGetter
    {
        public static ThemeOutputModel GetThemeOutputModelMock(int id)
        {
            return id switch
            {
                1 => new ThemeOutputModel
                {
                    Name = "Test theme 1"
                },
                2 => new ThemeOutputModel
                {
                    Name = "Test theme 2"
                },
                3 => new ThemeOutputModel
                {
                    Name = "Test theme 3"
                },
                4 => new ThemeOutputModel
                {
                    Name = "Test theme 4"
                },
                5 => new ThemeOutputModel
                {
                    Name = "Test theme 5"
                },
                6 => new ThemeOutputModel
                {
                    Name = "Test theme 6"
                },
                7=>new ThemeOutputModel(),
                _ => null,
            };
        }
    }
}
