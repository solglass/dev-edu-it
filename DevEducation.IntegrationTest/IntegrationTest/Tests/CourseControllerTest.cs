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
        public IRestClient Client { get; set; }
        public RestRequest Request { get; set; }
        [SetUp]
        public void Setup()
        {
            Client = new RestClient(TestHelper.ApiUrl);
            Client.SetupClient();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void AddCourse_ValidCourseInputModelSent_OkResponseGot_RecievedCourseModelMatchesExpected(int mockId)
        {
            //Given
            var expectedOutputModel = (CourseOutputModel)CourseOutputModelMockGetter.GetCourseOutputModelMock(mockId).Clone();

            var inputModel = CourseMockGetter.GetInputModel(mockId);

            Request = BaseTest.FormPostRequest<CourseInputModel>(Method.POST, TestHelper.Course_Create, inputModel);

            //When
            var response = Client.Execute<CourseOutputModel>(Request);
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

            Request = BaseTest.FormPostRequest<CourseInputModel>(Method.POST, TestHelper.Course_Create, inputModel);

            //When
           var response = Client.Execute<string>(Request);
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
