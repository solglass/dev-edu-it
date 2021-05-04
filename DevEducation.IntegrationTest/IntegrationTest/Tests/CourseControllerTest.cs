using NUnit.Framework;
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
    public class CourseControllerTest
    {
        private IRestClient _client;
        private RestRequest _request;
        [SetUp]
        public void Setup()
        {
            _client = new RestClient(TestHelper.ApiUrl);
            _client.SetupClient();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void AddCourse_ValidCourseInputModelSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();

            var inputModel = CourseMockGetter.GetInputModel(mockId);

            _request = _client.FormPostRequest<CourseInputModel>(TestHelper.Course_Create, inputModel);

            //When
            var response = _client.Execute<CourseOutputModel>(_request);
            var actualOutputModel = response.Data;
            Assert.IsTrue(actualOutputModel.Id != 0);


            //Then
            expectedOutputModel.Id = actualOutputModel.Id;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(6)]
        public void AddCourse_EmptyCourseInputModelSent_ConflictResponseGot_MessageIsNotNull(int mockId)
        {
            //Given
            var expectedOutputModel = CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();

            var inputModel = CourseMockGetter.GetInputModel(mockId);

            _request = _client.FormPostRequest<CourseInputModel>(TestHelper.Course_Create, inputModel);

            //When
            var response = _client.Execute<string>(_request);
            var message = response.Data;


            //Then
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
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

            var inputModel = CourseMockGetter.GetInputModel(mockId);
            _request = _client.FormPostRequest(TestHelper.Course_Create, inputModel);
            var response = _client.Execute<CourseOutputModel>(_request);
            var addedOutputModel = response.Data;

            //When 
            _request = _client.FormGetRequest<CourseInputModel>($"{TestHelper.Course_Get}/{ addedOutputModel.Id}");
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then

            Assert.AreEqual(HttpStatusCode.OK, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [Test]
        public void GetCourse_InvalidCourseIdSent_NotFoundResponseGot_RecievedCourseModelIsNull()
        {
            //Given
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _request = _client.FormGetRequest<CourseInputModel>($"{TestHelper.Course_Get}/{TestHelper.Invalid_ID}");
            var responseGet = _client.Execute<string>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNotNull(message);
        }


        [TestCase(1)]
        public void UpdateCourse_ValidModelSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            var inputModel = CourseMockGetter.GetInputModel(mockId);
            _request = _client.FormPostRequest<CourseInputModel>(TestHelper.Course_Create, inputModel);
            var responsePost = _client.Execute<CourseOutputModel>(_request);
            var addedOutputModel = responsePost.Data;
            expectedOutputModel.Id = addedOutputModel.Id;


            inputModel.Name = "Updated Name";
            inputModel.Description = "Updated description";
            inputModel.Duration = 9;
            expectedOutputModel.Name = inputModel.Name;
            expectedOutputModel.Description = inputModel.Description;
            expectedOutputModel.Duration = inputModel.Duration;

            //When 
            _request = _client.FormPutRequest<CourseInputModel>($"{TestHelper.Course_Update}/{addedOutputModel.Id}", inputModel);
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

            var inputModel = CourseMockGetter.GetInputModel(mockId);

            _request = _client.FormPutRequest<CourseInputModel>($"{TestHelper.Course_Update}/{TestHelper.Invalid_ID}", inputModel);
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Content;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNotNull(message);
        }

        [TestCase(1)]
        public void DeleteCourse_ValidCourseId_OkResponseGot_RecievedExtendedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseExtendedOutputModel)CourseExtendedOutputModelMockGetter.GetCourseExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            var inputModel = CourseMockGetter.GetInputModel(mockId);

            _request = _client.FormPostRequest<CourseInputModel>(TestHelper.Course_Create, inputModel);
            var response = _client.Execute<CourseExtendedOutputModel>(_request);
            var addedOutputModel = response.Data;
            expectedOutputModel.Id = addedOutputModel.Id;
            expectedOutputModel.IsDeleted = true;



            //When 
            _request = _client.FormDeleteRequest<CourseInputModel>($"{ TestHelper.Course_Delete}/{addedOutputModel.Id}");
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
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _request = _client.FormDeleteRequest<CourseInputModel>($"{ TestHelper.Course_Delete}/{TestHelper.Invalid_ID}");
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
            var expectedOutputModel = (CourseExtendedOutputModel)CourseExtendedOutputModelMockGetter.GetCourseExtendedOutputModelMock(mockId).Clone();
            var inputModel = CourseMockGetter.GetInputModel(mockId);
            _request = _client.FormPostRequest<CourseInputModel>(TestHelper.Course_Create, inputModel);
            var response = _client.Execute<CourseExtendedOutputModel>(_request);
            var addedOutputModel = response.Data;

            _request = _client.FormDeleteRequest<CourseInputModel>($"{ TestHelper.Course_Delete}/{addedOutputModel.Id}");
            response = _client.Execute<CourseExtendedOutputModel>(_request);


            //When 
            _request = _client.FormPutRequest<CourseInputModel>($"{TestHelper.Course_Update}/{ addedOutputModel.Id}/{ TestHelper.Course_Recovery}");
            var responseGet = _client.Execute<CourseExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;


            //Then
            Assert.AreEqual(HttpStatusCode.OK, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(1)]
        public void RecoverCourse_InvalidCourseIdSent_NotFoundResponseGot_RecievedNull(int mockId)
        {
            //Given
            // var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _client.FormPutRequest<CourseInputModel>($"{ TestHelper.Course_Recovery}/{TestHelper.Invalid_ID}");
            var responseGet = _client.Execute<string>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Content;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNotNull(message);
        }
        /*
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


            _client.FormPostRequest<ThemeInputModel>(Method.POST, new ThemeMockGetter(), TestHelper.Theme_Create, mockId);


            //When
            var response = _client.Execute<ThemeExtendedOutputModel>(_request);
            var actualStatusCode = response.StatusCode;
            var actualOutputModel = response.Data;
            Assert.IsTrue(actualOutputModel.Id != 0);


            //Then
            expectedOutputModel.Id = actualOutputModel.Id;
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(6)]
        public void AddTheme_EmptyThemeInputModelSent_ConflictResponseGot_RecievedErrorMessageIsNotNull(int mockId)
        {
            //Given
            var expectedOutputModel = (ThemeExtendedOutputModel)ThemeExtendedOutputModelMockGetter.GetThemeExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.Conflict;

            _client.FormPostRequest<ThemeInputModel>(Method.POST, new ThemeMockGetter(), TestHelper.Theme_Create, mockId);

            //When
            var response = _client.Execute<string>(_request);
            var actualStatusCode = response.StatusCode;
            var message = response.Data;


            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNotNull(message);
        }

        [TestCase(1)]
        public void GetTheme_ValidIdSent_OkResponseGot_RecievedThemeModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (ThemeExtendedOutputModel)ThemeExtendedOutputModelMockGetter.GetThemeExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _client.FormPostRequest<ThemeInputModel>(Method.POST, new ThemeMockGetter(), TestHelper.Theme_Create, mockId);

            var response = _client.Execute<ThemeExtendedOutputModel>(_request);
            var addedOutputModel = response.Data;
            Assert.IsTrue(addedOutputModel.Id != 0);

            //When
            _client.FormPostRequest<ThemeExtendedOutputModel>(Method.GET, new ThemeMockGetter(), $"{TestHelper.Theme_Get}/{addedOutputModel.Id}");

            var responseGet = _client.Execute<ThemeExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            expectedOutputModel.Id = actualOutputModel.Id;
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [Test]
        public void GetTheme_InvalidThemeIdSent_NotFoundResponseGot_RecievedErrorMessageNotNull()
        {
            //Given
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _client.FormPostRequest<ThemeInputModel>(Method.GET, new ThemeMockGetter(), $"{ TestHelper.Theme_Get}/{TestHelper.Invalid_ID}"); ;
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Content;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNotNull(message);
        }

        [TestCase(1)]
        public void DeleteTheme_ValidThemeId_OkResponseGot_RecievedExtendedThemeModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (ThemeExtendedOutputModel)ThemeExtendedOutputModelMockGetter.GetThemeExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _client.FormPostRequest<ThemeInputModel>(Method.POST, new ThemeMockGetter(), TestHelper.Theme_Create, mockId);
            var response = _client.Execute<ThemeExtendedOutputModel>(_request);
            var addedOutputModel = response.Data;
            expectedOutputModel.Id = addedOutputModel.Id;
            expectedOutputModel.IsDeleted = true;



            //When 
            _client.FormPostRequest<ThemeInputModel>(Method.DELETE, new ThemeMockGetter(), $"{ TestHelper.Theme_Delete}/{ addedOutputModel.Id}");
            HttpMethod = Method.DELETE;
            var responseGet = _client.Execute<ThemeExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(1)]
        public void DeleteTheme_InvalidThemeIdSent_NotFoundResponseGot_RecievedNull(int mockId)
        {
            //Given
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _client.FormPostRequest<ThemeInputModel>(Method.DELETE, new ThemeMockGetter(), $"{ TestHelper.Theme_Delete}/{TestHelper.Invalid_ID}");
            var responseGet = _client.Execute<ThemeExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNull(actualOutputModel);
        }

        [TestCase(1)]
        public void RecoverTheme_ValidThemeId_OkResponseGot_RecievedExtendedThemeModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (ThemeExtendedOutputModel)ThemeExtendedOutputModelMockGetter.GetThemeExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _client.FormPostRequest<ThemeInputModel>(Method.POST, new ThemeMockGetter(), TestHelper.Theme_Recovery, mockId);
            var response = _client.Execute<ThemeExtendedOutputModel>(_request);
            var addedOutputModel = response.Data;
            var responsePost = _client.Execute<ThemeExtendedOutputModel>(_request);
            expectedOutputModel.Id = addedOutputModel.Id;


            //When 
            _client.FormPostRequest<ThemeInputModel>(Method.PUT, new ThemeMockGetter(), $"{ TestHelper.Theme_Recovery}/{ addedOutputModel.Id}", inputModel: addedOutputModel);
            var responseGet = _client.Execute<ThemeExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;


            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(1)]
        public void RecoverTheme_InvalidThemeIdSent_NotFoundResponseGot_RecievedNull(int mockId)
        {
            //Given
            // var expectedOutputModel = (ThemeOutputModel)ThemeOutputModelMockGetter.GetThemeOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.NotFound;


            //When 
            _client.FormPostRequest<ThemeInputModel>(Method.PUT, new ThemeMockGetter(), $"{ TestHelper.Theme_Recovery}/{TestHelper.Invalid_ID}");
            var responseGet = _client.Execute<string>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Content;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNotNull(message);
        }
        */

        [TearDown]
        public void TearDown()
        {
            BaseTest.Connection.Execute("dbo.CleanDatabase", commandType: CommandType.StoredProcedure);
        }

    }
}
