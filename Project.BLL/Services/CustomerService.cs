using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Dtos.Customers;
using Project.BLL.Query;
using Project.DAL.Uow;
using Project.Domain.Entities;

namespace Project.BLL.Services
{
    public class CustomerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateCustomerDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Customer>(dto);
            await _uow.Customers.AddAsync(entity, ct);
            await _uow.CommitAsync(ct);
            return entity.CustomerId;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync(QueryParams qp, CancellationToken ct)
        {
            var query = _uow.Customers.Query();

            if (!string.IsNullOrWhiteSpace(qp.Search))
                query = query.Where(c => c.Name.Contains(qp.Search));

            var list = await query.Skip(qp.Skip).Take(qp.PageSize).ToListAsync(ct);
            return _mapper.Map<IEnumerable<CustomerDto>>(list);
        }

        public async Task<CustomerDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.Customers.GetByIdAsync(id, ct);
            return _mapper.Map<CustomerDto?>(entity);
        }

        public async Task UpdateAsync(int id, UpdateCustomerDto dto, CancellationToken ct)
        {
            var entity = await _uow.Customers.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Customer {id} not found");

            _mapper.Map(dto, entity);
            _uow.Customers.Update(entity);
            await _uow.CommitAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.Customers.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Customer {id} not found");

            _uow.Customers.Remove(entity);
            await _uow.CommitAsync(ct);
        }
    }
}
