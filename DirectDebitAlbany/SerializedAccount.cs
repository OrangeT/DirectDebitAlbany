namespace OrangeTentacle.DirectDebitAlbany
{
    public interface ISerializedAccount
    {
        string Number { get; }
        string SortCode { get; }
        string Name { get; }
        string Line();
    }

    public class SerializedAccount : ISerializedAccount
    {
        internal SerializedAccount()
        {}

        public string Number { get; internal set; }
        public string SortCode { get; internal set; }
        public string Name { get; internal set; }

        public string Line()
        {
            return SortCode + Number + Name; 
        }
    }
}
