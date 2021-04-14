using NUnit.Framework;
using RestSharp;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Mocks.InputModels;
using IntegrationTest.Mocks.OutputModels;
using System.Text.Json;
using System.Net;
using IntegrationTest.Models.OutputModels;
using System.Collections.Generic;
using Dapper;
using IntegrationTest.Models;
using RestSharp.Authenticators;
using IntegrationTest.Mocks;

namespace IntegrationTest
{
    public class CourseControllerTest: BaseTest
    {

        private RestClient _client;
        private Method _httpMethod;
        private RestRequest _request;
        private CourseInputModel _inputModel;
        private List<int> _CourseIdList;
        private string _token;

        [SetUp]
        public void Setup()
        {
            _client = new RestClient("https://localhost:44365/");
            _CourseIdList = new List<int>();
            _httpMethod = Method.POST;
            var authenticationInputModel = new AuthenticationInputModel { Login = appSettings.Login, Password = appSettings.Password };
            var authenticationRequest = new RestRequest("api/Authentication", _httpMethod);
            authenticationRequest.AddParameter("application/json", JsonSerializer.Serialize(authenticationInputModel), ParameterType.RequestBody);
            var authenticationResponse = _client.Execute<AuthResponse>(authenticationRequest);
            _token = authenticationResponse.Data.Token;
            _client.Authenticator = new JwtAuthenticator(_token);
        }

        [Test]
        public void AddCourse_ValidCourseInputModelSent_OkResponseGot_CourseExistsUnderId()
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(1).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _inputModel = (CourseInputModel)CourseMockGetter.GetCourseInputModelMock(1).Clone();
            _request = new RestRequest("api/Course", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);

            //When
            var response = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = response.StatusCode;
            var actualOutputModel = response.Data;
            _CourseIdList.Add(actualOutputModel.Id);


            //Then
            Assert.IsTrue(actualOutputModel.Id != 0);
            expectedOutputModel.Id = actualOutputModel.Id;
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TearDown]
        public void TearDown()
        {
            _CourseIdList.ForEach(Id =>
            {
                _connection.Execute("dbo.User_HardDelete", new { Id }, commandType: System.Data.CommandType.StoredProcedure);
            });
        }
    }
}
