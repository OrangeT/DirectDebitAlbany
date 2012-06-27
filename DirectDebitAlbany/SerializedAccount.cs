namespace OrangeTentacle.DirectDebitAlbany
{
    public interface ISerializedAccount
    {
        string Number { get; }
        string SortCode { get; }
        string Name { get; }
        string Line { get; }
    }

    public class SerializedAccount : ISerializedAccount
    {
        internal SerializedAccount()
        {}

        public string Number { get; internal set; }
        public string SortCode { get; internal set; }
        public string Name { get; internal set; }

        public virtual string Line 
        {
            get { return SortCode + Number + Name; }
        }
    }
}
