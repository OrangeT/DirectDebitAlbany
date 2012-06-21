namespace OrangeTentacle.DirectDebitAlbany
{
    public class Destination : BankAccount
    {
        public Destination(string number, string sortCode, string name)
            : base(number, sortCode, name)
        {
        }
    }
}
