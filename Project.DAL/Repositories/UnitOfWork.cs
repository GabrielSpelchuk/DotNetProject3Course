using Project.DAL.Repositories.Interfaces;
using System.Data;

namespace Project.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly IDbConnectionFactory _factory;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public ICustomerRepository Customers { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IProductRepository Products { get; private set; }


        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction;

        public UnitOfWork(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        private void EnsureConnectionOpen()
        {
            if (_connection == null)
                _connection = _factory.CreateConnection();
            if (_connection.State != ConnectionState.Open)
                ((System.Data.Common.DbConnection)_connection).Open();
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_connection == null)
                _connection = _factory.CreateConnection();

            if (_connection.State != ConnectionState.Open)
                await ((System.Data.Common.DbConnection)_connection).OpenAsync(ct);

            _transaction = _connection.BeginTransaction();

            Customers = new Implementations.AdoCustomerRepository(_connection, _transaction);
            Orders = new Implementations.DapperOrderRepository(_connection, _transaction);
            Products = new Implementations.DapperProductRepository(_connection, _transaction);
        }

        public Task CommitAsync()
        {
            _transaction?.Commit();
            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            _transaction?.Rollback();
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            _transaction?.Dispose();

            if (_connection != null)
            {
                await ((System.Data.Common.DbConnection)_connection).CloseAsync();
                _connection.Dispose();
            }
        }

        public Task BeginTransactionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
