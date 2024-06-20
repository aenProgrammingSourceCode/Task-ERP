using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public Transaction(decimal amount, decimal balanceBefore, decimal balanceAfter, string description, DateTime date)
        {
            Amount = amount;
            BalanceBefore = balanceBefore;
            BalanceAfter = balanceAfter;
            Description = description;
            Date = date;
        }
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        // Add the foreign key property
        public Guid BankAccountId { get; set; }

        // Navigation property
        public BankAccount BankAccount { get; set; }

       
    }
}
