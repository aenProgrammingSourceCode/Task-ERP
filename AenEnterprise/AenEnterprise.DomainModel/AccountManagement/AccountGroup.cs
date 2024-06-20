using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.AccountManagement
{
    public class AccountGroup
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int BranchId { get; set; }
        public int AccountTypeId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime Dated { get; set; }
        public int UserId { get; set; }
        public int BSLineId { get; set; }
    }
}
