namespace OrangeTentacle.DirectDebitAlbany
{
    public class Originator : BankAccount
    {
        public Originator(string number, string sortCode, string name)
            : base(number, sortCode, name)
        {
        }
    }
}
