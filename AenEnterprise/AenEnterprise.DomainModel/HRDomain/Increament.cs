using AenEnterprise.DomainModel.EmployeeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.HRDomain
{
    public class Increament
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
    }
}
