using AenEnterprise.DomainModel.EmployeeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.HRDomain
{
    public class Salary
    {
        public int Id { get; set; }
        public DateTime MonthYear { get; set; }
        public int TotalWorkDays { get; set; }
        public int DaysPresent { get; set; }
        public int DaysAbsent { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal SalaryPerDay { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal GrossSalary
        {
            get { return BasicSalary + Allowances - Deductions; }
            set { }
        }
       

        public decimal NetSalary { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        // Add more properties as needed

        public void CalculateNetSalary()
        {
            // Calculate the net salary based on your business logic
            NetSalary = GrossSalary; 
        }
        
    }
}
