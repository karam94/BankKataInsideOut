using System.Collections.Generic;

namespace BankKataInsideOut.Interfaces
{
    public interface IBankStatementPrinter
    {
        List<string> Print(Account account);
    }
}