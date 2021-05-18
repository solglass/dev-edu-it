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
    public static class BaseTest
    {

        public static RestRequest Request { get; set; }
        public static SqlConnection Connection { get; private set; }
        private static AppSettings _appSettings;
        private static string _token;

        static BaseTest()
        {
            _appSettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText("appsettings.json"));
            Connection = new SqlConnection(_appSettings.ConnectionString);
        }

        public static void SetupClient(this IRestClient Client)
        {

            var authenticationInputModel = new AuthenticationInputModel { Login = _appSettings.Login, Password = _appSettings.Password };
            var authenticationRequest = new RestRequest(TestHelper.User_Authentication, Method.POST);
            authenticationRequest.AddParameter("application/json", JsonSerializer.Serialize(authenticationInputModel), ParameterType.RequestBody);
            var authenticationResponse = Client.Execute<AuthResponse>(authenticationRequest);
            _token = authenticationResponse.Data.Token;
            Client.Authenticator = new JwtAuthenticator(_token);
        }

        public static RestRequest FormPostRequest<T>(this IRestClient Client, string route, T inputModel = default)
        {
            Request = new RestRequest(route, Method.POST);

            var InputModel = inputModel;
            if (inputModel != null)
            {
                Request.AddParameter("application/json", JsonSerializer.Serialize(InputModel), ParameterType.RequestBody);
            }

            return Request;
        }

        public static RestRequest FormPutRequest<T>(this IRestClient Client, string route, T inputModel = default)
        {

            Request = new RestRequest(route, Method.PUT);

            if (inputModel == null)
            {
                return Request;
            }
            var InputModel = inputModel;
            Request.AddParameter("application/json", JsonSerializer.Serialize(InputModel), ParameterType.RequestBody);

            return Request;
        }

        public static RestRequest FormGetRequest<T>(this IRestClient Client, string route) => new RestRequest(route, Method.GET);

        public static RestRequest FormDeleteRequest<T>(this IRestClient Client, string route) =>  new RestRequest(route, Method.DELETE);

        [TearDown]
        public static void DeleteAll()
        {
            Connection.Execute("dbo.CleanDatabase", commandType: CommandType.StoredProcedure);
        }
    }
}
