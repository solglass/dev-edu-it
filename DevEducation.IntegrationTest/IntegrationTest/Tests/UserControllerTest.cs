using NUnit.Framework;
using RestSharp;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Mocks.InputModels;
using IntegrationTest.Mocks.OutputModels;
using System.Net;
using IntegrationTest.Models.OutputModels;
using System.Collections.Generic;
using Dapper;
using System.Data;

namespace IntegrationTest
{
    public class UserControllerTest 
    {
        private List<int> _userIdList;
        public IRestClient Client { get; set; }
        public RestRequest Request { get; set; }
        [SetUp]
        public void Setup()
        {
            Client = new RestClient(TestHelper.ApiUrl);
            Client.SetupClient();
            _userIdList = new List<int>();
        }

        [TestCase(1)]
        public void RegistrationPass_ValidUserInputModelSent_OkResponseGot_UserExistsUnderId(int mockId)
        {
            //Given
            var expectedOutputModel = (UserOutputModel)UserOutputModelMockGetter.GetUserOutputModelMock(1).Clone();
            var expectedStatusCode = HttpStatusCode.OK;
            var inputModel = UserMockGetter.GetInputModel(mockId);
            Request = BaseTest.FormPostRequest<UserInputModel>(Method.POST, TestHelper.User_Register, inputModel);

            //When
            var response = Client.Execute<UserOutputModel>(Request);
            var actualStatusCode = response.StatusCode;
            var actualOutputModel = response.Data;
            _userIdList.Add(actualOutputModel.Id);


            //Then
            Assert.IsTrue(actualOutputModel.Id != 0);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

    }
}