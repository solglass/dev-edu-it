using System;
using System.Collections.Generic;
using IntegrationTest.Mocks.InputModels;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Models.OutputModels;

namespace IntegrationTest.Mocks
{
    public static ThemeInputModel ThemeMockGetter 
    {
        public object GetInputModel(int mockId)
        {
            return mockId switch
            {
                1 => new ThemeInputModel
                {
                    Name = "Test theme 1"
                },
                2 => new ThemeInputModel
                {
                    Name = "Test theme 2"
                },
                3 => new ThemeInputModel
                {
                    Name = "Test theme 3"
                },
                4 => new ThemeInputModel
                {
                    Name = "Test theme 4"
                },
                5 => new ThemeInputModel
                {
                    Name = "Test theme 5"
                },
                6 =>new ThemeInputModel(),
                _ => null,
            };
        }
    }
}
