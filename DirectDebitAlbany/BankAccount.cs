namespace OrangeTentacle.DirectDebitAlbany
{
    public class BankAccount
    {
        public string Number { get; protected set; }
        public string SortCode { get; protected set; }
        public string Name { get; protected set; }

        public BankAccount(string number, string sortCode, string name)
        {
            Number = number;
            SortCode = sortCode;
            Name = name;
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
