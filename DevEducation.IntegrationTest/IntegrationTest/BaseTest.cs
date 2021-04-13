using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.SqlClient;
using IntegrationTest.Settings;

namespace IntegrationTest
{
    public abstract class BaseTest
    {
        protected SqlConnection _connection;
        protected AppSettings appSettings;
        public BaseTest()
        {
            appSettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText("appsettings.json"));
            _connection = new SqlConnection(appSettings.ConnectionString);
        }
    }
}
