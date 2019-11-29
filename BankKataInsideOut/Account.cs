using System.Collections.Generic;
using System.Linq;

namespace BankKataInsideOut
{
    public class Account
    {
        public int Balance { get; set; }
        public IList<Transaction> Transactions { get; set; }

        public Account(int currentBalance, IList<Transaction> currentTransactions)
        {
            Balance = currentBalance;
            Transactions = currentTransactions;
        }

        public int GetTotalBalance()
        {
            return Transactions.Sum(x => x.Amount);
        }
    }
}