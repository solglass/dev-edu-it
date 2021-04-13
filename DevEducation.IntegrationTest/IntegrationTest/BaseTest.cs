using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace IntegrationTest
{
    public abstract class BaseTest
    {
        protected SqlConnection _connection;
        public BaseTest()
        {
            _connection = new SqlConnection(JsonSerializer.Deserialize<string>(File.ReadAllText("connectionString.json")));
        }
    }
}
