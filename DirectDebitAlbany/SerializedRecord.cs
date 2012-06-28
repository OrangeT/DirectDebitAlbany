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
        string Line(DirectDebitConfiguration config);
        string Line(string[] fields);
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

        public string Line()
        {
            return Line(DEFAULT_FIELDS);
        }

        public string Line(DirectDebitConfiguration config)
        {
            return Line(config.Record.GetProperties());
        }

        public string Line(string[] fields)
        {
            return Sugar.ComposeLine<SerializedRecord>(fields, this);
        }
    }
}
