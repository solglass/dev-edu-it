using NUnit.Framework;
using RestSharp;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Mocks;
using System.Text.Json;
using System.Net;
using IntegrationTest.Models.OutputModels;
using System.Collections.Generic;
using Dapper;

namespace IntegrationTest
{
    public class UserControllerTest:BaseTest
    {
        private RestClient _client;
        private Method _httpMethod;
        private RestRequest _request;
        private UserInputModel _inputModel;

        [SetUp]
        public void Setup()
        {
            _client = new RestClient("https://localhost:44365/");
        }
     
        [Test]
        public void RegistrationPass_ValidUserInputModelSent_OkResponseGot_UserExistsUnderId()
        {
            //Given
            _httpMethod = Method.POST;
            _request = new RestRequest("api/user/register", _httpMethod);
            _inputModel = (UserInputModel)UserInputModelMockGetter.GetUserInputModelMock(1).Clone();
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);
            var response = _client.Execute<UserOutputModel>(_request);

            //When
            var expected = new UserOutputModel
            {
                Email = "ololosh@mail.ru",
                FirstName = "Ololosh",
                BirthDate = "25.09.1995",
                LastName = "Horoshiy",
                Phone = "123123123",
                UserPic = " 21",
                Login = "ololosha",
                Roles = new List<int> { 1 }
            };

            //Then
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(expected, response.Data);
        }

        [TearDown]
        public void TearDown()
        {
            FUCKING_NAPALM_DELETE();
        }
        public void FUCKING_NAPALM_DELETE()
        {
           //_connection.Execute("dbo.CleanDatabase",
           //     commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}