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
        public RestClient _client;
        public Method _httpMethod;
        public RestRequest _request;
        public string _token;


        protected SqlConnection _connection;
        protected AppSettings appSettings;
        public BaseTest()
        {
            appSettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText("appsettings.json"));
            _connection = new SqlConnection(appSettings.ConnectionString);
        }

        public void SetupClient() 
        {
            _client = new RestClient(TestHelper.ApiUrl);
            _httpMethod = Method.POST;
            var authenticationInputModel = new AuthenticationInputModel { Login = appSettings.Login, Password = appSettings.Password };
            var authenticationRequest = new RestRequest(TestHelper.User_Authentication, _httpMethod);
            authenticationRequest.AddParameter("application/json", JsonSerializer.Serialize(authenticationInputModel), ParameterType.RequestBody);
            var authenticationResponse = _client.Execute<AuthResponse>(authenticationRequest);
            _token = authenticationResponse.Data.Token;
            _client.Authenticator = new JwtAuthenticator(_token);
        }

        public RestRequest FormRequest<T>(Method method, IModelMockGetter modelMockGetter, string route, int mockId)
        {
            _httpMethod = method;
            _request = new RestRequest(route, _httpMethod);
            var inputModel = (T)modelMockGetter.GetInputModel(mockId);
            _request.AddParameter("application/json", JsonSerializer.Serialize(inputModel), ParameterType.RequestBody);
            return _request;
        }

        [TearDown]
        public void DeleteAll()
        {
            _connection.Execute("dbo.CleanDatabase", commandType: CommandType.StoredProcedure);
        }
    }
}
