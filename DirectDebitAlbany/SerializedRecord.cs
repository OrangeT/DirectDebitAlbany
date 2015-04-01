namespace OrangeTentacle.DirectDebitAlbany
{
    public interface ISerializedRecord
    {
        string TransCode { get; }
        string Amount { get; }
        string Reference { get; }
        ISerializedAccount Originator { get; }
        ISerializedAccount Destination { get; }
        string Line { get; }
    }

    public class SerializedRecord : ISerializedRecord
    {
        internal SerializedRecord()
        {}

        public readonly static string[] DEFAULT_FIELDS = new [] { "Destination.Line", "TransCode", 
            "Originator.Line", "Amount", "Reference" };

        public string TransCode { get; internal set; }
        public string Amount { get; internal set; }
        public string Reference { get; internal set; }
        public ISerializedAccount Originator { get; internal set; }
        public ISerializedAccount Destination { get; internal set; }
        public string Line { get; internal set; }
    }
}
