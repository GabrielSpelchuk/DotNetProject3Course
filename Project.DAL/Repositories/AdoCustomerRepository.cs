using System.Data;
using Microsoft.Extensions.Logging;
using Project.Domain.Entities;

namespace Project.DAL.Repositories
{
    public interface ICustomerAdoRepository
    {
        Task<int> CreateAsync(Customer customer, CancellationToken ct);
        Task<Customer?> GetByIdAsync(int id, CancellationToken ct);
    }

    public class AdoCustomerRepository : ICustomerAdoRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction? _transaction;
        private readonly ILogger<AdoCustomerRepository> _log;

        public AdoCustomerRepository(IDbConnection connection, IDbTransaction? transaction, ILogger<AdoCustomerRepository> log)
        {
            _connection = connection;
            _transaction = transaction;
            _log = log;
        }

        public async Task<int> CreateAsync(Customer customer, CancellationToken ct)
        {
            var sql = "INSERT INTO orders_db.customers (name, email) VALUES (@name, @email) RETURNING customer_id;";
            if (_connection.State != ConnectionState.Open) await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);
            var cmd = new Npgsql.NpgsqlCommand(sql, (System.Data.Common.DbConnection)_connection as Npgsql.NpgsqlConnection, _transaction as Npgsql.NpgsqlTransaction);
            cmd.Parameters.AddWithValue("name", customer.Name);
            cmd.Parameters.AddWithValue("email", customer.Email);
            var result = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt32(result);
        }

        public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct)
        {
            var sql = "SELECT customer_id, name, email, created_at FROM orders_db.customers WHERE customer_id = @id;";
            if (_connection.State != ConnectionState.Open) await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);
            var cmd = new Npgsql.NpgsqlCommand(sql, (System.Data.Common.DbConnection)_connection as Npgsql.NpgsqlConnection, _transaction as Npgsql.NpgsqlTransaction);
            cmd.Parameters.AddWithValue("id", id);
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            if (!await rdr.ReadAsync(ct)) return null;
            return new Customer { CustomerId = rdr.GetInt32(0), Name = rdr.GetString(1), Email = rdr.GetString(2), CreatedAt = rdr.GetDateTime(3) };
        }
    }
}
