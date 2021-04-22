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
    public class UserControllerTest : BaseTest
    {
        private List<int> _userIdList;

        [SetUp]
        public void Setup()
        {
            SetupClient();
            _userIdList = new List<int>();
        }

        [TestCase(1)]
        public void RegistrationPass_ValidUserInputModelSent_OkResponseGot_UserExistsUnderId(int mockId)
        {
            //Given
            var expectedOutputModel = (UserOutputModel)UserOutputModelMockGetter.GetUserOutputModelMock(1).Clone();
            var expectedStatusCode = HttpStatusCode.OK;

            _request = FormRequest<UserInputModel>(Method.POST, new UserMockGetter(), TestHelper.User_Register, mockId);

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

    }
}