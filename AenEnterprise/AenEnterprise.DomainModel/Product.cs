using AenEnterprise.DomainModel.Inventory;
using AenEnterprise.DomainModel.PurchaseManagement;
using AenEnterprise.DomainModel.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UnitId { get; set; }
        public string? DefaultVatPercent { get; set; } = string.Empty;
        public decimal? PurchasePrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? WholesalePrice { get; set; }
        public decimal? MRP { get; set; }
        public decimal? TradePrice { get; set; }
        public string? InventoryType { get; set; }
        public bool? IsQuantityMeasureable { get; set; }
        public bool? FixedAsset { get; set; }
        public bool? IsPurchaseable { get; set; }
        public bool? IsSaleable { get; set; }
        public bool? IsConsumable { get; set; }
        public bool? RawMaterials { get; set; }
        public bool? IsInventoryRequired { get; set; }
        public int WarehouseId { get; set; }
        public int CompanyId { get; set; }
        public Warehouse Warehouse { get; set; }
        public Company Company { get; set; }
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public List<PurchaseItem>? PurchaseItems { get; set; }
    }
}
