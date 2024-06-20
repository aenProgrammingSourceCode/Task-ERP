using AenEnterprise.DomainModel.HRDomain;
using AenEnterprise.DomainModel.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.EmployeeManagement
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }=string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string HairColor { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string IdentificationMark { get; set; } = string.Empty;
        public string Favourite { get; set; } = string.Empty;
        public string TAXIDNo { get; set; } = string.Empty;
        public DateTime DateOfExpiry { get; set; }
        public List<Warehouse> Warehouses { get; set; }
        public List<Attendance> Attendances { get; set; }
        public List<Salary> Salaries { get; set; }
        public List<Increament> Increaments { get; set; }
        public List<Department>? Departments { get; set; }
        public List<Leave>? Leaves { get; set; }
    }
}
