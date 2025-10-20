using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Dtos.Products;
using Project.BLL.Query;
using Project.DAL.Uow;
using Project.Domain.Entities;

namespace Project.BLL.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateProductDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Product>(dto);
            await _uow.Products.AddAsync(entity, ct);
            await _uow.CommitAsync(ct);
            return entity.ProductId;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(QueryParams qp, CancellationToken ct)
        {
            var query = _uow.Products.Query()
                .Include(p => p.Supplier)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(qp.Search))
                query = query.Where(p => p.Name.Contains(qp.Search));

            var list = await query
                .Skip(qp.Skip)
                .Take(qp.PageSize)
                .ToListAsync(ct);

            return _mapper.Map<IEnumerable<ProductDto>>(list);
        }

        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.Products.Query()
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == id, ct);

            return _mapper.Map<ProductDto?>(entity);
        }

        public async Task UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct)
        {
            var entity = await _uow.Products.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Product {id} not found");

            _mapper.Map(dto, entity);
            _uow.Products.Update(entity);
            await _uow.CommitAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.Products.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Product {id} not found");

            _uow.Products.Remove(entity);
            await _uow.CommitAsync(ct);
        }
    }
}
