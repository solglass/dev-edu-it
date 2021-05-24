using NUnit.Framework;
using RestSharp;
using IntegrationTest.Models.InputModels;
using System.Net;
using IntegrationTest.Models.OutputModels;
using System.Collections.Generic;
using Dapper;
using IntegrationTest.Mocks;
using System.Data;
using System.Collections;

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

        [TestCaseSource(typeof(ValidCourseWithoutThemesAndMaterialsMockCollection))]
        public void AddCourse_ValidCourseInputModelSent_OkResponseGot_RecievedCourseModelMatchesExpected(CourseOutputModel expectedOutputModel, CourseInputModel inputModel)
        {
                //Given
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


        [TestCaseSource(typeof(EmptyCourseMockCollection))]
        public void AddCourse_EmptyCourseInputModelSent_ConflictResponseGot_MessageIsNotNull(CourseOutputModel expectedOutputModel, CourseInputModel inputModel)
        {

            //Given

            _request = _client.FormPostRequest<CourseInputModel>(TestHelper.Course_Create, inputModel);

            //When
            var response = _client.Execute<string>(_request);
            var message = response.Data;


            //Then
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
            Assert.IsNotNull(message);
        }

        [TestCaseSource(typeof(ValidCourseWithThemesAndMaterialsMockCollection))]
        public void AddCourse_ValidCourseInputModelWithThemesAndMaterialsSent_OkResponseGot_RecievedCourseModelMatchesExpected(CourseWithProgramOutputModel expectedOutputModel, CourseInputModel inputModel, ThemeInputModel[] themesMocks, MaterialInputModel[] materialsMocks)
        {
            //Given
            var themesExpectedOrdered = new List<ThemeOrderedOutputModel>();
            int order = 1;
            foreach (ThemeInputModel themeInputModel in themesMocks)
            {
                
                _request = _client.FormPostRequest<ThemeInputModel>(TestHelper.Theme_Create, themeInputModel);
                var responseTheme = _client.Execute<ThemeOutputModel>(_request);
                var addedThemeOutputModel = responseTheme.Data;
                inputModel.Themes.Add(new OrderedThemeInputModel { Order = order, Id = addedThemeOutputModel.Id});
                expectedOutputModel.Themes.Add(new ThemeOrderedOutputModel { Order = order, Id = addedThemeOutputModel.Id });
                order += 1;
            }

            foreach (MaterialInputModel materialinputModel in materialsMocks)
            {
                _request = _client.FormPostRequest<MaterialInputModel>(TestHelper.Material_Create, materialinputModel);
                var responseMaterial = _client.Execute<MaterialOutputModel>(_request);
                var addedMaterialOutputModel = responseMaterial.Data;
                expectedOutputModel.Materials.Add(addedMaterialOutputModel);

            }


            //When
            _request = _client.FormPostRequest<CourseInputModel>(TestHelper.Course_Create, inputModel);
            var response = _client.Execute<CourseWithProgramOutputModel>(_request);
            var actualOutputModel = response.Data;
            foreach (MaterialOutputModel material in expectedOutputModel.Materials)
            {
                _request = _client.FormPostRequest<MaterialInputModel>($"{TestHelper.Course_Create}/{actualOutputModel.Id}{TestHelper.AddMaterialToCourse}{material.Id}");
                var responseMaterial = _client.Execute<MaterialOutputModel>(_request);
                var addedMaterialToCourse = responseMaterial.Data;
                Assert.AreEqual(HttpStatusCode.Created, responseMaterial.StatusCode);
            }


            //Then
            expectedOutputModel.Id = actualOutputModel.Id;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
            CollectionAssert.AreEqual(expectedOutputModel.Themes, actualOutputModel.Themes);

        }




        [TestCaseSource(typeof(ValidCourseWithoutThemesAndMaterialsMockCollection))]
        public void GetCourse_ValidCourseIdSent_OkResponseGot_RecievedCourseModelMatchesExpected(CourseOutputModel expectedOutputModel, CourseInputModel inputModel)
        {

            //Given
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
            CollectionAssert.AreEqual(expectedOutputModel.Materials, actualOutputModel.Materials);
        }

        [TestCaseSource(typeof(ValidCourseWithThemesAndMaterialsMockCollection))]
        public void GetCourseWithThemesAndMaterials_ValidCourseIdSent_OkResponseGot_RecievedCourseModelMatchesExpected(CourseWithProgramOutputModel expectedOutputModel, CourseInputModel inputModel, ThemeInputModel[] themesMocks, MaterialInputModel[] materialsMocks)
        {
            //Given
            var themesExpectedOrdered = new List<ThemeOrderedOutputModel>();
            int order = 1;
            foreach (ThemeInputModel themeInputModel in themesMocks)
            {

                _request = _client.FormPostRequest<ThemeInputModel>(TestHelper.Theme_Create, themeInputModel);
                var responseTheme = _client.Execute<ThemeOutputModel>(_request);
                var addedThemeOutputModel = responseTheme.Data;
                inputModel.Themes.Add(new OrderedThemeInputModel { Order = order, Id = addedThemeOutputModel.Id });
                expectedOutputModel.Themes.Add(new ThemeOrderedOutputModel { Order = order, Id = addedThemeOutputModel.Id });
                order += 1;
            }

            foreach (MaterialInputModel materialinputModel in materialsMocks)
            {
                _request = _client.FormPostRequest<MaterialInputModel>(TestHelper.Material_Create, materialinputModel);
                var responseMaterial = _client.Execute<MaterialOutputModel>(_request);
                var addedMaterialOutputModel = responseMaterial.Data;
                expectedOutputModel.Materials.Add(addedMaterialOutputModel);

            }


            //When
            _request = _client.FormPostRequest<CourseInputModel>(TestHelper.Course_Create, inputModel);
            var response = _client.Execute<CourseWithProgramOutputModel>(_request);
            var addedOutputModel = response.Data;
            foreach (MaterialOutputModel material in expectedOutputModel.Materials)
            {
                _request = _client.FormPostRequest<MaterialInputModel>($"{TestHelper.Course_Create}/{addedOutputModel.Id}{TestHelper.AddMaterialToCourse}{material.Id}");
                var responseMaterial = _client.Execute<MaterialOutputModel>(_request);
                var addedMaterialToCourse = responseMaterial.Data;
                Assert.AreEqual(HttpStatusCode.Created, responseMaterial.StatusCode);
            }

            //When 
            _request = _client.FormGetRequest<CourseInputModel>($"{TestHelper.Course_Get}/{ addedOutputModel.Id}");
            var responseGet = _client.Execute<CourseWithProgramOutputModel>(_request);
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

        
        [TestCaseSource(typeof(ValidCourseWithoutThemesAndMaterialsMockCollection))]
        public void UpdateCourse_ValidModelWithoutThemesAndMaterialsSent_OkResponseGot_RecievedCourseModelMatchesExpected(CourseOutputModel expectedOutputModel, CourseInputModel inputModel)
        {
            //Given
            _request = _client.FormPostRequest(TestHelper.Course_Create, inputModel);
            var response = _client.Execute<CourseOutputModel>(_request);
            var addedOutputModel = response.Data;
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

        [TestCaseSource(typeof(EmptyCourseMockCollection))]
        public void UpdateCourse_InvalidCourseIdSentEmptyModelSent_NotFoundResponseGot_RecievedCourseModelMatchesExpectedEmptyModel(CourseExtendedOutputModel emptyModel, CourseInputModel inputModel)
        {
            //Given


            //When
            _request = _client.FormPutRequest<CourseInputModel>($"{TestHelper.Course_Update}/{TestHelper.Invalid_ID}", inputModel);
            var responseGet = _client.Execute<CourseOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var message = responseGet.Content;

            //Then
            Assert.AreEqual(HttpStatusCode.NotFound, actualStatusCode);
            Assert.IsNotNull(message);
        }


        [TestCaseSource(typeof(ValidCourseWithoutThemesAndMaterialsMockCollection))]
        public void DeleteCourse_ValidCourseId_OkResponseGot_RecievedExtendedCourseModelMatchesExpectedEmptyModel(CourseOutputModel expectedOutputModel, CourseInputModel inputModel)
        {
            //Given
            _request = _client.FormPostRequest(TestHelper.Course_Create, inputModel);
            var response = _client.Execute<CourseOutputModel>(_request);
            var addedOutputModel = response.Data;



            //When 
            _request = _client.FormDeleteRequest<CourseInputModel>($"{ TestHelper.Course_Delete}/{addedOutputModel.Id}");
            var responseGet = _client.Execute<CourseExtendedOutputModel>(_request);
            var actualStatusCode = responseGet.StatusCode;
            var actualOutputModel = responseGet.Data;

            //Then
            Assert.AreEqual(HttpStatusCode.OK, actualStatusCode);
            Assert.AreEqual(expectedOutputModel, actualOutputModel);
        }

        [TestCaseSource(typeof(ValidCourseExtendedModelMockCollection))]
        public void DeleteCourse_InvalidCourseIdSent_NotFoundResponseGot_RecievedNull(CourseExtendedOutputModel expectedOutputModel, CourseInputModel inputModel)
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

        [TestCaseSource(typeof(ValidCourseExtendedModelMockCollection))]
        public void RecoverCourse_ValidCourseId_OkResponseGot_RecievedExtendedCourseModelMatchesExpectedEmptyModel(CourseExtendedOutputModel expectedOutputModel, CourseInputModel inputModel)
        {
            //Given
            _request = _client.FormPostRequest(TestHelper.Course_Create, inputModel);
            var response = _client.Execute<CourseExtendedOutputModel>(_request);
            var addedOutputModel = response.Data;
            expectedOutputModel.Id = addedOutputModel.Id;

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
        
        [Test]
        public void RecoverCourse_InvalidCourseIdSent_NotFoundResponseGot_RecievedNull()
        {
            //Given


            //When 
            _request = _client.FormPutRequest<CourseInputModel>($"{TestHelper.Course_Create}/{TestHelper.Invalid_ID}/{ TestHelper.Course_Recovery}");
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
