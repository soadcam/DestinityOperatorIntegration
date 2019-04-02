using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace OperadorDestinoCarpetaMock.DAO
{
    public class DapperDAO
    {
        private readonly string _connectionString;

        public DapperDAO() =>
            _connectionString = ConfigurationManager.ConnectionStrings["DocumentConnection"].ConnectionString;

        public void ExecQuery(string query, object parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql: query, param: parameters);
            }
        }

        public T GetFirstOrDefault<T>(string query, object parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IEnumerable<T> result = connection.Query<T>(sql: query, param: parameters);
                return result.FirstOrDefault();
            }
        }

        public IEnumerable<T> GetList<T>(string query, object parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<T>(sql: query, param: parameters);
            }
        }
    }
}