using AenEnterprise.DomainModel.EmployeeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.HRDomain
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public bool IsHoliday { get; set; }
        public int HoursWorked { get; set; }
        public string WorkShift { get; set; } = string.Empty;
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        // Add more properties as needed
    }
}
