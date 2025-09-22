using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class NpgsqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public NpgsqlConnectionFactory(string connectionString) => _connectionString = connectionString;
        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}
