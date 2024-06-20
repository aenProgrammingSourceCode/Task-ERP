using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel
{
    public class BankAccount
    {
        public BankAccount()
        {
            Id = Guid.NewGuid();
            Balance = 0;
            CustomerRef = "";
            Transactions = new List<Transaction>();
            Transactions.Add(new Transaction(0m, 0m, 0m, "account created", DateTime.Now));
        }
        public BankAccount(Guid id, decimal balance, List<Transaction> transactions, string customerRef)
        {
            Id = id;
            Balance = balance;
            CustomerRef = customerRef;
            Transactions = transactions;
        }
        public Guid Id { get; set; }
        public decimal Balance { get; private set; }
        public string CustomerRef { get; set; }
        public List<Transaction> Transactions { get; private set; }

        public bool CanWithdraw(decimal amount)
        {
            return (Balance >= amount);
        }

        public void Withdraw(decimal amount, string reference)
        {
            if (CanWithdraw(amount))
            {
                decimal balanceBefore = Balance;
                Balance -= amount;
                decimal balanceAfter = Balance;
                Transactions.Add(new Transaction(amount, balanceBefore, balanceAfter, reference, DateTime.Now));
            }
        }

        public void Deposit(decimal amount, string reference)
        {
            decimal balanceBefore = Balance;
            Balance += amount;
            decimal balanceAfter = Balance;
            Transactions.Add(new Transaction(amount, balanceBefore, balanceAfter, reference, DateTime.Now));
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return Transactions;
        }
    }
}


//    class Program
//    {
//        static void Main(string[] args)
//        {
//            using (var context = new YourDbContext())
//            {
//                // Create a new BankAccount
//                var newAccount = new BankAccount();
//                context.BankAccounts.Add(newAccount);
//                context.SaveChanges();

//                // Retrieve a BankAccount by its AccountNo (assuming you know the GUID)
//                Guid accountId = newAccount.AccountNo;
//                var retrievedAccount = context.BankAccounts.FirstOrDefault(b => b.AccountNo == accountId);

//                if (retrievedAccount != null)
//                {
//                    // Use the BankAccount
//                    Console.WriteLine($"Account Balance: {retrievedAccount.Balance}");
//                    retrievedAccount.Deposit(100, "Deposit transaction");
//                    retrievedAccount.Withdraw(50, "Withdraw transaction");

//                    // Save changes to the database
//                    context.SaveChanges();

//                    // Retrieve transactions for the account
//                    var transactions = retrievedAccount.GetTransactions();
//                    foreach (var transaction in transactions)
//                    {
//                        Console.WriteLine($"Transaction: {transaction.Description}, Amount: {transaction.Amount}");
//                    }
//                }
//            }
//        }
//    }

//}
 
