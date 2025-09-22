using AutoMapper;
using Dapper;
using Project.BLL.Services.Interfaces;
using Project.DAL.Entities;
using Project.DAL.Implementations;
using Project.DAL.Repositories;
using Project.DAL.Repositories.Interfaces;
using static Project.BLL.Dtos.OrderDtos;

namespace Project.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDbConnectionFactory _connFactory;
        private readonly IMapper _mapper;

        public OrderService(IDbConnectionFactory connFactory, IMapper mapper)
        {
            _connFactory = connFactory;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct)
        {
            await using var uow = new UnitOfWork(_connFactory);
            await uow.BeginTransactionAsync();
            try
            {
                var order = new Order { CustomerId = dto.CustomerId, SupplierId = dto.SupplierId, Status = "Pending" };
                var orderId = await uow.Orders.CreateAsync(order, ct);

                var items = new List<OrderItem>();
                foreach (var it in dto.Items)
                {
                    var price = await uow.Products.GetPriceAsync(it.ProductId, ct) ?? it.UnitPrice;
                    items.Add(new OrderItem { OrderId = orderId, ProductId = it.ProductId, Quantity = it.Quantity, UnitPrice = price });
                }

                await uow.Orders.AddItemsAsync(orderId, items, ct);

                await uow.CommitAsync();
                return orderId;
            }
            catch
            {
                await uow.RollbackAsync();
                throw;
            }
        }

        public async Task<OrderDto> GetOrderAsync(int orderId, CancellationToken ct)
        {
            await using var uow = new UnitOfWork(_connFactory);
            await uow.BeginTransactionAsync();
            var order = await uow.Orders.GetByIdAsync(orderId, ct);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task ConfirmOrderAsync(int orderId, CancellationToken ct)
        {
            await using var uow = new UnitOfWork(_connFactory);
            await uow.BeginTransactionAsync(ct);
            try
            {
                var order = await uow.Orders.GetByIdAsync(orderId, ct);
                if (order == null)
                    throw new KeyNotFoundException($"Order {orderId} not found");

                decimal total = order.Items.Sum(i => i.Quantity * i.UnitPrice);

                var updateSql = @"UPDATE orders_db.orders 
                          SET status = @Status 
                          WHERE order_id = @OrderId;";

                await uow.Connection.ExecuteAsync(updateSql,
                    new { Status = "Confirmed", OrderId = orderId },
                    uow.Transaction);

                var insertPayment = @"INSERT INTO orders_db.payments (order_id, amount, paid_at) 
                              VALUES (@OrderId, @Amount, NOW()) 
                              ON CONFLICT (order_id) DO NOTHING;";

                await uow.Connection.ExecuteAsync(insertPayment,
                    new { OrderId = orderId, Amount = total },
                    uow.Transaction);

                await uow.CommitAsync();
            }
            catch
            {
                await uow.RollbackAsync();
                throw;
            }
        }


        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(int customerId, CancellationToken ct)
        {
            await using var uow = new UnitOfWork(_connFactory);
            await uow.BeginTransactionAsync();
            var orders = await uow.Orders.GetByCustomerIdAsync(customerId, ct);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }
}
