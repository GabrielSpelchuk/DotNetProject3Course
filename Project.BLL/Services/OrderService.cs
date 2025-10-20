using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Dtos.Customers;
using Project.BLL.Dtos.Orders;
using Project.BLL.Query;
using Project.DAL.Uow;
using Project.Domain.Entities;

namespace Project.BLL.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        
        public async Task<int> CreateAsync(CreateOrderDto dto, CancellationToken ct)
        {
            var order = _mapper.Map<Order>(dto);
            await _uow.Orders.AddAsync(order, ct);
            await _uow.CommitAsync(ct);
            return order.OrderId;
        }
        
        public async Task<IEnumerable<OrderDto>> GetAllAsync(QueryParams qp, CancellationToken ct)
        {
            var query = _uow.Orders.Query()
                .Include(o => o.Customer)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(qp.Search))
                query = query.Where(o => o.Customer.Name.Contains(qp.Search));

            var result = await query
                .Skip(qp.Skip)
                .Take(qp.PageSize)
                .ToListAsync(ct);

            return _mapper.Map<IEnumerable<OrderDto>>(result);
        }
        
        public async Task<OrderDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.Orders.Query()
                .Include(o => o.Customer)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id, ct);

            return _mapper.Map<OrderDto?>(entity);
        }
        
        public async Task UpdateAsync(int id, UpdateOrderDto dto, CancellationToken ct)
        {
            var entity = await _uow.Orders.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Order {id} not found");

            _mapper.Map(dto, entity);
            _uow.Orders.Update(entity);
            await _uow.CommitAsync(ct);
        }
        
        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.Orders.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Order {id} not found");

            _uow.Orders.Remove(entity);
            await _uow.CommitAsync(ct);
        }
        
        public async Task ConfirmAsync(int id, CancellationToken ct)
        {
            var order = await _uow.Orders.Query()
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id, ct);

            if (order == null)
                throw new KeyNotFoundException($"Order {id} not found");

            if (!order.Items.Any())
                throw new InvalidOperationException("Cannot confirm order with no items");

            order.Status = "Confirmed";
            var total = order.Items.Sum(i => i.Quantity * i.Product!.Price);

            var payment = new Payment
            {
                OrderId = order.OrderId,
                Amount = total,
                PaidAt = DateTime.UtcNow
            };

            await _uow.Payments.AddAsync(payment, ct);
            await _uow.CommitAsync(ct);
        }
    }
}
