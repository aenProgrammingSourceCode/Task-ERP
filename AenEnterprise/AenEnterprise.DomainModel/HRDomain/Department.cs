using AenEnterprise.DomainModel.EmployeeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.HRDomain
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Other department-related properties
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
