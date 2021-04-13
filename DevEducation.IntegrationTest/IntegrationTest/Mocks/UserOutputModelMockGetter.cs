using System;
using System.Collections.Generic;
using IntegrationTest.Models.OutputModels;
namespace IntegrationTest.Mocks
{
    public static class UserOutputModelMockGetter
    {
        public static UserOutputModel GetUserOutputModelMock(int mockId)
        {

            UserOutputModel UserOutputModel = mockId switch
            {
                1 => new UserOutputModel()
                {
                    Email = "ololosh@mail.ru",
                    FirstName = "Ololosh",
                    BirthDate = "25.09.1995",
                    LastName = "Horoshiy",
                    Phone = "123123123",
                    UserPic = " 21",
                    Login = "ololosha",
                    Roles = new List<int> { 1 }
                },

                2 => new UserOutputModel()
                {
                    Email = "Use1r15@mail.ru",
                    FirstName = "Antonio",
                    BirthDate = "25.09.1995",
                    LastName = "Negodny",
                    Phone = "9999999999",
                    UserPic = " 22",
                    Login = "AN7123"
                },

                3 => new UserOutputModel()
                {
                    Email = "vasyarulit@mail.ru",
                    FirstName = "Vasek",
                    BirthDate = "25.09.1995",
                    LastName = "Pupkin",
                    Phone = "999977777",
                    UserPic = " 23",
                    Login = "vasya",
                },
                4 => new UserOutputModel
                {
                    Email = "Case4444@mail.ru",
                    FirstName = "Anton",
                    BirthDate = "25.09.1995",
                    LastName = "Negodyaj",
                    Phone = "4448444444",
                    UserPic = "00",
                    Login = "Case4444Login"
                },
                5 => new UserOutputModel
                {
                    Email = "Case55555@mail.ru",
                    FirstName = "Anton",
                    BirthDate = "25.09.1995",
                    LastName = "Negodyaj",
                    Phone = "5555595555",
                    UserPic = "00",
                    Login = "Case55555Login"
                },
                6 => new UserOutputModel
                {
                    Email = "DELETED@mail.ru",
                    FirstName = "Anton",
                    BirthDate = "25.09.1995",
                    LastName = "Negodyaj",
                    Phone = "5555595555",
                    UserPic = "00",
                    Login = "DELETEDLogin"
                },
                7 => new UserOutputModel(),
                _ => throw new NotImplementedException()
            };

            return UserOutputModel;
        }

    }
}
