using System;
using System.Collections.Generic;
using IntegrationTest.Models.InputModels;
namespace IntegrationTest.Mocks.InputModels
{
    public static class UserMockGetter 
    {
        public static UserInputModel GetInputModel(int mockId)
        {

            UserInputModel UserInputModel = mockId switch
            {
                1 => new UserInputModel()
                {
                    Email = "ololosh@mail.ru",
                    FirstName = "Ololosh",
                    BirthDate = "25.09.1995",
                    LastName = "Horoshiy",
                    Password = "1234567",
                    Phone = "123123123",
                    UserPic = " 21",
                    Login = "ololosha",
                    Roles = new List<int> { 1 }
                },

                2 => new UserInputModel()
                {
                    Email = "Use1r15@mail.ru",
                    FirstName = "Antonio",
                    BirthDate = "25.09.1995",
                    LastName = "Negodny",
                    Password = "1234567",
                    Phone = "9999999999",
                    UserPic = " 22",
                    Login = "AN7123"
                },

                3 => new UserInputModel()
                {
                    Email = "vasyarulit@mail.ru",
                    FirstName = "Vasek",
                    BirthDate = "25.09.1995",
                    LastName = "Pupkin",
                    Password = "1234567",
                    Phone = "999977777",
                    UserPic = " 23",
                    Login = "vasya",
                },
                4 => new UserInputModel
                {
                    Email = "Case4444@mail.ru",
                    FirstName = "Anton",
                    BirthDate = "25.09.1995",
                    LastName = "Negodyaj",
                    Password = "1234567",
                    Phone = "4448444444",
                    UserPic = "00",
                    Login = "Case4444Login"
                },
                5 => new UserInputModel
                {
                    Email = "Case55555@mail.ru",
                    FirstName = "Anton",
                    BirthDate = "25.09.1995",
                    LastName = "Negodyaj",
                    Password = "1234567",
                    Phone = "5555595555",
                    UserPic = "00",
                    Login = "Case55555Login"
                },
                6 => new UserInputModel
                {
                    Email = "DELETED@mail.ru",
                    FirstName = "Anton",
                    BirthDate = "25.09.1995",
                    LastName = "Negodyaj",
                    Password = "1234567",
                    Phone = "5555595555",
                    UserPic = "00",
                    Login = "DELETEDLogin"
                },
                7 => new UserInputModel(),
                _ => throw new NotImplementedException()
            };

            return UserInputModel;
        }

    }
}
