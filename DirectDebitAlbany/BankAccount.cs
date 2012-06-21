using System.Text.RegularExpressions;

namespace OrangeTentacle.DirectDebitAlbany
{
    public class BankAccount
    {
        public string Number { get; protected set; }
        public string SortCode { get; protected set; }
        public string Name { get; protected set; }

        private BankAccount(string number, string sortCode, string name,
                int lower, int higher)
        {
            var numberVal = string.Format(@"^\d{{{0},{1}}}$", lower, higher);
            //var numberVal = string.Format(@"^\d{6,8}$", lower, higher);
            if (! Regex.IsMatch(number, numberVal))
                throw new DirectDebitException(string.Format(
                            "Number Must Be Between {0} and {1} digits",
                            lower, higher));

            var sortcodeVal = @"^\d{6}$";
            if (! Regex.IsMatch(sortCode, sortcodeVal))
                throw new DirectDebitException("Sort Code Must Be 6 digits");

            var nameVal = @"^[0-9A-Za-z\s]+$";
            if (! Regex.IsMatch(name, nameVal))
                throw new DirectDebitException("Name Must Be Alpha Numeric");

            Number = number;
            SortCode = sortCode;
            Name = name;
           
        }

        public BankAccount(string number, string sortCode, string name)
            : this(number, sortCode, name, 6, 8)
        {
        }

        public BankAccount(string number, string sortCode, string name, Bank bank)
            : this(number, sortCode, name, 6, 10)
        {
        }

        public override bool Equals(object obj)
        {
            var bankAccount = obj as BankAccount;
            if (bankAccount == null)
                return false;

            return Number.Equals(bankAccount.Number) 
                && SortCode.Equals(bankAccount.SortCode);
        }
    }
}
