using AenEnterprise.DomainModel.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string TaxIdentifier { get; set; } = string.Empty;
        public string IndustryType { get; set; } = string.Empty;
        public string CompanyAddress { get; set; } = string.Empty;
        public string Country  { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
        public string CompanyPhone { get; set; } = string.Empty;
        public DateTime IncorporationDate { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string InvoiceAddress { get; set; } = string.Empty;
        public string WebPageAddress { get; set; } = string.Empty;
        public string VATCode { get; set; } = string.Empty;
        public string VATAreaCode { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }
        public decimal AnnualRevenue { get; set; }
        public bool IsPubliclyListed { get; set; }
        public bool IsMultipleWareHouse { get; set; }
        public List<Product> Products { get; set; }
        public List<Warehouse> Warehouses { get; set; }
    }
}
