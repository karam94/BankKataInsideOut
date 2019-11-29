using System;
using BankKataInsideOut.Interfaces;

namespace BankKataInsideOut
{
    public class DateProvider : IDateProvider
    {
        public DateTime Today()
        {
            return DateTime.Today;
        }
    }
}