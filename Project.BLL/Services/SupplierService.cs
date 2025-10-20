using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Dtos.Suppliers;
using Project.BLL.Query;
using Project.DAL.Uow;
using Project.Domain.Entities;

namespace Project.BLL.Services
{
    public class SupplierService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateSupplierDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Supplier>(dto);
            await _uow.Suppliers.AddAsync(entity, ct);
            await _uow.CommitAsync(ct);
            return entity.SupplierId;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync(QueryParams qp, CancellationToken ct)
        {
            var query = _uow.Suppliers.Query();

            if (!string.IsNullOrWhiteSpace(qp.Search))
                query = query.Where(s => s.Name.Contains(qp.Search));

            var list = await query.Skip(qp.Skip).Take(qp.PageSize).ToListAsync(ct);
            return _mapper.Map<IEnumerable<SupplierDto>>(list);
        }

        public async Task<SupplierDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.Suppliers.GetByIdAsync(id, ct);
            return _mapper.Map<SupplierDto?>(entity);
        }

        public async Task UpdateAsync(int id, UpdateSupplierDto dto, CancellationToken ct)
        {
            var entity = await _uow.Suppliers.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Supplier {id} not found");

            _mapper.Map(dto, entity);
            _uow.Suppliers.Update(entity);
            await _uow.CommitAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.Suppliers.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Supplier {id} not found");

            _uow.Suppliers.Remove(entity);
            await _uow.CommitAsync(ct);
        }
    }
}
