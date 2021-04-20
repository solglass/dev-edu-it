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

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void AddCourse_ValidCourseInputModelSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _inputModel = (CourseInputModel)CourseMockGetter.GetCourseInputModelMock(mockId).Clone();
            _request = new RestRequest("api/Course", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);

            //When
            var response = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = response.StatusCode;
            var actualOutputModel = response.Data;
            Assert.IsTrue(actualOutputModel.Id != 0);
            _CourseIdList.Add(actualOutputModel.Id);


            //Then
            expectedOutputModel.Id = actualOutputModel.Id;
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(6)]
        public void AddCourse_EmptyCourseInputModelSent_ConflictResponseGot_RecievedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
           var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.Conflict;

            _httpMethod = Method.POST;
            _inputModel = (CourseInputModel)CourseMockGetter.GetCourseInputModelMock(mockId).Clone();
            _request = new RestRequest("api/Course", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);

            //When
            var response = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = response.StatusCode;
            var actualOutputModel = response.Data;


            //Then
            Assert.IsTrue(actualOutputModel.Id == 0);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void GetCourse_ValidCourseIdSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _inputModel = (CourseInputModel)CourseMockGetter.GetCourseInputModelMock(mockId).Clone();
            _request = new RestRequest("api/Course", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);
            var responsePost = _client.Execute<CourseOutputModel>(_request);
            var addedOutputModel = responsePost.Data;
            expectedOutputModel.Id = addedOutputModel.Id;
            _CourseIdList.Add(expectedOutputModel.Id);


            //When 
            _httpMethod = Method.GET;
            _request = new RestRequest($"api/Course/{addedOutputModel.Id}", _httpMethod);
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [Test]
        public void GetCourse_InvalidCourseIdSent_NotFoundResponseGot_RecievedCourseModelIsNull()
        {
            //Given
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _httpMethod = Method.GET;
            _request = new RestRequest("api/Course/0", _httpMethod);
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNull(actualOutputModel);
        }


        [TestCase(1)]
        public void UpdateCourse_ValidModelSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _inputModel = (CourseInputModel)CourseMockGetter.GetCourseInputModelMock(mockId).Clone();
            _request = new RestRequest("api/Course", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);
            var responsePost = _client.Execute<CourseOutputModel>(_request);
            var addedOutputModel = responsePost.Data;
            expectedOutputModel.Id = addedOutputModel.Id;
            _CourseIdList.Add(expectedOutputModel.Id);

            _inputModel.Name = "Updated Name";
            _inputModel.Description = "Updated description";
            _inputModel.Duration = 9;
            expectedOutputModel.Name = _inputModel.Name;
            expectedOutputModel.Description = _inputModel.Description;
            expectedOutputModel.Duration = _inputModel.Duration;

            //When 
            _httpMethod = Method.PUT;
            _request = new RestRequest($"api/Course/{addedOutputModel.Id}", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(6)]
        public void UpdateCourse_InvalidCourseIdSentEmptyModelSent_NotFoundResponseGot_RecievedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.NotFound;
            _inputModel = (CourseInputModel)CourseMockGetter.GetCourseInputModelMock(mockId).Clone();


            //When 
            _httpMethod = Method.PUT; ;
            _request = new RestRequest("api/Course/0", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNull(actualOutputModel);
        }

        [TestCase(1)]
        public void DeleteCourse_ValidCourseId_OkResponseGot_RecievedExtendedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseExtendedOutputModel)CourseExtendedOutputModelGetter.GetCourseExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _inputModel = (CourseInputModel)CourseMockGetter.GetCourseInputModelMock(mockId).Clone();
            _request = new RestRequest("api/Course", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);
            var responsePost = _client.Execute<CourseExtendedOutputModel>(_request);
            var addedOutputModel = responsePost.Data;
            expectedOutputModel.Id = addedOutputModel.Id;
            expectedOutputModel.IsDeleted = true;
            _CourseIdList.Add(expectedOutputModel.Id);


            //When 
            _httpMethod = Method.DELETE;
            _request = new RestRequest($"api/Course/{addedOutputModel.Id}", _httpMethod);
            var responseGet = _client.Execute<CourseExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(1)]
        public void DeleteCourse_InvalidCourseIdSent_NotFoundResponseGot_RecievedNull(int mockId)
        {
            //Given
           // var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _httpMethod = Method.DELETE ;
            _request = new RestRequest("api/Course/0", _httpMethod);
            var responseGet = _client.Execute<CourseExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNull(actualOutputModel);
        }

        [TestCase(1)]
        public void RecoverCourse_ValidCourseId_OkResponseGot_RecievedExtendedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseExtendedOutputModel)CourseExtendedOutputModelGetter.GetCourseExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _inputModel = (CourseInputModel)CourseMockGetter.GetCourseInputModelMock(mockId).Clone();
            _request = new RestRequest("api/Course", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);
            var responsePost = _client.Execute<CourseExtendedOutputModel>(_request);
            var addedOutputModel = responsePost.Data;
            expectedOutputModel.Id = addedOutputModel.Id;
            expectedOutputModel.IsDeleted = false;
            _CourseIdList.Add(expectedOutputModel.Id);


            _httpMethod = Method.DELETE;
            _request = new RestRequest($"api/Course/{addedOutputModel.Id}", _httpMethod);

            //When 
            _httpMethod = Method.PUT;
            _request = new RestRequest($"api/Course/{addedOutputModel.Id}/recovery", _httpMethod);
            var responseGet = _client.Execute<CourseExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;



            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(1)]
        public void RecoverCourse_InvalidCourseIdSent_NotFoundResponseGot_RecievedNull(int mockId)
        {
            //Given
            // var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _httpMethod = Method.PUT;
            _request = new RestRequest("api/Course/0/recovery", _httpMethod);
            var responseGet = _client.Execute<CourseExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNull(actualOutputModel);
        }



        [TearDown]
        public void TearDown()
        {
            _CourseIdList.ForEach(Id =>
            {
                _connection.Execute("dbo.Course_HardDelete", new { Id }, commandType: System.Data.CommandType.StoredProcedure);
            });
        }
    }
}
