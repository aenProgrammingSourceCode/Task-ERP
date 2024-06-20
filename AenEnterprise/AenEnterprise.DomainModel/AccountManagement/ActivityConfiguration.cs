using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.AccountManagement
{
    public class ActivityConfiguration
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int StoreId { get; set; }
        public string ActivityId { get; set; }=string.Empty;
        public string TransactionHeads { get; set; } = string.Empty;
        public string AccountNo { get; set; } = string.Empty;
        public bool IsDebit { get; set; }
        public bool IsCredit { get; set; }
    }
}
