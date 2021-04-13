using NUnit.Framework;
using RestSharp;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Mocks;
using System.Text.Json;
using System.Net;
using IntegrationTest.Models.OutputModels;
using System.Collections.Generic;
using Dapper;
using IntegrationTest.Models;
using RestSharp.Authenticators;

namespace IntegrationTest
{
    public class UserControllerTest:BaseTest
    {
        private RestClient _client;
        private Method _httpMethod;
        private RestRequest _request;
        private UserInputModel _inputModel;
        private List<int> _userIdList;
        private string _token;

        [SetUp]
        public void Setup()
        {
            _client = new RestClient("https://localhost:44365/");
            _userIdList = new List<int>();
            _httpMethod = Method.POST;
            var authenticationInputModel = new AuthenticationInputModel { Login = "volodya22", Password = "qwe!@#" };
            var authenticationRequest = new RestRequest("api/Authentication", _httpMethod);
            authenticationRequest.AddParameter("application/json", JsonSerializer.Serialize(authenticationInputModel), ParameterType.RequestBody);
            var authenticationResponse = _client.Execute<AuthResponse>(authenticationRequest);
            _token = authenticationResponse.Data.Token;
            _client.Authenticator = new JwtAuthenticator(_token);
        }
     
        [Test]
        public void RegistrationPass_ValidUserInputModelSent_OkResponseGot_UserExistsUnderId()
        {
            //Given
            var expectedOutputModel = (UserOutputModel)UserOutputModelMockGetter.GetUserOutputModelMock(1).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _inputModel = (UserInputModel)UserInputModelMockGetter.GetUserInputModelMock(1).Clone();
            _request = new RestRequest("api/user/register", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);

            //When
            var response = _client.Execute<UserOutputModel>(_request);
            var actualStatusCode = response.StatusCode;
            var actualOutputModel = response.Data;
            _userIdList.Add(actualOutputModel.Id);


            //Then
            Assert.IsTrue(actualOutputModel.Id != 0);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteAllRoleFromUser();
            DeleteUser();
        }

        private void DeleteAllRoleFromUser()
        {
            foreach (var userId in _userIdList)
            {
                _connection.Execute("dbo.User_Role_DeleteAll", new { userId }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        private void DeleteUser()
        {
            foreach (var Id in _userIdList)
            {
                _connection.Execute("dbo.User_HardDelete", new { Id }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}