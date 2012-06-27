using System;

namespace OrangeTentacle.DirectDebitAlbany
{
    public interface ISerializedAccount
    {
        string Number { get; }
        string SortCode { get; }
        string Name { get; }
        string Line();
        string Line(string[] fields);
    }

    public class SerializedAccount : ISerializedAccount
    {
        internal SerializedAccount()
        {}

        public readonly string[] DEFAULT_FIELDS = new [] { "SortCode", "Number", "Name" };

        public string Number { get; internal set; }
        public string SortCode { get; internal set; }
        public string Name { get; internal set; }

        public string Line()
        {
            return Line(DEFAULT_FIELDS);
        }

        public string Line(string[] fields)
        {
            return Sugar.ComposeLine<SerializedAccount>(fields, this);
        }
    }
}
