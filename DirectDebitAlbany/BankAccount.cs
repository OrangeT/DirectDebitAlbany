using System.Text.RegularExpressions;
using System.Linq;

namespace OrangeTentacle.DirectDebitAlbany
{
    public interface IBankAccount
    {
         string Number { get; }
         string SortCode { get; }
         string Name { get; }
         Bank? Bank { get; }
         bool Equals(object obj);
         ISerializedAccount Serialize();
    }

    public class BankAccount : IBankAccount
    {
        public string Number { get; protected set; }
        public string SortCode { get; protected set; }
        public string Name { get; protected set; }
        public Bank? Bank { get; protected set; }

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
            if (number.Length == 9 && ! BankValidation.Nine.Contains(bank))
                throw new DirectDebitException("Bank Invalid for length of number");

            if (number.Length == 10 && ! BankValidation.Ten.Contains(bank))
                throw new DirectDebitException("Bank Invalid for length of number");

            Bank = bank;
        }

        public override bool Equals(object obj)
        {
            var bankAccount = obj as BankAccount;
            if (bankAccount == null)
                return false;

            return Number.Equals(bankAccount.Number) 
                && SortCode.Equals(bankAccount.SortCode);
        }

        public ISerializedAccount Serialize()
        {
            var account = new SerializedAccount();
            if (Number.Length <= 8)
                account.Number = Number.PadLeft(8, '0');
            if (Number.Length == 9)
                account.Number = Number.Substring(1, 8);
            if (Number.Length == 10 && Bank == DirectDebitAlbany.Bank.Natwest)
                account.Number = Number.Substring(2, 8);
            if (Number.Length == 10 && Bank == DirectDebitAlbany.Bank.Coop)
                account.Number = Number.Substring(0, 8);

            account.SortCode = SortCode;

            account.Name = Name.FixedWidth(18);

            return account;
        }
    }
}
