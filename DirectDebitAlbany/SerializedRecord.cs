namespace OrangeTentacle.DirectDebitAlbany
{
    public interface ISerializedRecord
    {
        string TransCode { get; }
        string Amount { get; }
        string Reference { get; }
        string Originator { get; }
        string Destination { get; }
        string Line();
    }

    public class SerializedRecord : ISerializedRecord
    {
        internal SerializedRecord()
        {}

        public string TransCode { get; internal set; }
        public string Amount { get; internal set; }
        public string Reference { get; internal set; }
        public string Originator { get; internal set; }
        public string Destination { get; internal set; }

        public string Line()
        {
            return Destination + TransCode + Originator + Amount + Reference;
        }
    }
}
