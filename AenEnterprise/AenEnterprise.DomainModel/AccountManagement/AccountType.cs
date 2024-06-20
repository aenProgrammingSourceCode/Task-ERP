using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.AccountManagement
{
    public class AccountType
    {
        public int Id{ get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime Dated { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }
}
