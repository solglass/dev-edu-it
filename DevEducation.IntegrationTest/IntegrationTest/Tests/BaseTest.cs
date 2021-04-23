using System.IO;
using System.Text.Json;
using System.Data.SqlClient;
using IntegrationTest.Settings;
using RestSharp;
using IntegrationTest.Models.InputModels;
using IntegrationTest.Models;
using RestSharp.Authenticators;
using IntegrationTest.Mocks.InputModels;
using Dapper;
using System.Data;
using NUnit.Framework;

namespace IntegrationTest
{
    public abstract class BaseTest
    {
        protected RestClient Client { get; set; }
        protected Method HttpMethod { get; set; }
        protected RestRequest Request { get; set; }
        protected dynamic InputModel { get; set; } 
        protected SqlConnection Connection { get;  }
        private AppSettings _appSettings;
        private string _token;
        public BaseTest()
        {
            _appSettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText("appsettings.json"));
            Connection = new SqlConnection(_appSettings.ConnectionString);
        }

        public void SetupClient() 
        {
            Client = new RestClient(TestHelper.ApiUrl);
            HttpMethod = Method.POST;
            var authenticationInputModel = new AuthenticationInputModel { Login = _appSettings.Login, Password = _appSettings.Password };
            var authenticationRequest = new RestRequest(TestHelper.User_Authentication, HttpMethod);
            authenticationRequest.AddParameter("application/json", JsonSerializer.Serialize(authenticationInputModel), ParameterType.RequestBody);
            var authenticationResponse = Client.Execute<AuthResponse>(authenticationRequest);
            _token = authenticationResponse.Data.Token;
            Client.Authenticator = new JwtAuthenticator(_token);
        }

        public RestRequest FormRequest<T>(Method method, IModelMockGetter modelMockGetter, string route, int mockId = -1, dynamic inputModel = null)
        {
            InputModel = null;
            HttpMethod = method;
            Request = new RestRequest(route, HttpMethod);
            if (mockId > 0)
            { 
                InputModel = (T)modelMockGetter.GetInputModel(mockId); 
            }
            else if (inputModel != null)
            {
                InputModel = inputModel;
            }
            if (InputModel != null)
            {
                Request.AddParameter("application/json", JsonSerializer.Serialize(InputModel), ParameterType.RequestBody);
            }
            return Request;
        }

        [TearDown]
        public void DeleteAll()
        {
            Connection.Execute("dbo.CleanDatabase", commandType: CommandType.StoredProcedure);
        }
    }
}
