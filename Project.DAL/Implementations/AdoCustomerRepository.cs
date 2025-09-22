using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Project.DAL.Entities;
using System.Collections.Generic;
using System;
using Project.DAL.Repositories.Interfaces;

namespace Project.DAL.Implementations
{
    public class AdoCustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public AdoCustomerRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
        }

        public async Task<int> CreateAsync(Customer customer, CancellationToken ct)
        {
            using var cmd = _connection.CreateCommand();
            cmd.Transaction = _transaction;
            cmd.CommandText = @"INSERT INTO orders_db.customers (name, email) VALUES (@name, @email) RETURNING customer_id;";

            var paramName = cmd.CreateParameter();
            paramName.ParameterName = "@name";
            paramName.Value = customer.Name;
            cmd.Parameters.Add(paramName);

            var paramEmail = cmd.CreateParameter();
            paramEmail.ParameterName = "@email";
            paramEmail.Value = customer.Email;
            cmd.Parameters.Add(paramEmail);

            if (_connection.State != ConnectionState.Open)
                await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);

            var result = await ((System.Data.Common.DbCommand)cmd).ExecuteScalarAsync(ct);
            return Convert.ToInt32(result);
        }


        public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"SELECT customer_id, name, email, created_at 
                        FROM orders_db.customers 
                        WHERE customer_id = @id;";

            var p = cmd.CreateParameter();
            p.ParameterName = "@id";
            p.Value = id;
            cmd.Parameters.Add(p);

            if (_connection.State != ConnectionState.Open)
                await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);

            await using var reader = await ((System.Data.Common.DbCommand)cmd).ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct))
                return null;

            return new Customer
            {
                CustomerId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                CreatedAt = reader.GetDateTime(3)
            };
        }


        public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken ct)
        {
            var list = new List<Customer>();

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"SELECT customer_id, name, email, created_at 
                        FROM orders_db.customers;";

            if (_connection.State != ConnectionState.Open)
                await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);

            await using var reader = await ((System.Data.Common.DbCommand)cmd).ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new Customer
                {
                    CustomerId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    CreatedAt = reader.GetDateTime(3)
                });
            }

            return list;
        }

    }
}
