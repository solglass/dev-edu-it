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

            Request = FormRequest<CourseInputModel>(Method.POST, new CourseMockGetter(), TestHelper.Course_Create, mockId);

            //When
            var response = Client.Execute<CourseOutputModel>(Request);
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
           var response = Client.Execute<string>(Request);
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
            var response = Client.Execute<CourseOutputModel>(Request);
            var addedOutputModel = response.Data;
            Assert.IsTrue(addedOutputModel.Id != 0);

            //When 
            FormRequest<CourseInputModel>(Method.GET, new CourseMockGetter(),
              $"{TestHelper.Course_Get}/{ addedOutputModel.Id}");
            var responseGet = Client.Execute<CourseOutputModel>(Request);
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
            HttpMethod = Method.GET;
            Request = new RestRequest($"/Course/{TestHelper.Invalid_ID}", HttpMethod);
            var responseGet = Client.Execute<string>(Request);
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

            FormRequest<CourseInputModel>(Method.POST, new CourseMockGetter(), TestHelper.Course_Create, mockId);
            var responsePost = Client.Execute<CourseOutputModel>(Request);
            var addedOutputModel = responsePost.Data;
            expectedOutputModel.Id = addedOutputModel.Id;


            InputModel.Name = "Updated Name";
            InputModel.Description = "Updated description";
            InputModel.Duration = 9;
            expectedOutputModel.Name = InputModel.Name;
            expectedOutputModel.Description = InputModel.Description;
            expectedOutputModel.Duration = InputModel.Duration;

            //When 
            FormRequest<CourseInputModel>(Method.PUT, new CourseMockGetter(), $"{TestHelper.Course_Update}/{addedOutputModel.Id}", inputModel: InputModel);
            var responseGet = Client.Execute<CourseOutputModel>(Request);
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
            var responseGet = Client.Execute<CourseOutputModel>(Request);
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
            var expectedOutputModel = (CourseExtendedOutputModel)CourseExtendedOutputModelGetter.GetCourseExtendedOutputModelMock(mockId).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            FormRequest<CourseInputModel>(Method.POST, new CourseMockGetter(), TestHelper.Course_Create, mockId);
            var response = Client.Execute<CourseExtendedOutputModel>(Request);
            var addedOutputModel = response.Data;
            expectedOutputModel.Id = addedOutputModel.Id;
            expectedOutputModel.IsDeleted = true;



            //When 
            HttpMethod = Method.DELETE;
            Request = new RestRequest($"/Course/{addedOutputModel.Id}", HttpMethod);
            var responseGet = Client.Execute<CourseExtendedOutputModel>(Request);
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
            HttpMethod = Method.DELETE;
            Request = new RestRequest("api/Course/0", HttpMethod);
            var responseGet = Client.Execute<CourseExtendedOutputModel>(Request);
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

            FormRequest<CourseInputModel>(Method.POST, new CourseMockGetter(), TestHelper.Course_Update, mockId);
            var response = Client.Execute<CourseExtendedOutputModel>(Request);
            var addedOutputModel = response.Data;
            var responsePost = Client.Execute<CourseExtendedOutputModel>(Request);



            //When 
            FormRequest<CourseInputModel>(Method.PUT, new CourseMockGetter(), $"{ TestHelper.Course_Update}/{ addedOutputModel.Id}", inputModel: addedOutputModel);
            var responseGet = Client.Execute<CourseExtendedOutputModel>(Request);
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
            HttpMethod = Method.PUT;
            Request = new RestRequest($"/Course/{TestHelper.Invalid_ID}/recovery", HttpMethod);
            var responseGet = Client.Execute<string>(Request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Content;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNotNull(message);
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


            FormRequest<ThemeInputModel>(Method.POST, new ThemeMockGetter(), TestHelper.Theme_Create, mockId);


            //When
            var response = Client.Execute<ThemeExtendedOutputModel>(Request);
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

            FormRequest<ThemeInputModel>(Method.POST, new ThemeMockGetter(), TestHelper.Theme_Create, mockId);

            //When
            var response = Client.Execute<string>(Request);
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

            FormRequest<ThemeInputModel>(Method.POST, new ThemeMockGetter(), TestHelper.Theme_Create, mockId);

            var response = Client.Execute<ThemeExtendedOutputModel>(Request);
            var addedOutputModel = response.Data;
            Assert.IsTrue(addedOutputModel.Id != 0);

            //When
            FormRequest<ThemeExtendedOutputModel>(Method.GET, new ThemeMockGetter(), $"{TestHelper.Theme_Get}/{addedOutputModel.Id}");
  
            var responseGet = Client.Execute<ThemeExtendedOutputModel>(Request);
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
            HttpMethod = Method.GET;
            Request = new RestRequest($"api/Course/theme/{TestHelper.Invalid_ID}", HttpMethod);
            var responseGet = Client.Execute<CourseOutputModel>(Request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Content;

            //Then
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsNotNull(message);
        }
        




        [TearDown]
        public void TearDown()
        {
            Connection.Execute("dbo.CleanDatabase", commandType: CommandType.StoredProcedure);
        }
    }
}
