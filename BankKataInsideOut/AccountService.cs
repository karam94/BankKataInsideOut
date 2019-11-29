using BankKataInsideOut.Interfaces;

namespace BankKataInsideOut
{
    public class AccountService : IAccountService
    {
        private readonly Account _account;
        private readonly IBankStatementPrinter _bankStatementPrinter;
        private readonly IDateProvider _dateProvider;

        public AccountService(Account account, IBankStatementPrinter bankStatementPrinter, IDateProvider dateProvider)
        {
            _account = account;
            _bankStatementPrinter = bankStatementPrinter;
            _dateProvider = dateProvider;
        }

        public void Deposit(int amount)
        {
            _account.Balance += amount;
            RecordTransaction(amount);
        }

        public void Withdraw(int amount)
        {
            _account.Balance -= amount;
            RecordTransaction(-amount);
        }

        public void PrintStatement()
        {
            _bankStatementPrinter.Print(_account);
        }

        private void RecordTransaction(int transactionAmount)
        {
            _account.Transactions.Add(new Transaction(_dateProvider.Today(), transactionAmount));
        }
    }
}