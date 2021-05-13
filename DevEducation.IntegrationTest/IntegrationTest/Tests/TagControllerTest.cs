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

            _request = _client.FormPostRequest<CourseInputModel>( TestHelper.Course_Create, inputModel);

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

            _request = _client.FormPostRequest<CourseInputModel>( TestHelper.Course_Create, inputModel);

            //When
           var response = _client.Execute<string>(_request);
           var message = response.Data;


            //Then
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
            Assert.IsNotNull(message);
        }




        [TearDown]
        public void TearDown()
        {
            BaseTest.Connection.Execute("dbo.CleanDatabase", commandType: CommandType.StoredProcedure);
        }
    }
}
