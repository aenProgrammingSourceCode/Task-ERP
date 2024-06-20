using AenEnterprise.DataAccess;
using AenEnterprise.DomainModel;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    //public class BankTransaction
    //{
    //    public static void BankReturn()
    //    {
    //        using (var context = new YourDbContext())
    //        {
    //            // Create a new BankAccount
    //            var newAccount = new BankAccount();
    //            context.BankAccounts.Add(newAccount);
    //            context.SaveChanges();

    //            // Retrieve a BankAccount by its AccountNo (assuming you know the GUID)
    //            Guid accountId = newAccount.AccountNo;
    //            var retrievedAccount = context.BankAccounts.FirstOrDefault(b => b.AccountNo == accountId);

    //            if (retrievedAccount != null)
    //            {
    //                // Use the BankAccount
    //                Console.WriteLine($"Account Balance: {retrievedAccount.Balance}");
    //                retrievedAccount.Deposit(100, "Deposit transaction");
    //                retrievedAccount.Withdraw(50, "Withdraw transaction");

    //                // Save changes to the database
    //                context.SaveChanges();

    //                // Retrieve transactions for the account
    //                var transactions = retrievedAccount.GetTransactions();
    //                foreach (var transaction in transactions)
    //                {
    //                    Console.WriteLine($"Transaction: {transaction.Description}, Amount: {transaction.Amount}");
    //                }
    //            }
    //        }
    //    }
    //}
}



