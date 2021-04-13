using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace IntegrationTest
{
    public abstract class BaseTest
    {
        private string _connectionString;
        protected SqlConnection _connection;
        public BaseTest()
        {
            GetConnectionString();
            _connection = new SqlConnection(_connectionString);
        }
        private async void GetConnectionString()
        {
            // чтение данных
            using (FileStream fs = new FileStream("connectionString.json", FileMode.OpenOrCreate))
            {
                _connectionString = await JsonSerializer.DeserializeAsync<string>(fs);
            }
        }
    }
}
