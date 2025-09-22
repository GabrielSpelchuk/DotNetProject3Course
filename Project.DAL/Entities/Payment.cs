using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities
{
    public class Payment { 
        public int PaymentId { get; set; } 
        public int OrderId { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime PaidAt { get; set; } 
    }
}
