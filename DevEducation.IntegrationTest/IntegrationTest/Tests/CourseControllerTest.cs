﻿using NUnit.Framework;
using RestSharp;
using IntegrationTest.Models.InputModels;
using System.Net;
using IntegrationTest.Models.OutputModels;
using System.Collections.Generic;
using Dapper;
using IntegrationTest.Mocks;
using System.Data;

namespace IntegrationTest
{
    public class CourseControllerTest: BaseTest
    {

        [SetUp]
        public void Setup()
        {
            SetupClient();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void AddCourse_ValidCourseInputModelSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _request = FormRequest<CourseInputModel>(Method.POST, new CourseMockGetter(), TestHelper.Course_Create, mockId);

            //When
            var response = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = response.StatusCode;
            var actualOutputModel = response.Data;
            Assert.IsTrue(actualOutputModel.Id != 0);


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

            FormRequest<CourseInputModel>(Method.POST, new CourseMockGetter(), TestHelper.Course_Create, mockId);

            //When
           var response = _client.Execute<string>(_request);
           var actualStatusCode = response.StatusCode;
           var message = response.Data;


            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNotNull(message);
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

            FormRequest<CourseInputModel>(Method.POST, new CourseMockGetter(), TestHelper.Course_Create, mockId);
            var response = _client.Execute<CourseOutputModel>(_request);
            var addedOutputModel = response.Data;
            Assert.IsTrue(addedOutputModel.Id != 0);

            //When 
            FormRequest<CourseInputModel>(Method.GET, new CourseMockGetter(),
              $"{TestHelper.Course_Get}/{ addedOutputModel.Id}");
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
            _request = new RestRequest($"api/Course/{TestHelper.Invalid_ID}", _httpMethod);
            var responseGet = _client.Execute<string>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            //Assert.IsNotNull(message);
        }

        
        [TestCase(1)]
        public void UpdateCourse_ValidModelSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            FormRequest<CourseInputModel>(Method.POST, new CourseMockGetter(), TestHelper.Course_Create, mockId);
            var responsePost = _client.Execute<CourseOutputModel>(_request);
            var addedOutputModel = responsePost.Data;
            expectedOutputModel.Id = addedOutputModel.Id;


            _inputModel.Name = "Updated Name";
            _inputModel.Description = "Updated description";
            _inputModel.Duration = 9;
            expectedOutputModel.Name = _inputModel.Name;
            expectedOutputModel.Description = _inputModel.Description;
            expectedOutputModel.Duration = _inputModel.Duration;

            //When 
            FormRequest<CourseInputModel>(Method.PUT, new CourseMockGetter(), $"{TestHelper.Course_Update}/{addedOutputModel.Id}", inputModel: _inputModel);
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

            var expectedStatusCode = HttpStatusCode.NotFound;



            FormRequest<CourseInputModel>(Method.PUT, new CourseMockGetter(), $"{TestHelper.Course_Update}/{TestHelper.Invalid_ID}",  mockId);
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            //Assert.IsNull(actualOutputModel);
        }
        /*
        [TestCase(1)]
        public void DeleteCourse_ValidCourseId_OkResponseGot_RecievedExtendedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseExtendedOutputModel)CourseExtendedOutputModelGetter.GetCourseExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _inputModel = (CourseInputModel)CourseMockGetter.GetInputModel(mockId).Clone();
            _request = new RestRequest("api/Course", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);
            var responsePost = _client.Execute<CourseExtendedOutputModel>(_request);
            var addedOutputModel = responsePost.Data;
            expectedOutputModel.Id = addedOutputModel.Id;
            expectedOutputModel.IsDeleted = true;
            _courseIdList.Add(expectedOutputModel.Id);


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
            _httpMethod = Method.DELETE;
            _request = new RestRequest("api/Course/0", _httpMethod);
            var responseGet = _client.Execute<CourseExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNull(actualOutputModel);
        }
       /*
        [TestCase(1)]
        public void RecoverCourse_ValidCourseId_OkResponseGot_RecievedExtendedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseExtendedOutputModel)CourseExtendedOutputModelGetter.GetCourseExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _inputModel = (CourseInputModel)CourseMockGetter.GetInputModel(mockId).Clone();
            _request = new RestRequest("api/Course", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_inputModel), ParameterType.RequestBody);
            var responsePost = _client.Execute<CourseExtendedOutputModel>(_request);
            var addedOutputModel = responsePost.Data;
            expectedOutputModel.Id = addedOutputModel.Id;
            expectedOutputModel.IsDeleted = false;
            _courseIdList.Add(expectedOutputModel.Id);


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

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void AddTheme_ValidThemeInputModelSent_OkResponseGot_RecievedThemeModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (ThemeExtendedOutputModel)ThemeExtendedOutputModelMockGetter.GetThemeExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _themeInputModel = (ThemeInputModel)ThemeMockGetter.GetThemeInputModelMock(mockId).Clone();
            _request = new RestRequest("api/Course/theme", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_themeInputModel), ParameterType.RequestBody);

            //When
            var response = _client.Execute<ThemeExtendedOutputModel>(_request);
            var actualStatusCode = response.StatusCode;
            var actualOutputModel = response.Data;
            Assert.IsTrue(actualOutputModel.Id != 0);
            _themeIdList.Add(actualOutputModel.Id);


            //Then
            expectedOutputModel.Id = actualOutputModel.Id;
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(6)]
        public void AddTheme_EmptyThemeInputModelSent_ConflictResponseGot_RecievedThemeModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (ThemeExtendedOutputModel)ThemeExtendedOutputModelMockGetter.GetThemeExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.Conflict;

            _httpMethod = Method.POST;
            _themeInputModel = (ThemeInputModel)ThemeMockGetter.GetThemeInputModelMock(mockId).Clone();
            _request = new RestRequest("api/Course/theme", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_themeInputModel), ParameterType.RequestBody);

            //When
            var response = _client.Execute<ThemeExtendedOutputModel>(_request);
            var actualStatusCode = response.StatusCode;
            var actualOutputModel = response.Data;


            //Then
            Assert.IsTrue(actualOutputModel.Id == 0);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(1)]
        public void GetTheme_ValidIdSent_OkResponseGot_RecievedThemeModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (ThemeExtendedOutputModel)ThemeExtendedOutputModelMockGetter.GetThemeExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _httpMethod = Method.POST;
            _themeInputModel = (ThemeInputModel)ThemeMockGetter.GetThemeInputModelMock(mockId).Clone();
            _request = new RestRequest("api/Course/theme", _httpMethod);
            _request.AddParameter("application/json", JsonSerializer.Serialize(_themeInputModel), ParameterType.RequestBody);


            var response = _client.Execute<ThemeExtendedOutputModel>(_request);
            var addedOutputModel = response.Data;
            Assert.IsTrue(addedOutputModel.Id != 0);
            _themeIdList.Add(addedOutputModel.Id);

            //When
            _httpMethod = Method.GET;
            _request = new RestRequest($"api/Course/theme/{addedOutputModel.Id}", _httpMethod);
            var responseGet = _client.Execute<ThemeExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            expectedOutputModel.Id = actualOutputModel.Id;
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [Test]
        public void GetCourse_InvalidThemeIdSent_NotFoundResponseGot_RecievedCourseModelIsNull()
        {
            //Given
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _httpMethod = Method.GET;
            _request = new RestRequest("api/Course/theme/0", _httpMethod);
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNull(actualOutputModel);
        }
        */




        [TearDown]
        public void TearDown()
        {
            _connection.Execute("dbo.CleanDatabase", commandType: CommandType.StoredProcedure);
        }
    }
}