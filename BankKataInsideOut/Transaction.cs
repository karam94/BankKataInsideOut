using System;

namespace BankKataInsideOut
{
    public class Transaction
    {
        public DateTime Date { get; }
        public int Amount { get; }

        public Transaction(DateTime date, int amount)
        {
            Date = date;
            Amount = amount;
        }
    }
}