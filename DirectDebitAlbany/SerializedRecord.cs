namespace OrangeTentacle.DirectDebitAlbany
{
    public interface ISerializedRecord
    {
        string TransCode { get; }
        string Amount { get; }
        string Reference { get; }
        string Originator { get; }
        string Destination { get; }
        string Line { get; }
    }

    public class SerializedRecord : ISerializedRecord
    {
        internal SerializedRecord()
        {}

        public readonly static string[] DEFAULT_FIELDS = new [] { "Destination", "TransCode", 
            "Originator", "Amount", "Reference" };

        public string TransCode { get; internal set; }
        public string Amount { get; internal set; }
        public string Reference { get; internal set; }
        public string Originator { get; internal set; }
        public string Destination { get; internal set; }
        public string Line { get; internal set; }
    }
}
