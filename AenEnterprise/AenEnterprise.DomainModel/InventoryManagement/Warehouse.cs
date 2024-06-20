using AenEnterprise.DomainModel.EmployeeManagement;
using AenEnterprise.DomainModel.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.Inventory
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CostingMethodId { get; set; }
        public string? Address1 { get; set; } = string.Empty;
        public string? Address2 { get; set; }= string.Empty;
        public string? PhoneNo { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<Product> Products { get; set; }
        public List<Stock> Stocks { get; set; }

    }
}
