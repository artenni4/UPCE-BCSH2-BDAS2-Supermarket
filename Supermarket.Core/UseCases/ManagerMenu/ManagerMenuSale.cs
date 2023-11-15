using Supermarket.Core.Domain.PaymentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public class ManagerMenuSale
    {
        public required int Id { get; set; }
        public required DateTime Date { get; set; }
        public required string Products { get; set; }
        public required decimal Price { get; set; }
        public required string PaymentType { get; set; }
        public required int CashboxId { get; set; }
        public required string CashboxName { get; set; }
    }
}
