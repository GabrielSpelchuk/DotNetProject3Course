using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Dtos.Orders;
using Project.DAL.Uow;
using Project.Domain.Entities;

namespace Project.BLL.Services
{
    public class PaymentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CreatePaymentDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Payment>(dto);
            await _uow.Payments.AddAsync(entity, ct);
            await _uow.CommitAsync(ct);
            return entity.PaymentId;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _uow.Payments.Query()
                .Include(p => p.Order)
                .ToListAsync(ct);

            return _mapper.Map<IEnumerable<PaymentDto>>(list);
        }

        public async Task<PaymentDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.Payments.GetByIdAsync(id, ct);
            return _mapper.Map<PaymentDto?>(entity);
        }
    }
}