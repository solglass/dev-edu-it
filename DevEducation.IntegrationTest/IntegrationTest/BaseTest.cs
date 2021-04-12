using System.Data.SqlClient;

namespace IntegrationTest
{
    public abstract class BaseTest
    {
        private const string _connectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu.Test; Persist Security Info=True";
        protected SqlConnection _connection;
        public BaseTest()
        {
            _connection = new SqlConnection(_connectionString);
        }
    }
}
