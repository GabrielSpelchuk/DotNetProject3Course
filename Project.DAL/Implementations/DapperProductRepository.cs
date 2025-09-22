using Dapper;
using Project.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Implementations
{
    public class DapperProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public DapperProductRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<decimal?> GetPriceAsync(int productId, CancellationToken ct)
        {
            var sql = @"SELECT price FROM catalog_db.products WHERE product_id = @ProductId;";
            if (_connection.State != ConnectionState.Open) await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);
            return await _connection.ExecuteScalarAsync<decimal?>(new CommandDefinition(sql, new { ProductId = productId }, _transaction, cancellationToken: ct));
        }
    }
}
