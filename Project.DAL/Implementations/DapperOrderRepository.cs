using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Project.DAL.Repositories.Interfaces;

namespace Project.DAL.Implementations
{
    public class DapperOrderRepository : IOrderRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public DapperOrderRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
        }

        public async Task<int> CreateAsync(Order order, CancellationToken ct)
        {
            var sql = @"INSERT INTO orders_db.orders (customer_id, supplier_id, status) VALUES (@CustomerId, @SupplierId, @Status) RETURNING order_id;";
            if (_connection.State != ConnectionState.Open) await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);
            var id = await _connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { order.CustomerId, order.SupplierId, order.Status }, _transaction, cancellationToken: ct));
            return id;
        }

        public async Task AddItemsAsync(int orderId, IEnumerable<OrderItem> items, CancellationToken ct)
        {
            var sql = @"INSERT INTO orders_db.order_items (order_id, product_id, quantity, unit_price) VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice);";
            if (_connection.State != ConnectionState.Open) await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);
            foreach (var it in items)
            {
                await _connection.ExecuteAsync(new CommandDefinition(sql, new { OrderId = orderId, ProductId = it.ProductId, Quantity = it.Quantity, UnitPrice = it.UnitPrice }, _transaction, cancellationToken: ct));
            }
        }

        public async Task<Order> GetByIdAsync(int id, CancellationToken ct)
        {
            var sqlOrder = @"SELECT order_id, customer_id, supplier_id, status, created_at FROM orders_db.orders WHERE order_id = @Id;";
            var sqlItems = @"SELECT order_id, product_id, quantity, unit_price FROM orders_db.order_items WHERE order_id = @Id;";
            if (_connection.State != ConnectionState.Open) await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);
            var order = await _connection.QuerySingleOrDefaultAsync<Order>(new CommandDefinition(sqlOrder, new { Id = id }, _transaction, cancellationToken: ct));
            if (order == null) return null;
            var items = (await _connection.QueryAsync<OrderItem>(new CommandDefinition(sqlItems, new { Id = id }, _transaction, cancellationToken: ct))).ToList();
            order.Items = items;
            return order;
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId, CancellationToken ct)
        {
            var sql = @"SELECT o.order_id, o.customer_id, o.supplier_id, o.status, o.created_at,
                               oi.product_id, oi.quantity, oi.unit_price
                        FROM orders_db.orders o
                        LEFT JOIN orders_db.order_items oi ON o.order_id = oi.order_id
                        WHERE o.customer_id = @CustomerId
                        ORDER BY o.order_id;";
            var dict = new Dictionary<int, Order>();
            if (_connection.State != ConnectionState.Open) await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);
            await _connection.QueryAsync<Order, OrderItem, Order>(new CommandDefinition(sql, new { CustomerId = customerId }, _transaction, cancellationToken: ct),
                (o, oi) =>
                {
                    if (!dict.TryGetValue(o.OrderId, out var order))
                    {
                        order = o;
                        order.Items = new List<OrderItem>();
                        dict.Add(order.OrderId, order);
                    }
                    if (oi != null) order.Items.Add(oi);
                    return order;
                }, splitOn: "product_id");
            return dict.Values;
        }
    }
}
