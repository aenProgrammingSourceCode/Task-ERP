using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }=string.Empty;
        public DateTime CreatedDate { get; set; }
        public string BranchAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string BranchEmail { get; set; } = string.Empty;
        public string BranchPhone { get; set; } = string.Empty;
        public DateTime OpeningDate { get; set; }
        public bool IsActive { get; set; }

        // Foreign key property for Company
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
