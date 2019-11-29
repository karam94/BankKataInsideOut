using System.Collections.Generic;
using System.Linq;
using BankKataInsideOut.Interfaces;

namespace BankKataInsideOut
{
    public class BankStatementPrinter : IBankStatementPrinter
    {
        private List<string> _statementLines;

        public BankStatementPrinter()
        {
            _statementLines = new List<string>();
        }

        public List<string> Print(Account account)
        {
            var runningBalance = 0;
            var totalAccountBalance = account.GetTotalBalance();

            _statementLines.Add("Date || Amount || Balance");

            foreach (var transaction in account.Transactions.OrderByDescending(x => x.Date))
            {
                _statementLines.Add($"{transaction.Date:dd'/'MM'/'yyyy} || {transaction.Amount} || {totalAccountBalance - runningBalance}");
                runningBalance += transaction.Amount;
            }

            return _statementLines;
        }
    }
}