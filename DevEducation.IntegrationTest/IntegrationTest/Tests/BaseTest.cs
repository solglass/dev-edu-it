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

        public static Method HttpMethod { get; set; }
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

            HttpMethod = Method.POST;
            var authenticationInputModel = new AuthenticationInputModel { Login = _appSettings.Login, Password = _appSettings.Password };
            var authenticationRequest = new RestRequest(TestHelper.User_Authentication, HttpMethod);
            authenticationRequest.AddParameter("application/json", JsonSerializer.Serialize(authenticationInputModel), ParameterType.RequestBody);
            var authenticationResponse = Client.Execute<AuthResponse>(authenticationRequest);
            _token = authenticationResponse.Data.Token;
            Client.Authenticator = new JwtAuthenticator(_token);
        }

        public static RestRequest FormPostRequest<T>(this IRestClient Client, string route, T inputModel)
        {
            HttpMethod = Method.POST;
            Request = new RestRequest(route, HttpMethod);

            var InputModel = inputModel;

            Request.AddParameter("application/json", JsonSerializer.Serialize(InputModel), ParameterType.RequestBody);

            return Request;
        }

        public static RestRequest FormPutRequest<T>(this IRestClient Client, string route, T inputModel)
        {
            HttpMethod = Method.PUT;
            Request = new RestRequest(route, HttpMethod);

           var InputModel = inputModel;

            Request.AddParameter("application/json", JsonSerializer.Serialize(InputModel), ParameterType.RequestBody);

            return Request;
        }

        public static RestRequest FormGetRequest<T>(this IRestClient Client, string route)
        {
            HttpMethod = Method.GET;
            Request = new RestRequest(route, HttpMethod);

            return Request;
        }

        public static RestRequest FormDeleteRequest<T>(this IRestClient Client, string route)
        {
            HttpMethod = Method.DELETE;
            Request = new RestRequest(route, HttpMethod);

            return Request;
        }

        [TearDown]
        public static void DeleteAll()
        {
            Connection.Execute("dbo.CleanDatabase", commandType: CommandType.StoredProcedure);
        }
    }
}
