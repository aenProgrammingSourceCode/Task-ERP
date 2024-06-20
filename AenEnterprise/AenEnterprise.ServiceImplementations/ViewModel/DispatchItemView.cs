using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.ViewModel
{
    public class DispatchItemView
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string VehicalNo { get; set; } = string.Empty;
        public decimal VehicalEmptyWeight { get; set; }
        public decimal VehicalLoadWeight { get; set; }
        public decimal DispatchQuantity { get; set; }
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
    }
}
