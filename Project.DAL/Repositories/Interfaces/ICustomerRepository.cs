using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> CreateAsync(Customer customer, CancellationToken ct);
        Task<Customer> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken ct);
    }
}
