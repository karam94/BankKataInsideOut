using System;
using System.Collections.Generic;
using BankKataInsideOut;
using BankKataInsideOut.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace BankKataInsideOutTests
{
    public class GivenAccountService
    {
        private BankStatementPrinter _bankStatementPrinter;
        private IDateProvider _dateProvider;
        private IDateProvider _fakeDateProvider;

        [SetUp]
        public void Setup()
        {
            _bankStatementPrinter = new BankStatementPrinter();
            _dateProvider = new DateProvider();
            _fakeDateProvider = Substitute.For<IDateProvider>();
        }

        [TestCase(0, 1000, 1000)]
        [TestCase(100, 1000, 1100)]
        [TestCase(-200, 1000, 800)]
        public void WhenDepositAccountBalanceIncreases(int startingBalance, int depositAmount, int expectedEndBalance)
        {
            // Arrange
            var account = new Account(startingBalance, new List<Transaction>());
            var accountService = new AccountService(account, _bankStatementPrinter, _dateProvider);

            // Act
            accountService.Deposit(depositAmount);

            // Assert
            account.Balance.ShouldBe(expectedEndBalance);
        }

        [TestCase(0, 500, -500)]
        [TestCase(1000, 500, 500)]
        [TestCase(1000, 10, 990)]
        public void WhenWithdrawAccountBalanceDecreases(int startingBalance, int withdrawAmount, int expectedEndBalance)
        {
            // Arrange
            var account = new Account(startingBalance, new List<Transaction>());
            var accountService = new AccountService(account, _bankStatementPrinter, _dateProvider);

            // Act
            accountService.Withdraw(withdrawAmount);

            // Assert
            account.Balance.ShouldBe(expectedEndBalance);
        }

        [TestCase(0, 1000, 2000, 500, 0, 2500)]
        [TestCase(100, 1000, 2000, 500, 100, 2500)]
        public void WhenDepositingThenWithdrawingAccountBalanceAsExpected(int startingBalance, int firstDeposit, int secondDeposit, 
            int firstWithdrawal, int secondWithdrawal, int expectedEndBalance)
        {
            // Arrange
            var account = new Account(startingBalance, new List<Transaction>());
            var accountService = new AccountService(account, _bankStatementPrinter, _dateProvider);

            // Act
            accountService.Deposit(firstDeposit);
            accountService.Deposit(secondDeposit);
            accountService.Withdraw(firstWithdrawal);
            accountService.Withdraw(secondWithdrawal);

            // Assert
            account.Balance.ShouldBe(expectedEndBalance);
        }

        [TestCase(0, 1000, 2000, 500)]
        public void WhenPrintingStatementThenFinalStatementAsExpected(int startingBalance, int firstDeposit, int secondDeposit, int firstWithdrawal)
        {
            // Arrange
            var account = new Account(startingBalance, new List<Transaction>());
            var accountService = new AccountService(account, _bankStatementPrinter, _fakeDateProvider);

            // Act
            _fakeDateProvider.Today()
                .Returns(DateTime.ParseExact("10/01/2012", "dd/MM/yyyy", null));
            accountService.Deposit(firstDeposit);

            _fakeDateProvider.Today()
                .Returns(DateTime.ParseExact("13/01/2012", "dd/MM/yyyy", null));
            accountService.Deposit(secondDeposit);

            _fakeDateProvider.Today()
                .Returns(DateTime.ParseExact("14/01/2012", "dd/MM/yyyy", null));
            accountService.Withdraw(firstWithdrawal);

            accountService.PrintStatement();

            // Assert
            _bankStatementPrinter.Print(account)[0].ShouldBe("Date || Amount || Balance");
            _bankStatementPrinter.Print(account)[1].ShouldBe("14/01/2012 || -500 || 2500");
            _bankStatementPrinter.Print(account)[2].ShouldBe("13/01/2012 || 2000 || 3000");
            _bankStatementPrinter.Print(account)[3].ShouldBe("10/01/2012 || 1000 || 1000");
        }
    }
}