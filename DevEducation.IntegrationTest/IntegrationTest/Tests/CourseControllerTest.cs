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

            var inputModel = (CourseInputModel)CourseMockGetter.GetInputModel(mockId).Clone();

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

            var inputModel = (CourseInputModel)CourseMockGetter.GetInputModel(mockId).Clone();
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
            CollectionAssert.AreEqual(expectedOutputModel.Themes, actualOutputModel.Themes);
            CollectionAssert.AreEqual(expectedOutputModel.Materials, actualOutputModel.Materials);
        }

        [TestCase(1)]
        public void GetCourseWithThemesAndMaterials_ValidCourseIdSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();

            var inputModel = (CourseInputModel)CourseMockGetter.GetInputModel(mockId).Clone();
            _request = _client.FormPostRequest(TestHelper.Course_Create, inputModel);
            var response = _client.Execute<CourseOutputModel>(_request);
            var addedOutputModel = response.Data;

            var ThemeinputModel = (ThemeInputModel)ThemeMockGetter.GetInputModel(mockId).Clone();
            _request = _client.FormPostRequest(TestHelper.Theme_Create, inputModel);
            var responseTheme = _client.Execute<ThemeOutputModel>(_request);
            var addedThemeOutputModel = response.Data;

            var MaterialinputModel = (MaterialInputModel)MaterialMockGetter.GetInputModel(mockId).Clone();
            _request = _client.FormPostRequest(TestHelper.Material_Create, inputModel);
            var responseMaterial = _client.Execute<MaterialOutputModel>(_request);
            var addedMaterialOutputModel = response.Data;

            //When 
            _request = _client.FormGetRequest<CourseInputModel>($"{TestHelper.Course_Get}/{ addedOutputModel.Id}");
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then

            Assert.AreEqual(HttpStatusCode.OK, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
            CollectionAssert.AreEqual(expectedOutputModel.Themes, actualOutputModel.Themes);
            CollectionAssert.AreEqual(expectedOutputModel.Materials, actualOutputModel.Materials);
        }

        [Test]
        public void GetCourse_InvalidCourseIdSent_NotFoundResponseGot_RecievedCourseModelIsNull()
        {
            //Given

            //When 
            _request = _client.FormGetRequest<CourseInputModel>($"{TestHelper.Course_Get}/{TestHelper.Invalid_ID}");
            var responseGet = _client.Execute<string>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Data;

            //Then
            Assert.AreEqual(HttpStatusCode.NotFound, actualStatusCode);
            Assert.IsNotNull(message);
        }


        [TestCase(1)]
        public void UpdateCourse_ValidModelSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();

            var inputModel = (CourseInputModel)CourseMockGetter.GetInputModel(mockId).Clone();
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
            Assert.AreEqual(HttpStatusCode.OK, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);

        }

        [TestCase(6)]
        public void UpdateCourse_InvalidCourseIdSentEmptyModelSent_NotFoundResponseGot_RecievedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var inputModel = (CourseInputModel)CourseMockGetter.GetInputModel(mockId).Clone();
            
            //When
            _request = _client.FormPutRequest<CourseInputModel>($"{TestHelper.Course_Update}/{TestHelper.Invalid_ID}", inputModel);
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Content;

            //Then
            Assert.AreEqual(HttpStatusCode.NotFound, actualStatusCode);
            Assert.IsNotNull(message);
        }

        [TestCase(1)]
        public void DeleteCourse_ValidCourseId_OkResponseGot_RecievedExtendedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseExtendedOutputModel)CourseExtendedOutputModelMockGetter.GetCourseExtendedOutputModelMock(mockId).Clone();

            var inputModel = (CourseInputModel)CourseMockGetter.GetInputModel(mockId).Clone();

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
            Assert.AreEqual(HttpStatusCode.OK, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCase(1)]
        public void DeleteCourse_InvalidCourseIdSent_NotFoundResponseGot_RecievedNull(int mockId)
        {
            //Given

            //When 
            _request = _client.FormDeleteRequest<CourseInputModel>($"{ TestHelper.Course_Delete}/{TestHelper.Invalid_ID}");
            var responseGet = _client.Execute<CourseExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(HttpStatusCode.NotFound, actualStatusCode);
            Assert.IsNull(actualOutputModel);
        }

        [TestCase(1)]
        public void RecoverCourse_ValidCourseId_OkResponseGot_RecievedExtendedCourseModelMatchesExpectedEmptyModel(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseExtendedOutputModel)CourseExtendedOutputModelMockGetter.GetCourseExtendedOutputModelMock(mockId).Clone();
            var inputModel = (CourseInputModel)CourseMockGetter.GetInputModel(mockId).Clone();
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


            //When 
            _client.FormPutRequest<CourseInputModel>($"{ TestHelper.Course_Recovery}/{TestHelper.Invalid_ID}");
            var responseGet = _client.Execute<string>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Content;

            //Then
            Assert.AreEqual(HttpStatusCode.NotFound, actualStatusCode);
            Assert.IsNotNull(message);
        }


        [TearDown]
        public void TearDown()
        {
            BaseTest.Connection.Execute("dbo.CleanDatabase", commandType: CommandType.StoredProcedure);
        }

    }
}
