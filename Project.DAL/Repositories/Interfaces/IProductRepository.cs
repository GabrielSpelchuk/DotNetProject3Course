using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<decimal?> GetPriceAsync(int productId, CancellationToken ct);
    }
}
