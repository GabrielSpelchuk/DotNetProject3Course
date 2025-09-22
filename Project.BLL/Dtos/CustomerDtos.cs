using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Dtos
{
    public class CustomerDtos
    {
        public record CustomerDto(int CustomerId, string Name, string Email, DateTime CreatedAt);
        public record CreateCustomerDto(string Name, string Email);
    }
}
